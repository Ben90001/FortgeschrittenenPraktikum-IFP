using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class LevelManagerTests
{
    [Test]
    public void Awake_SetsLevelToNull_ByDefault()
    {
        LevelManager levelManager = new GameObject().AddComponent<LevelManager>();

        GameObject loadedLevel = levelManager.Test_Level;

        Assert.IsNull(loadedLevel);
    }

    [Test]
    public void PlaceTowerAtTile_AddsTowerToTowersDictionary_WhenTilePositionHasNoTower()
    {
        LevelManager levelManager = new GameObject().AddComponent<LevelManager>();
        GameObject towerPrefab = new GameObject("TowerPrefab");

        Vector3Int tilePosition = new Vector3Int(1, 1, 0);

        bool noTowerOnTileBefore = !levelManager.Test_TilePositionHasTower(tilePosition);
        Assert.IsTrue(noTowerOnTileBefore, "There already was a tower at this tile.");

        levelManager.PlaceTowerAtTile(towerPrefab, tilePosition);
        
        Dictionary<Vector2Int, GameObject> towers = levelManager.Test_Towers;
        bool towerAdded = towers.ContainsKey(new Vector2Int(1, 1));

        Assert.IsTrue(noTowerOnTileBefore, "There already was a tower at this tile.");
        Assert.IsTrue(towerAdded, "The tower was not added to the towers dictionary.");
    }

    [Test]
    public void DecreasePlayerLives_UpdatesPlayerLivesCorrectly()
    {
        LevelManager levelManager = new GameObject().AddComponent<LevelManager>();
     
        levelManager.Test_PlayerLives = 5;

        levelManager.DecreasePlayerLives();

        Assert.AreEqual(4, levelManager.Test_PlayerLives, "The player lives were not correctly diminished");
    }

    [Test]
    public void PlaceTowerAtTile_DoesNotAddTowerToTowersDictionary_WhenTilePositionHasTower()
    {
        LevelManager levelManager = new GameObject().AddComponent<LevelManager>();

        GameObject towerPrefab = new GameObject("TowerPrefab");
       
        Vector3Int tilePosition = new Vector3Int(1, 1, 0);
      
        levelManager.Test_Towers.Add(new Vector2Int(1, 1), new GameObject("Tower"));

        levelManager.PlaceTowerAtTile(towerPrefab, tilePosition);
        
        Dictionary<Vector2Int, GameObject> towersAfter = levelManager.Test_Towers;
        int towerCount = towersAfter.Count;

        Assert.AreEqual(1, towerCount, "Added tower to dictionary even though there already was a tower at this tile position.");
    }

    [Test]
    public void GetTileKeyFromTilePosition_ReturnsCorrectTileKey()
    {
        LevelManager levelManager = new GameObject().AddComponent<LevelManager>();

        Vector3Int tilePosition = new Vector3Int(2, 3, 0);

        Vector2Int tileKey = LevelManager.GetTileKeyFromTilePosition(tilePosition);
        
        Assert.AreEqual(new Vector2Int(2, 3), tileKey, "GetTileKeyFromTilePosition returned an incorrect tile key.");
    }

    [Test]
    public void TilePositionHasTower_ReturnsFalse_WhenNoTowerIsPresent()
    {
        LevelManager levelManager = new GameObject().AddComponent<LevelManager>();
        
        Vector3Int tilePosition = new Vector3Int(1, 1, 0);

        bool hasTower = levelManager.Test_TilePositionHasTower(tilePosition);

        Assert.IsFalse(hasTower, "TilePositionHasTower should return false if there is no tower at tile position.");
    }

    [Test]
    public void TilePositionHasTower_ReturnsTrue_WhenTowerIsPresent()
    {
        LevelManager levelManager = new GameObject().AddComponent<LevelManager>();

        Vector3Int tilePosition = new Vector3Int(1, 1, 0);

        levelManager.Test_Towers.Add(new Vector2Int(1, 1), new GameObject("Tower"));

        bool hasTower = levelManager.Test_TilePositionHasTower(tilePosition);

        Assert.IsTrue(hasTower, "TilePositionHasTower should return true if a tower exists at tile position.");
    }

    [Test]
    public void IncreaseCurrency_AddsToCurrency()
    {
        GameObject gameObject = new GameObject();
        LevelManager levelManager = gameObject.AddComponent<LevelManager>();

        levelManager.IncreaseCurrency(50);
        Assert.AreEqual(150, levelManager.Test_Currency);
    }

    [Test]
    public void SpendCurrency_DeductsFromCurrency()
    {
        GameObject gameObject = new GameObject();
        LevelManager levelManager = gameObject.AddComponent<LevelManager>();
        levelManager.SpendCurrency(30);
        Assert.AreEqual(70, levelManager.Test_Currency);
    }

    [Test]
    public void SpendCurrency_ReturnsTrueIfEnoughCurrency()
    {
        GameObject gameObject = new GameObject();
        LevelManager levelManager = gameObject.AddComponent<LevelManager>();

        bool result = levelManager.SpendCurrency(30);
        Assert.IsTrue(result);
    }

    [Test]
    public void SpendCurrency_ReturnsFalseIfNotEnoughCurrency()
    {
        GameObject gameObject = new GameObject();
        LevelManager levelManager = gameObject.AddComponent<LevelManager>();
        bool result = levelManager.SpendCurrency(200);
        Assert.IsFalse(result);
    }
}