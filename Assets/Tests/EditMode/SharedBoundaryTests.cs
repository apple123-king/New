using Game;
using NUnit.Framework;

public sealed class SharedBoundaryTests
{
    [Test]
    public void RoleHudView_RetainsVisibilityFlagsAndRole()
    {
        RoleHudView view = new RoleHudView(
            PlayerViewRole.Dodger,
            GamePresentationMode.Debug,
            playerAScore: 1,
            playerBScore: 2,
            majorRoundNumber: 3,
            currentAttacker: PlayerSide.PlayerB,
            bulletsRemaining: 2,
            deploymentSecondsRemaining: 1.5f,
            showBullets: true,
            showDeploymentMask: true,
            showScopeStatus: true,
            showShotSummary: true,
            showDodgerState: true);

        Assert.AreEqual(PlayerViewRole.Dodger, view.ViewRole);
        Assert.AreEqual(GamePresentationMode.Debug, view.PresentationMode);
        Assert.IsTrue(view.ShowBullets);
        Assert.IsTrue(view.ShowShotSummary);
        Assert.AreEqual(2, view.BulletsRemaining);
    }
}
