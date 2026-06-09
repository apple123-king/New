using UnityEngine;

namespace Game.DodgerAndUI
{
    /// <summary>
    /// Pure dodger state for movement, pose, and deployment lock.
    /// </summary>
    public sealed class DodgerController
    {
        public DodgerController(Vector3 startPosition)
        {
            Position = startPosition;
            Pose = DodgerPose.Standing;
            LockedDeploymentPosition = startPosition;
            LockedDeploymentPose = DodgerPose.Standing;
        }

        public Vector3 Position { get; private set; }

        public DodgerPose Pose { get; private set; }

        public Vector3 LockedDeploymentPosition { get; private set; }

        public DodgerPose LockedDeploymentPose { get; private set; }

        public bool HasDeploymentLock { get; private set; }

        public void Move(Vector3 delta)
        {
            Position += delta;
        }

        public void SetPose(DodgerPose pose)
        {
            Pose = pose;
        }

        public void LockDeploymentStart()
        {
            LockedDeploymentPosition = Position;
            LockedDeploymentPose = Pose;
            HasDeploymentLock = true;
        }

        public Vector3 GetHitboxSize()
        {
            switch (Pose)
            {
                case DodgerPose.Crouching:
                    return new Vector3(0.6f, 1.1f, 0.6f);
                case DodgerPose.Prone:
                    return new Vector3(0.6f, 0.45f, 1.4f);
                default:
                    return new Vector3(0.6f, 1.8f, 0.6f);
            }
        }
    }
}
