namespace Game
{
    /// <summary>
    /// 本地双人切片中的玩家阵营。
    /// </summary>
    public enum PlayerSide
    {
        None = 0,
        PlayerA = 1,
        PlayerB = 2
    }

    /// <summary>
    /// CoreLoop 当前所处的比赛阶段。
    /// </summary>
    public enum CoreLoopPhase
    {
        NotStarted = 0,
        Deployment = 1,
        Attack = 2,
        MatchOver = 3
    }

    /// <summary>
    /// 比赛结束原因。
    /// </summary>
    public enum MatchEndReason
    {
        None = 0,
        ScoreLimitReached = 1,
        RoundLimitReached = 2
    }

    /// <summary>
    /// 运行时展示模式。Secure 为正式信息保密模式，Debug 为全信息调试模式。
    /// </summary>
    public enum GamePresentationMode
    {
        Secure = 0,
        Debug = 1
    }

    /// <summary>
    /// 当前 HUD 和视图所服务的角色。
    /// </summary>
    public enum PlayerViewRole
    {
        Attacker = 0,
        Dodger = 1
    }

    /// <summary>
    /// CoreLoop 对外暴露的只读状态快照。
    /// </summary>
    public readonly struct CoreLoopSnapshot
    {
        public CoreLoopSnapshot(
            CoreLoopPhase phase,
            int majorRoundNumber,
            PlayerSide currentAttacker,
            PlayerSide currentDefender,
            int playerAScore,
            int playerBScore,
            int bulletsRemaining,
            float deploymentSecondsRemaining,
            float secondsSinceLastShot,
            bool stalemateWarningActive,
            bool matchOver,
            PlayerSide winner,
            MatchEndReason matchEndReason,
            int wallResetVersion)
        {
            Phase = phase;
            MajorRoundNumber = majorRoundNumber;
            CurrentAttacker = currentAttacker;
            CurrentDefender = currentDefender;
            PlayerAScore = playerAScore;
            PlayerBScore = playerBScore;
            BulletsRemaining = bulletsRemaining;
            DeploymentSecondsRemaining = deploymentSecondsRemaining;
            SecondsSinceLastShot = secondsSinceLastShot;
            StalemateWarningActive = stalemateWarningActive;
            MatchOver = matchOver;
            Winner = winner;
            MatchEndReason = matchEndReason;
            WallResetVersion = wallResetVersion;
        }

        public CoreLoopPhase Phase { get; }

        public int MajorRoundNumber { get; }

        public PlayerSide CurrentAttacker { get; }

        public PlayerSide CurrentDefender { get; }

        public int PlayerAScore { get; }

        public int PlayerBScore { get; }

        public int BulletsRemaining { get; }

        public float DeploymentSecondsRemaining { get; }

        public float SecondsSinceLastShot { get; }

        public bool StalemateWarningActive { get; }

        public bool MatchOver { get; }

        public PlayerSide Winner { get; }

        public MatchEndReason MatchEndReason { get; }

        public int WallResetVersion { get; }
    }

    /// <summary>
    /// CoreLoop 的只读访问接口，供 Shooter、WallDestruction 和 UI 读取状态。
    /// </summary>
    public interface ICoreLoopReadOnly
    {
        CoreLoopSnapshot Snapshot { get; }
    }

    /// <summary>
    /// 供不同角色视图消费的最小 HUD 只读数据。
    /// </summary>
    public readonly struct RoleHudView
    {
        public RoleHudView(
            PlayerViewRole viewRole,
            GamePresentationMode presentationMode,
            int playerAScore,
            int playerBScore,
            int majorRoundNumber,
            PlayerSide currentAttacker,
            int bulletsRemaining,
            float deploymentSecondsRemaining,
            bool showBullets,
            bool showDeploymentMask,
            bool showScopeStatus,
            bool showShotSummary,
            bool showDodgerState)
        {
            ViewRole = viewRole;
            PresentationMode = presentationMode;
            PlayerAScore = playerAScore;
            PlayerBScore = playerBScore;
            MajorRoundNumber = majorRoundNumber;
            CurrentAttacker = currentAttacker;
            BulletsRemaining = bulletsRemaining;
            DeploymentSecondsRemaining = deploymentSecondsRemaining;
            ShowBullets = showBullets;
            ShowDeploymentMask = showDeploymentMask;
            ShowScopeStatus = showScopeStatus;
            ShowShotSummary = showShotSummary;
            ShowDodgerState = showDodgerState;
        }

        public PlayerViewRole ViewRole { get; }

        public GamePresentationMode PresentationMode { get; }

        public int PlayerAScore { get; }

        public int PlayerBScore { get; }

        public int MajorRoundNumber { get; }

        public PlayerSide CurrentAttacker { get; }

        public int BulletsRemaining { get; }

        public float DeploymentSecondsRemaining { get; }

        public bool ShowBullets { get; }

        public bool ShowDeploymentMask { get; }

        public bool ShowScopeStatus { get; }

        public bool ShowShotSummary { get; }

        public bool ShowDodgerState { get; }
    }
}
