using UnityEngine;

namespace Game.Shooter
{
    /// <summary>
    /// Public result for one shooter fire command.
    /// </summary>
    public readonly struct ShooterShotResult
    {
        public ShooterShotResult(
            Vector3 rayOrigin,
            Vector3 authoritativeDirection,
            Vector3 visualRecoilOffset,
            bool hitWall,
            bool hitDodger,
            int destroyedBrickCount)
        {
            RayOrigin = rayOrigin;
            AuthoritativeDirection = authoritativeDirection.normalized;
            VisualRecoilOffset = visualRecoilOffset;
            HitWall = hitWall;
            HitDodger = hitDodger;
            DestroyedBrickCount = destroyedBrickCount;
        }

        public Vector3 RayOrigin { get; }

        public Vector3 AuthoritativeDirection { get; }

        public Vector3 VisualRecoilOffset { get; }

        public bool HitWall { get; }

        public bool HitDodger { get; }

        public int DestroyedBrickCount { get; }
    }
}
