using System;
using Game.CoreLoop;
using Game.DodgerAndUI;
using Game.Shooter;
using UnityEngine;

namespace Game.SceneIntegration
{
    /// <summary>
    /// Pure boundary that applies input commands to runtime controllers.
    /// </summary>
    public sealed class SceneCommandBoundary
    {
        public void ApplyDodgerCommand(DodgerController dodger, DodgerCommand command, float deltaSeconds)
        {
            if (dodger == null)
            {
                throw new ArgumentNullException(nameof(dodger));
            }

            Vector3 moveVector = new Vector3(command.MoveXAxis, 0f, command.MoveZAxis);
            if (moveVector.sqrMagnitude > 0f)
            {
                Vector3 delta = moveVector.normalized * 3.2f * deltaSeconds;
                Vector3 next = dodger.Position + delta;
                next.x = Mathf.Clamp(next.x, -4.8f, 4.8f);
                next.z = Mathf.Clamp(next.z, 1.3f, 7.2f);
                dodger.Move(next - dodger.Position);
            }

            if (command.RequestedPose.HasValue)
            {
                dodger.SetPose(command.RequestedPose.Value);
            }
        }

        public ShooterShotResult? ApplyAttackerCommand(
            ShooterController shooter,
            CoreLoopSnapshot snapshot,
            AttackerCommand command,
            float deltaSeconds,
            Vector3 cameraPosition,
            Vector3 cameraForward,
            Vector3 visualRecoilOffset)
        {
            if (shooter == null)
            {
                throw new ArgumentNullException(nameof(shooter));
            }

            if (snapshot.Phase != CoreLoopPhase.Attack)
            {
                shooter.CloseScope();
                return null;
            }

            if (command.ScopeHeld)
            {
                if (!shooter.IsScoped)
                {
                    shooter.OpenScope(cameraPosition, cameraForward);
                }
                else
                {
                    shooter.UpdateAimRay(cameraPosition, cameraForward);
                }

                shooter.Tick(deltaSeconds);
            }
            else if (shooter.IsScoped)
            {
                shooter.CloseScope();
            }

            if (shooter.CanFire && command.FirePressed)
            {
                return shooter.Fire(visualRecoilOffset);
            }

            return null;
        }
    }

    public readonly struct DodgerCommand
    {
        public DodgerCommand(float moveXAxis, float moveZAxis, DodgerPose? requestedPose)
        {
            MoveXAxis = moveXAxis;
            MoveZAxis = moveZAxis;
            RequestedPose = requestedPose;
        }

        public float MoveXAxis { get; }

        public float MoveZAxis { get; }

        public DodgerPose? RequestedPose { get; }
    }

    public readonly struct AttackerCommand
    {
        public AttackerCommand(bool scopeHeld, bool firePressed)
        {
            ScopeHeld = scopeHeld;
            FirePressed = firePressed;
        }

        public bool ScopeHeld { get; }

        public bool FirePressed { get; }
    }
}
