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
        Assert.IsTrue(noTowerOnTileBefore, "Vor dem Hinzuf�gen des Turms war bereits ein Turm auf dem Tile.");

        levelManager.PlaceTowerAtTile(towerPrefab, tilePosition);

        
        Dictionary<Vector2Int, GameObject> towers = levelManager.GetTowers();
        bool towerAdded = towers.ContainsKey(new Vector2Int(1, 1));

        
        Assert.IsTrue(noTowerOnTileBefore, "Vor dem Hinzuf�gen des Turms war bereits ein Turm auf dem Tile.");
        Assert.IsTrue(towerAdded, "Der Turm wurde nicht zum 'towers'-Dictionary hinzugef�gt.");
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

       
        Assert.AreEqual(1, towerCount, "Es wurde ein Turm zur 'towers'-Dictionary hinzugef�gt, obwohl bereits einer vorhanden war.");
    }

    [Test]
    public void GetTileKeyFromTilePosition_ReturnsCorrectTileKey()
    {
        
        LevelManager levelManager = new GameObject().AddComponent<LevelManager>();

        Vector3Int tilePosition = new Vector3Int(2, 3, 0);


        Vector2Int tileKey = levelManager.GetTileKeyFromTilePosition(tilePosition);

        
        Assert.AreEqual(new Vector2Int(2, 3), tileKey, "GetTileKeyFromTilePosition gibt einen falschen Tile-Key zur�ck.");
    }

    [Test]
    public void TilePositionHasTower_ReturnsFalse_WhenNoTowerIsPresent()
    {
        
        LevelManager levelManager = new GameObject().AddComponent<LevelManager>();
        
        Vector3Int tilePosition = new Vector3Int(1, 1, 0);

        bool hasTower = levelManager.TilePositionHasTower(tilePosition);

        Assert.IsFalse(hasTower, "TilePositionHasTower sollte false zur�ckgeben, wenn kein Turm vorhanden ist.");
    }

    [Test]
    public void TilePositionHasTower_ReturnsTrue_WhenTowerIsPresent()
    {
        LevelManager levelManager = new GameObject().AddComponent<LevelManager>();

        Vector3Int tilePosition = new Vector3Int(1, 1, 0);

        levelManager.GetTowers().Add(new Vector2Int(1, 1), new GameObject("Tower"));

        bool hasTower = levelManager.TilePositionHasTower(tilePosition);

        Assert.IsTrue(hasTower, "TilePositionHasTower sollte true zur�ckgeben, wenn ein Turm vorhanden ist.");
    }
}