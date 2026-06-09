using System;
using Game.WallDestruction;
using UnityEngine;

namespace Game.Shooter
{
    /// <summary>
    /// Pure shooter state for scope, aim, and authoritative fire.
    /// </summary>
    public sealed class ShooterController
    {
        public const float ScopeWarmupSeconds = 0.25f;

        private readonly Action<bool> reportShotToCoreLoop;
        private readonly IWallShotResolver wallShotResolver;

        private Vector3 aimOrigin;
        private Vector3 aimDirection = Vector3.forward;
        private float scopedSeconds;

        public ShooterController(Action<bool> reportShotToCoreLoop, IWallShotResolver wallShotResolver)
        {
            this.reportShotToCoreLoop = reportShotToCoreLoop ?? throw new ArgumentNullException(nameof(reportShotToCoreLoop));
            this.wallShotResolver = wallShotResolver ?? throw new ArgumentNullException(nameof(wallShotResolver));
        }

        public bool IsScoped { get; private set; }

        public bool CanFire => IsScoped && scopedSeconds >= ScopeWarmupSeconds;

        public Vector3 AimOrigin => aimOrigin;

        public Vector3 AimDirection => aimDirection;

        public void OpenScope(Vector3 origin, Vector3 direction)
        {
            IsScoped = true;
            scopedSeconds = 0f;
            UpdateAimRay(origin, direction);
        }

        public void CloseScope()
        {
            IsScoped = false;
            scopedSeconds = 0f;
        }

        public void UpdateAimRay(Vector3 origin, Vector3 direction)
        {
            if (direction.sqrMagnitude <= 0f)
            {
                throw new ArgumentException("Aim direction cannot be zero.", nameof(direction));
            }

            aimOrigin = origin;
            aimDirection = direction.normalized;
        }

        public void Tick(float deltaSeconds)
        {
            if (deltaSeconds < 0f)
            {
                throw new ArgumentOutOfRangeException(nameof(deltaSeconds));
            }

            if (IsScoped)
            {
                scopedSeconds += deltaSeconds;
            }
        }

        public ShooterShotResult Fire(Vector3 visualRecoilOffset)
        {
            if (!CanFire)
            {
                throw new InvalidOperationException("Scope warmup is not complete.");
            }

            Vector3 authoritativeDirection = aimDirection;
            WallShotResolution wallResult = wallShotResolver.ResolveShot(aimOrigin, authoritativeDirection);
            reportShotToCoreLoop(wallResult.HitDodger);

            return new ShooterShotResult(
                aimOrigin,
                authoritativeDirection,
                visualRecoilOffset,
                wallResult.HitWall,
                wallResult.HitDodger,
                wallResult.DestroyedBrickCount);
        }
    }
}
