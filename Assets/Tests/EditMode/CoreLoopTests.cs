using Game;
using Game.CoreLoop;
using NUnit.Framework;

public sealed class CoreLoopTests
{
    // 测试：默认规则应符合迭代 1 的比赛常量。
    [Test]
    public void CreateDefault_ReturnsIterationOneRuleValues()
    {
        CoreLoopSettings settings = CoreLoopSettings.CreateDefault();

        Assert.AreEqual(9, settings.MaxMajorRounds);
        Assert.AreEqual(5, settings.ScoreToWin);
        Assert.AreEqual(3, settings.BulletsPerAttack);
        Assert.AreEqual(4f, settings.DeploymentDurationSeconds);
        Assert.AreEqual(8f, settings.StalemateWarningSeconds);
        Assert.AreEqual(11f, settings.StalemateForceSwitchSeconds);
    }

    // 测试：开始新比赛后应进入第一大回合部署阶段。
    [Test]
    public void StartNewMatch_PlayerAFirst_EntersDeployment()
    {
        CoreLoopController controller = CreateStartedController();

        CoreLoopSnapshot snapshot = controller.Snapshot;

        Assert.AreEqual(CoreLoopPhase.Deployment, snapshot.Phase);
        Assert.AreEqual(1, snapshot.MajorRoundNumber);
        Assert.AreEqual(PlayerSide.PlayerA, snapshot.CurrentAttacker);
        Assert.AreEqual(PlayerSide.PlayerB, snapshot.CurrentDefender);
        Assert.AreEqual(3, snapshot.BulletsRemaining);
        Assert.AreEqual(4f, snapshot.DeploymentSecondsRemaining);
    }

    // 测试：部署计时结束后应自动进入攻击阶段。
    [Test]
    public void Tick_DeploymentDurationElapsed_EntersAttack()
    {
        CoreLoopController controller = CreateStartedController();

        controller.Tick(4f);

        CoreLoopSnapshot snapshot = controller.Snapshot;
        Assert.AreEqual(CoreLoopPhase.Attack, snapshot.Phase);
        Assert.AreEqual(3, snapshot.BulletsRemaining);
        Assert.AreEqual(0f, snapshot.DeploymentSecondsRemaining);
    }

    // 测试：手动结束部署阶段也应进入攻击阶段。
    [Test]
    public void FinishDeployment_DeploymentPhase_EntersAttack()
    {
        CoreLoopController controller = CreateStartedController();

        controller.FinishDeployment();

        Assert.AreEqual(CoreLoopPhase.Attack, controller.Snapshot.Phase);
    }

    // 测试：三发未命中后攻防换手并进入新部署阶段。
    [Test]
    public void RecordShot_ThreeMisses_SwitchesAttacker()
    {
        CoreLoopController controller = CreateStartedController();
        controller.FinishDeployment();

        controller.RecordShot(false);
        controller.RecordShot(false);
        controller.RecordShot(false);

        CoreLoopSnapshot snapshot = controller.Snapshot;
        Assert.AreEqual(CoreLoopPhase.Deployment, snapshot.Phase);
        Assert.AreEqual(PlayerSide.PlayerB, snapshot.CurrentAttacker);
        Assert.AreEqual(1, snapshot.MajorRoundNumber);
        Assert.AreEqual(3, snapshot.BulletsRemaining);
    }

    // 测试：命中应得分、进入下一大回合、切换先手并通知墙体重置版本。
    [Test]
    public void RecordShot_Hit_StartsNextMajorRoundAndResetsWalls()
    {
        CoreLoopController controller = CreateStartedController();
        controller.FinishDeployment();

        controller.RecordShot(true);

        CoreLoopSnapshot snapshot = controller.Snapshot;
        Assert.AreEqual(1, snapshot.PlayerAScore);
        Assert.AreEqual(2, snapshot.MajorRoundNumber);
        Assert.AreEqual(PlayerSide.PlayerB, snapshot.CurrentAttacker);
        Assert.AreEqual(CoreLoopPhase.Deployment, snapshot.Phase);
        Assert.AreEqual(1, snapshot.WallResetVersion);
    }

