using Game;
using Game.CoreLoop;
using Game.DodgerAndUI;
using NUnit.Framework;
using UnityEngine;

public sealed class DodgerAndUITests
{
    [Test]
    public void Move_UpdatesDodgerPosition()
    {
        DodgerController dodger = new DodgerController(new Vector3(1f, 0f, 2f));

        dodger.Move(new Vector3(0.5f, 0f, -1f));

        Assert.AreEqual(new Vector3(1.5f, 0f, 1f), dodger.Position);
    }

    [Test]
    public void SetPose_ChangesHitboxSize()
    {
        DodgerController dodger = new DodgerController(Vector3.zero);

        Vector3 standingSize = dodger.GetHitboxSize();
        dodger.SetPose(DodgerPose.Crouching);
        Vector3 crouchingSize = dodger.GetHitboxSize();
        dodger.SetPose(DodgerPose.Prone);
        Vector3 proneSize = dodger.GetHitboxSize();

        Assert.Greater(standingSize.y, crouchingSize.y);
        Assert.Greater(crouchingSize.y, proneSize.y);
        Assert.Greater(proneSize.z, standingSize.z);
    }

    [Test]
    public void LockDeploymentStart_CapturesCurrentPositionAndPose()
    {
        DodgerController dodger = new DodgerController(Vector3.zero);
        dodger.Move(new Vector3(3f, 0f, 4f));
        dodger.SetPose(DodgerPose.Prone);

        dodger.LockDeploymentStart();

        Assert.IsTrue(dodger.HasDeploymentLock);
        Assert.AreEqual(new Vector3(3f, 0f, 4f), dodger.LockedDeploymentPosition);
        Assert.AreEqual(DodgerPose.Prone, dodger.LockedDeploymentPose);
    }

    [Test]
    public void BuildAttackerHud_ExposesBulletsAndDeploymentMask()
    {
        DebugHudPresenter presenter = new DebugHudPresenter();
        CoreLoopSnapshot snapshot = CreateSnapshot(CoreLoopPhase.Deployment, bulletsRemaining: 3);

        RoleHudView hud = presenter.BuildHud(snapshot, PlayerViewRole.Attacker, GamePresentationMode.Secure);

        Assert.IsTrue(hud.ShowBullets);
        Assert.IsTrue(hud.ShowDeploymentMask);
        Assert.IsTrue(hud.ShowScopeStatus);
        Assert.AreEqual(3, hud.BulletsRemaining);
        Assert.AreEqual(PlayerSide.PlayerA, hud.CurrentAttacker);
    }

    [Test]
    public void BuildDodgerHud_SecureMode_HidesBulletsAndDebugOnlyState()
    {
        DebugHudPresenter presenter = new DebugHudPresenter();
        CoreLoopSnapshot snapshot = CreateSnapshot(CoreLoopPhase.Deployment, bulletsRemaining: 3);

        RoleHudView hud = presenter.BuildHud(snapshot, PlayerViewRole.Dodger, GamePresentationMode.Secure);

        Assert.IsFalse(hud.ShowBullets);
        Assert.IsFalse(hud.ShowDeploymentMask);
        Assert.IsFalse(hud.ShowShotSummary);
        Assert.AreEqual(0, hud.BulletsRemaining);
    }

    [Test]
    public void BuildDodgerHud_DebugMode_ExposesSharedDebugInfo()
    {
        DebugHudPresenter presenter = new DebugHudPresenter();
        CoreLoopSnapshot snapshot = CreateSnapshot(CoreLoopPhase.Deployment, bulletsRemaining: 3);

        RoleHudView hud = presenter.BuildHud(snapshot, PlayerViewRole.Dodger, GamePresentationMode.Debug);

        Assert.IsTrue(hud.ShowBullets);
        Assert.IsTrue(hud.ShowDeploymentMask);
        Assert.IsTrue(hud.ShowShotSummary);
        Assert.AreEqual(3, hud.BulletsRemaining);
    }

    private static CoreLoopSnapshot CreateSnapshot(CoreLoopPhase phase, int bulletsRemaining)
    {
        return new CoreLoopSnapshot(
            phase,
            majorRoundNumber: 1,
            currentAttacker: PlayerSide.PlayerA,
            currentDefender: PlayerSide.PlayerB,
            playerAScore: 0,
            playerBScore: 0,
            bulletsRemaining,
            deploymentSecondsRemaining: 4f,
            secondsSinceLastShot: 0f,
            stalemateWarningActive: false,
            matchOver: false,
            winner: PlayerSide.None,
            matchEndReason: MatchEndReason.None,
            wallResetVersion: 0);
    }
}
