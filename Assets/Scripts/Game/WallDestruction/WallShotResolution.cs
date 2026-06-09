using UnityEngine;

namespace Game.WallDestruction
{
    /// <summary>
    /// Result returned after resolving one authoritative shot ray.
    /// </summary>
    public readonly struct WallShotResolution
    {
        public WallShotResolution(bool hitWall, bool hitDodger, Vector3 rayOrigin, Vector3 rayDirection, int destroyedBrickCount)
        {
            HitWall = hitWall;
            HitDodger = hitDodger;
            RayOrigin = rayOrigin;
            RayDirection = rayDirection.normalized;
            DestroyedBrickCount = destroyedBrickCount;
        }

        public bool HitWall { get; }

        public bool HitDodger { get; }

        public Vector3 RayOrigin { get; }

        public Vector3 RayDirection { get; }

        public int DestroyedBrickCount { get; }
    }

    /// <summary>
    /// Contract used by shooter logic to resolve wall and dodger hits.
    /// </summary>
    public interface IWallShotResolver
    {
        WallShotResolution ResolveShot(Vector3 rayOrigin, Vector3 rayDirection);
    }
}
