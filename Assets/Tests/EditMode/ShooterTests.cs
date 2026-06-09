using Game.Shooter;
using Game.WallDestruction;
using NUnit.Framework;
using UnityEngine;

public sealed class ShooterTests
{
    [Test]
    public void Fire_BeforeScopeWarmup_ThrowsAndDoesNotReportShot()
    {
        int reportedShots = 0;
        ShooterController shooter = new ShooterController(_ => reportedShots++, new StubWallShotResolver(hitDodger: true));

        shooter.OpenScope(Vector3.zero, Vector3.forward);
        shooter.Tick(ShooterController.ScopeWarmupSeconds - 0.01f);

        Assert.IsFalse(shooter.CanFire);
        Assert.Throws<System.InvalidOperationException>(() => shooter.Fire(Vector3.right));
        Assert.AreEqual(0, reportedShots);
    }

    [Test]
    public void OpenScope_NormalizesAuthoritativeAimRayImmediately()
    {
        ShooterController shooter = new ShooterController(_ => { }, new StubWallShotResolver(hitDodger: false));
        Vector3 origin = new Vector3(1f, 2f, 3f);

        shooter.OpenScope(origin, new Vector3(0f, 0f, 5f));

        Assert.IsTrue(shooter.IsScoped);
        Assert.AreEqual(origin, shooter.AimOrigin);
        Assert.That(Vector3.Distance(Vector3.forward, shooter.AimDirection), Is.LessThan(0.0001f));
    }

    [Test]
    public void Fire_UsesAuthoritativeRayAndReportsDodgerHit()
    {
        bool? reportedHit = null;
        StubWallShotResolver resolver = new StubWallShotResolver(hitDodger: true, destroyedBrickCount: 4);
        ShooterController shooter = new ShooterController(hit => reportedHit = hit, resolver);
        Vector3 origin = new Vector3(2f, 1f, -3f);
        Vector3 aimDirection = new Vector3(2f, 0f, 0f);
        Vector3 recoilOffset = new Vector3(-4f, 9f, 2f);

        shooter.OpenScope(origin, aimDirection);
        shooter.Tick(ShooterController.ScopeWarmupSeconds);
        ShooterShotResult result = shooter.Fire(recoilOffset);

        Assert.AreEqual(origin, resolver.LastOrigin);
        Assert.That(Vector3.Distance(Vector3.right, resolver.LastDirection), Is.LessThan(0.0001f));
        Assert.That(Vector3.Distance(Vector3.right, result.AuthoritativeDirection), Is.LessThan(0.0001f));
        Assert.AreEqual(recoilOffset, result.VisualRecoilOffset);
        Assert.IsTrue(result.HitWall);
        Assert.IsTrue(result.HitDodger);
        Assert.AreEqual(4, result.DestroyedBrickCount);
        Assert.AreEqual(true, reportedHit);
    }

    private sealed class StubWallShotResolver : IWallShotResolver
    {
        private readonly bool hitDodger;
        private readonly int destroyedBrickCount;

        public StubWallShotResolver(bool hitDodger, int destroyedBrickCount = 0)
        {
            this.hitDodger = hitDodger;
            this.destroyedBrickCount = destroyedBrickCount;
        }

        public Vector3 LastOrigin { get; private set; }

        public Vector3 LastDirection { get; private set; }

        public WallShotResolution ResolveShot(Vector3 rayOrigin, Vector3 rayDirection)
        {
            LastOrigin = rayOrigin;
            LastDirection = rayDirection;
            return new WallShotResolution(
                hitWall: true,
                hitDodger,
                rayOrigin,
                rayDirection,
                destroyedBrickCount);
        }
    }
}
