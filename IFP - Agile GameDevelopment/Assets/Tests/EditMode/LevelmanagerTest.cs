using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LevelManagerTests
{

    [Test]
    public void Awake_SetsLevelToNull_ByDefault()
    {
      
        LevelManager levelManager = new GameObject().AddComponent<LevelManager>();

       
        GameObject loadedLevel = levelManager.GetLevel();

        
        Assert.IsNull(loadedLevel);
    }


}
