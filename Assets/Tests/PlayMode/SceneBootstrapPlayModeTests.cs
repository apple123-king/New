using System.Collections;
using Game;
using Game.SceneIntegration;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public sealed class SceneBootstrapPlayModeTests
{
    [UnityTest]
    public IEnumerator RuntimeBootstrap_CreatesSplitSceneObjectsInSecureMode()
    {
        yield return null;

        GameSliceSceneBootstrap bootstrap = Object.FindObjectOfType<GameSliceSceneBootstrap>();
        Assert.IsNotNull(bootstrap);
        Assert.AreEqual(GamePresentationMode.Secure, bootstrap.PresentationMode);

        Camera attackCamera = GameObject.Find("AttackCamera")?.GetComponent<Camera>();
        Camera dodgerCamera = GameObject.Find("DodgerCamera")?.GetComponent<Camera>();
        GameObject greybox = GameObject.Find("GameSlice_Greybox");

        Assert.IsNotNull(attackCamera);
        Assert.IsNotNull(dodgerCamera);
        Assert.IsNotNull(greybox);
    }
}
