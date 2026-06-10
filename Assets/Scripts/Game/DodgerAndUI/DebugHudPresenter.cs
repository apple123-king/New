using Game.CoreLoop;

namespace Game.DodgerAndUI
{
    /// <summary>
    /// Builds the minimal state needed by the split-screen debug HUD.
    /// </summary>
    public sealed class DebugHudPresenter
    {
        public RoleHudView BuildHud(CoreLoopSnapshot snapshot, PlayerViewRole viewRole, GamePresentationMode presentationMode)
        {
            bool isDebugMode = presentationMode == GamePresentationMode.Debug;
            bool isAttackerView = viewRole == PlayerViewRole.Attacker;

            return new RoleHudView(
                viewRole,
                presentationMode,
                snapshot.PlayerAScore,
                snapshot.PlayerBScore,
                snapshot.MajorRoundNumber,
                snapshot.CurrentAttacker,
                isAttackerView || isDebugMode ? snapshot.BulletsRemaining : 0,
                snapshot.DeploymentSecondsRemaining,
                showBullets: isAttackerView || isDebugMode,
                showDeploymentMask: snapshot.Phase == CoreLoopPhase.Deployment && (isAttackerView || isDebugMode),
                showScopeStatus: isAttackerView || isDebugMode,
                showShotSummary: isAttackerView || isDebugMode,
                showDodgerState: !isAttackerView || isDebugMode);
        }
    }
}