    // 测试：先到五分应立刻结束比赛。
    [Test]
    public void RecordShot_PlayerReachesFiveScore_EndsMatch()
    {
        CoreLoopController controller = CreateStartedController();

        ScorePlayerA(controller);
        ScorePlayerA(controller);
        ScorePlayerA(controller);
        ScorePlayerA(controller);
        ScorePlayerA(controller);

        CoreLoopSnapshot snapshot = controller.Snapshot;
        Assert.IsTrue(snapshot.MatchOver);
        Assert.AreEqual(CoreLoopPhase.MatchOver, snapshot.Phase);
        Assert.AreEqual(MatchEndReason.ScoreLimitReached, snapshot.MatchEndReason);
        Assert.AreEqual(PlayerSide.PlayerA, snapshot.Winner);
        Assert.AreEqual(5, snapshot.PlayerAScore);
    }

    // 测试：到达九个大回合上限时按当前比分结算。
    [Test]
    public void RecordShot_NinthRoundScoredBeforeFive_EndsByRoundLimit()
    {
        CoreLoopController controller = new CoreLoopController(
            new CoreLoopSettings(
                maxMajorRounds: 9,
                scoreToWin: 10,
                bulletsPerAttack: 3,
                deploymentDurationSeconds: 4f,
                stalemateWarningSeconds: 8f,
                stalemateForceSwitchSeconds: 11f));
        controller.StartNewMatch(PlayerSide.PlayerA);

        for (int i = 0; i < 9; i++)
        {
            ScoreCurrentAttacker(controller);
        }

        CoreLoopSnapshot snapshot = controller.Snapshot;
        Assert.IsTrue(snapshot.MatchOver);
        Assert.AreEqual(MatchEndReason.RoundLimitReached, snapshot.MatchEndReason);
        Assert.AreEqual(9, snapshot.MajorRoundNumber);
        Assert.AreEqual(PlayerSide.PlayerA, snapshot.Winner);
        Assert.AreEqual(9, snapshot.PlayerAScore + snapshot.PlayerBScore);
    }

    // 测试：8 秒未开火应给出反僵局警告。
    [Test]
    public void Tick_AttackEightSecondsWithoutShot_ActivatesStalemateWarning()
    {
        CoreLoopController controller = CreateStartedController();
        controller.FinishDeployment();

        controller.Tick(8f);

        Assert.IsTrue(controller.Snapshot.StalemateWarningActive);
        Assert.AreEqual(PlayerSide.PlayerA, controller.Snapshot.CurrentAttacker);
    }

    // 测试：11 秒未开火应强制换手。
    [Test]
    public void Tick_AttackElevenSecondsWithoutShot_ForcesSwitch()
    {
        CoreLoopController controller = CreateStartedController();
        controller.FinishDeployment();

        controller.Tick(11f);

        CoreLoopSnapshot snapshot = controller.Snapshot;
        Assert.AreEqual(CoreLoopPhase.Deployment, snapshot.Phase);
        Assert.AreEqual(PlayerSide.PlayerB, snapshot.CurrentAttacker);
        Assert.IsFalse(snapshot.StalemateWarningActive);
    }

    private static CoreLoopController CreateStartedController()
    {
        CoreLoopController controller = new CoreLoopController(CoreLoopSettings.CreateDefault());
        controller.StartNewMatch(PlayerSide.PlayerA);
        return controller;
    }

    private static void ScoreCurrentAttacker(CoreLoopController controller)
    {
        controller.FinishDeployment();
        controller.RecordShot(true);
    }

    private static void ScorePlayerA(CoreLoopController controller)
    {
        if (controller.Snapshot.CurrentAttacker == PlayerSide.PlayerB)
        {
            controller.FinishDeployment();
            controller.RecordShot(false);
            controller.RecordShot(false);
            controller.RecordShot(false);
        }

        controller.FinishDeployment();
        controller.RecordShot(true);
    }
}
