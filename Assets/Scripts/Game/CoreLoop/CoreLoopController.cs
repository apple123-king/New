using System;

namespace Game.CoreLoop
{
    /// <summary>
    /// 管理比赛、大回合、部署和单次进攻的纯 C# 状态机。
    /// </summary>
    public sealed class CoreLoopController : ICoreLoopReadOnly
    {
        private readonly CoreLoopSettings settings;

        private CoreLoopPhase phase;
        private int majorRoundNumber;
        private PlayerSide firstAttackerThisRound;
        private PlayerSide currentAttacker;
        private int playerAScore;
        private int playerBScore;
        private int bulletsRemaining;
        private float deploymentElapsedSeconds;
        private float secondsSinceLastShot;
        private bool stalemateWarningActive;
        private PlayerSide winner;
        private MatchEndReason matchEndReason;
        private int wallResetVersion;

        public CoreLoopController(CoreLoopSettings settings)
        {
            this.settings = settings ?? throw new ArgumentNullException(nameof(settings));
            phase = CoreLoopPhase.NotStarted;
        }

        public CoreLoopSnapshot Snapshot
        {
            get
            {
                return new CoreLoopSnapshot(
                    phase,
                    majorRoundNumber,
                    currentAttacker,
                    GetOpponent(currentAttacker),
                    playerAScore,
                    playerBScore,
                    bulletsRemaining,
                    GetDeploymentSecondsRemaining(),
                    secondsSinceLastShot,
                    stalemateWarningActive,
                    phase == CoreLoopPhase.MatchOver,
                    winner,
                    matchEndReason,
                    wallResetVersion);
            }
        }

        /// <summary>
        /// 以指定先攻方开始一局新比赛。
        /// </summary>
        public void StartNewMatch(PlayerSide firstAttacker)
        {
            if (firstAttacker == PlayerSide.None)
            {
                throw new ArgumentException("先攻方不能为 None。", nameof(firstAttacker));
            }

            majorRoundNumber = 1;
            firstAttackerThisRound = firstAttacker;
            currentAttacker = firstAttacker;
            playerAScore = 0;
            playerBScore = 0;
            winner = PlayerSide.None;
            matchEndReason = MatchEndReason.None;
            wallResetVersion = 0;

            StartDeployment();
        }

        /// <summary>
        /// 推进当前阶段计时。
        /// </summary>
        public void Tick(float deltaSeconds)
        {
            if (deltaSeconds < 0f)
            {
                throw new ArgumentOutOfRangeException(nameof(deltaSeconds), "时间增量不能为负数。");
            }

            if (phase == CoreLoopPhase.Deployment)
            {
                deploymentElapsedSeconds += deltaSeconds;
                if (deploymentElapsedSeconds >= settings.DeploymentDurationSeconds)
                {
                    FinishDeployment();
                }

                return;
            }

            if (phase != CoreLoopPhase.Attack)
            {
                return;
            }

            secondsSinceLastShot += deltaSeconds;
            if (secondsSinceLastShot >= settings.StalemateWarningSeconds)
            {
                stalemateWarningActive = true;
            }

            if (secondsSinceLastShot >= settings.StalemateForceSwitchSeconds)
            {
                SwitchAttacker();
            }
        }

        /// <summary>
        /// 手动结束部署阶段，进入可开火阶段。
        /// </summary>
        public void FinishDeployment()
        {
            EnsurePhase(CoreLoopPhase.Deployment);

            phase = CoreLoopPhase.Attack;
            bulletsRemaining = settings.BulletsPerAttack;
            deploymentElapsedSeconds = settings.DeploymentDurationSeconds;
            secondsSinceLastShot = 0f;
            stalemateWarningActive = false;
        }

        /// <summary>
        /// 记录一次开火，并根据是否命中推进比分、回合或换手。
        /// </summary>
        public void RecordShot(bool hit)
        {
            EnsurePhase(CoreLoopPhase.Attack);

            bulletsRemaining--;
            secondsSinceLastShot = 0f;
            stalemateWarningActive = false;

            if (hit)
            {
                ScoreForCurrentAttacker();
                return;
            }

            if (bulletsRemaining <= 0)
            {
                SwitchAttacker();
            }
        }

        private void StartDeployment()
        {
            phase = CoreLoopPhase.Deployment;
            bulletsRemaining = settings.BulletsPerAttack;
            deploymentElapsedSeconds = 0f;
            secondsSinceLastShot = 0f;
            stalemateWarningActive = false;
        }

        private void ScoreForCurrentAttacker()
        {
            if (currentAttacker == PlayerSide.PlayerA)
            {
                playerAScore++;
            }
            else
            {
                playerBScore++;
            }

            if (playerAScore >= settings.ScoreToWin || playerBScore >= settings.ScoreToWin)
            {
                EndMatch(MatchEndReason.ScoreLimitReached);
                return;
            }

            if (majorRoundNumber >= settings.MaxMajorRounds)
            {
                EndMatch(MatchEndReason.RoundLimitReached);
                return;
            }

            majorRoundNumber++;
            wallResetVersion++;
            firstAttackerThisRound = GetOpponent(firstAttackerThisRound);
            currentAttacker = firstAttackerThisRound;
            StartDeployment();
        }

        private void SwitchAttacker()
        {
            currentAttacker = GetOpponent(currentAttacker);
            StartDeployment();
        }

        private void EndMatch(MatchEndReason reason)
        {
            phase = CoreLoopPhase.MatchOver;
            bulletsRemaining = 0;
            secondsSinceLastShot = 0f;
            stalemateWarningActive = false;
            matchEndReason = reason;
            winner = ResolveWinner();
        }

        private PlayerSide ResolveWinner()
        {
            if (playerAScore > playerBScore)
            {
                return PlayerSide.PlayerA;
            }

            if (playerBScore > playerAScore)
            {
                return PlayerSide.PlayerB;
            }

            return PlayerSide.None;
        }

        private float GetDeploymentSecondsRemaining()
        {
            if (phase != CoreLoopPhase.Deployment)
            {
                return 0f;
            }

            return Math.Max(0f, settings.DeploymentDurationSeconds - deploymentElapsedSeconds);
        }

        private static PlayerSide GetOpponent(PlayerSide side)
        {
            return side == PlayerSide.PlayerA ? PlayerSide.PlayerB : PlayerSide.PlayerA;
        }

        private void EnsurePhase(CoreLoopPhase expectedPhase)
        {
            if (phase != expectedPhase)
            {
                throw new InvalidOperationException($"当前阶段必须是 {expectedPhase}，实际是 {phase}。");
            }
        }
    }
}
