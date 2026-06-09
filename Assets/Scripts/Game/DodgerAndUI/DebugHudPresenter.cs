using Game.CoreLoop;

namespace Game.DodgerAndUI
{
    /// <summary>
    /// Builds the minimal state needed by the split-screen debug HUD.
    /// </summary>
    public sealed class DebugHudPresenter
    {
        public DebugHudState BuildAttackerHud(CoreLoopSnapshot snapshot)
        {
            return new DebugHudState(
                snapshot.PlayerAScore,
                snapshot.PlayerBScore,
                snapshot.MajorRoundNumber,
                snapshot.CurrentAttacker,
                snapshot.BulletsRemaining,
                snapshot.DeploymentSecondsRemaining,
                showBullets: true,
                showDeploymentMask: snapshot.Phase == CoreLoopPhase.Deployment);
        }

        public DebugHudState BuildDodgerHud(CoreLoopSnapshot snapshot)
        {
            return new DebugHudState(
                snapshot.PlayerAScore,
                snapshot.PlayerBScore,
                snapshot.MajorRoundNumber,
                snapshot.CurrentAttacker,
                bulletsRemaining: 0,
                snapshot.DeploymentSecondsRemaining,
                showBullets: false,
                showDeploymentMask: false);
        }
    }

    public readonly struct DebugHudState
    {
        public DebugHudState(
            int playerAScore,
            int playerBScore,
            int majorRoundNumber,
            PlayerSide currentAttacker,
            int bulletsRemaining,
            float deploymentSecondsRemaining,
            bool showBullets,
            bool showDeploymentMask)
        {
            PlayerAScore = playerAScore;
            PlayerBScore = playerBScore;
            MajorRoundNumber = majorRoundNumber;
            CurrentAttacker = currentAttacker;
            BulletsRemaining = bulletsRemaining;
            DeploymentSecondsRemaining = deploymentSecondsRemaining;
            ShowBullets = showBullets;
            ShowDeploymentMask = showDeploymentMask;
        }

        public int PlayerAScore { get; }

        public int PlayerBScore { get; }

        public int MajorRoundNumber { get; }

        public PlayerSide CurrentAttacker { get; }

        public int BulletsRemaining { get; }

        public float DeploymentSecondsRemaining { get; }

        public bool ShowBullets { get; }

        public bool ShowDeploymentMask { get; }
    }
}
