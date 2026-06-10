using Game;
using Game.CoreLoop;
using Game.DodgerAndUI;
using Game.SceneIntegration;
using Game.Shooter;
using Game.WallDestruction;
using NUnit.Framework;
using UnityEngine;

public sealed class SceneIntegrationCommandBoundaryTests
{
    [Test]
    public void ApplyDodgerCommand_MovesAndChangesPose()
    {
        SceneCommandBoundary boundary = new SceneCommandBoundary();
        DodgerController dodger = new DodgerController(new Vector3(0f, 0.9f, 4.5f));

        boundary.ApplyDodgerCommand(dodger, new DodgerCommand(1f, 0f, DodgerPose.Crouching), 1f);

        Assert.Greater(dodger.Position.x, 0f);
        Assert.AreEqual(DodgerPose.Crouching, dodger.Pose);
    }

    [Test]
    public void ApplyAttackerCommand_NonAttackPhase_ClosesScopeAndDoesNotFire()
    {
        SceneCommandBoundary boundary = new SceneCommandBoundary();
        ShooterController shooter = new ShooterController(_ => { }, new StubWallShotResolver(false));
        shooter.OpenScope(Vector3.zero, Vector3.forward);

        ShooterShotResult? result = boundary.ApplyAttackerCommand(
            shooter,
            CreateSnapshot(CoreLoopPhase.Deployment),
            new AttackerCommand(scopeHeld: false, firePressed: true),
            ShooterController.ScopeWarmupSeconds,
            Vector3.zero,
            Vector3.forward,
            Vector3.zero);

        Assert.IsFalse(shooter.IsScoped);
        Assert.IsFalse(result.HasValue);
    }

    [Test]
    public void ApplyAttackerCommand_AttackPhaseAfterWarmup_FiresAuthoritativeShot()
    {
        SceneCommandBoundary boundary = new SceneCommandBoundary();
        bool reportedHit = false;
        ShooterController shooter = new ShooterController(hit => reportedHit = hit, new StubWallShotResolver(true));

        boundary.ApplyAttackerCommand(
            shooter,
            CreateSnapshot(CoreLoopPhase.Attack),
            new AttackerCommand(scopeHeld: true, firePressed: false),
            ShooterController.ScopeWarmupSeconds,
            Vector3.zero,
            Vector3.forward,
            Vector3.zero);

        ShooterShotResult? result = boundary.ApplyAttackerCommand(
            shooter,
            CreateSnapshot(CoreLoopPhase.Attack),
            new AttackerCommand(scopeHeld: true, firePressed: true),
            0f,
            Vector3.zero,
            Vector3.forward,
            Vector3.zero);

        Assert.IsTrue(result.HasValue);
        Assert.IsTrue(reportedHit);
        Assert.IsTrue(result.Value.HitDodger);
    }

    private static CoreLoopSnapshot CreateSnapshot(CoreLoopPhase phase)
    {
        return new CoreLoopSnapshot(
            phase,
            majorRoundNumber: 1,
            currentAttacker: PlayerSide.PlayerA,
            currentDefender: PlayerSide.PlayerB,
            playerAScore: 0,
            playerBScore: 0,
            bulletsRemaining: 3,
            deploymentSecondsRemaining: 0f,
            secondsSinceLastShot: 0f,
            stalemateWarningActive: false,
            matchOver: false,
            winner: PlayerSide.None,
            matchEndReason: MatchEndReason.None,
            wallResetVersion: 0);
    }

    private sealed class StubWallShotResolver : IWallShotResolver
    {
        private readonly bool hitDodger;

        public StubWallShotResolver(bool hitDodger)
        {
            this.hitDodger = hitDodger;
        }

        public WallShotResolution ResolveShot(Vector3 rayOrigin, Vector3 rayDirection)
        {
            return new WallShotResolution(hitWall: false, hitDodger, rayOrigin, rayDirection, destroyedBrickCount: 0);
        }
    }
}
