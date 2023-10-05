using NUnit.Framework;
using System.Collections.Generic;
using System.Reflection;
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

    [Test]


 
    public void PlaceTowerAtTile_AddsTowerToTowersDictionary_WhenTilePositionHasNoTower()
    {
        
        LevelManager levelManager = new GameObject().AddComponent<LevelManager>();

        
        GameObject towerPrefab = new GameObject("TowerPrefab");

       
        Vector3Int tilePosition = new Vector3Int(1, 1, 0);

        
        bool noTowerOnTileBefore = !levelManager.TilePositionHasTower(tilePosition);
        Assert.IsTrue(noTowerOnTileBefore, "Vor dem Hinzufügen des Turms war bereits ein Turm auf dem Tile.");

        levelManager.PlaceTowerAtTile(towerPrefab, tilePosition);

        
        Dictionary<Vector2Int, GameObject> towers = levelManager.GetTowers();
        bool towerAdded = towers.ContainsKey(new Vector2Int(1, 1));

        
        Assert.IsTrue(noTowerOnTileBefore, "Vor dem Hinzufügen des Turms war bereits ein Turm auf dem Tile.");
        Assert.IsTrue(towerAdded, "Der Turm wurde nicht zum 'towers'-Dictionary hinzugefügt.");
    }

    [Test]
    public void DecreasePlayerLives_UpdatesPlayerLivesCorrectly()
    {
        
        LevelManager levelManager = new GameObject().AddComponent<LevelManager>();

     
        levelManager.PlayerLives = 5;

     
        levelManager.DecreasePlayerLives();

        Assert.AreEqual(4, levelManager.PlayerLives, "Die Spielerleben wurden nicht korrekt verringert.");
    }

    [Test]
    public void PlaceTowerAtTile_DoesNotAddTowerToTowersDictionary_WhenTilePositionHasTower()
    {
        LevelManager levelManager = new GameObject().AddComponent<LevelManager>();

     
        GameObject towerPrefab = new GameObject("TowerPrefab");

       
        Vector3Int tilePosition = new Vector3Int(1, 1, 0);

      
        levelManager.GetTowers().Add(new Vector2Int(1, 1), new GameObject("Tower"));

        levelManager.PlaceTowerAtTile(towerPrefab, tilePosition);

        
        Dictionary<Vector2Int, GameObject> towersAfter = levelManager.GetTowers();
        int towerCount = towersAfter.Count;

       
        Assert.AreEqual(1, towerCount, "Es wurde ein Turm zur 'towers'-Dictionary hinzugefügt, obwohl bereits einer vorhanden war.");
    }

    
}