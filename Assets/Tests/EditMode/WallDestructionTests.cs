using Game.WallDestruction;
using NUnit.Framework;
using UnityEngine;

public sealed class WallDestructionTests
{
    [Test]
    public void BrickGridWall_CreatesIntactGrid()
    {
        BrickGridWall wall = new BrickGridWall(width: 3, height: 2, brickSize: 1f);

        Assert.AreEqual(3, wall.Width);
        Assert.AreEqual(2, wall.Height);
        Assert.AreEqual(1f, wall.BrickSize);
        Assert.IsFalse(wall.IsDestroyed(0, 0));
        Assert.IsFalse(wall.IsDestroyed(2, 1));
    }

    [Test]
    public void ApplyCircularDamage_DestroysOnlyNewBricksInRadius()
    {
        BrickGridWall wall = new BrickGridWall(width: 3, height: 3, brickSize: 1f);

        int firstChanged = wall.ApplyCircularDamage(new Vector2(0.5f, 0.5f), radius: 0.01f);
        int secondChanged = wall.ApplyCircularDamage(new Vector2(0.5f, 0.5f), radius: 0.01f);

        Assert.AreEqual(1, firstChanged);
        Assert.AreEqual(0, secondChanged);
        Assert.IsTrue(wall.IsDestroyed(0, 0));
        Assert.IsFalse(wall.IsDestroyed(1, 0));
        Assert.IsFalse(wall.IsDestroyed(0, 1));
    }

    [Test]
    public void ApplyCircularDamage_LargerRadiusCanDestroyMultipleBricks()
    {
        BrickGridWall wall = new BrickGridWall(width: 2, height: 2, brickSize: 1f);

        int changed = wall.ApplyCircularDamage(new Vector2(1f, 1f), radius: 0.75f);

        Assert.AreEqual(4, changed);
        Assert.IsTrue(wall.IsDestroyed(0, 0));
        Assert.IsTrue(wall.IsDestroyed(0, 1));
        Assert.IsTrue(wall.IsDestroyed(1, 0));
        Assert.IsTrue(wall.IsDestroyed(1, 1));
    }

    [Test]
    public void ResetForVersion_ClearsWallOnlyWhenVersionChanges()
    {
        BrickGridWall wall = new BrickGridWall(width: 1, height: 1, brickSize: 1f);
        wall.ApplyCircularDamage(new Vector2(0.5f, 0.5f), radius: 0.01f);

        wall.ResetForVersion(0);
        Assert.IsTrue(wall.IsDestroyed(0, 0));

        wall.ResetForVersion(1);
        Assert.IsFalse(wall.IsDestroyed(0, 0));
        Assert.AreEqual(1, wall.ResetVersion);
    }
}
