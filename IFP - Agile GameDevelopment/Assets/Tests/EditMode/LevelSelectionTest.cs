using NUnit.Framework;
using UnityEngine;

public class LevelSelectionTest
{
    [Test]
    public void GetLevelKey_ReturnsCorrectLevelKeyForAllLevels()
    {
        int numberOfLevels = 2;

        for (int levelID = 1; levelID <= numberOfLevels; levelID++)
        {
            string expectedKey = "Assets/Prefabs/Levels/Level" + levelID.ToString() + ".prefab";

            string result = LevelSelection.GetLevelKey(levelID);

            Assert.AreEqual(expectedKey, result);
        }
    }

    [Test]
    public void LoadLevel_LoadsLevelGameObjectSuccessfully()
    {
        int levelID = 1;

        GameObject loadedLevel = LevelSelection.LoadLevel(levelID);

        Assert.IsNotNull(loadedLevel);
    }
}
