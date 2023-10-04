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






}
