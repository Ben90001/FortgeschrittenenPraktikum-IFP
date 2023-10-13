using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEditor;

public class BasicTowerTests
{
    private Vector3 spawningPosition;
    private GameObject BasicTowerPrefab;
    private GameObject EnemyPrefab;


    [SetUp]
    public void BeforeEveryTest()
    {
        spawningPosition.Set(0.0f, 1.0f, 0.0f);

        //get Prefabs
        BasicTowerPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Towers/BasicTowerPrefab.prefab");
        EnemyPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Enemies/Enemy.prefab");
    }

    [UnityTest]
    public IEnumerator PerformActionFalse_IfNoEnemyInRange()
    {
        //Test SetUp code
        Assert.IsNotNull(BasicTowerPrefab);

        //create Tower
        GameObject BasicTowerObject = GameObject.Instantiate<GameObject>(BasicTowerPrefab, spawningPosition, Quaternion.identity);
        Assert.IsNotNull(BasicTowerObject);
        //Assert no Enemy in Range
        Assert.IsNull(BasicTowerObject.GetComponent<BasicTower>().FindBestTargetForTests());
        //Actual Test
        Assert.False(BasicTowerObject.GetComponent<BasicTower>().PerformActionForTests());
        yield return null;
    }

    [UnityTest]
    public IEnumerator PerformActionTrue_IfEnemyInRange()
    {
        //create Tower
        GameObject BasicTowerObject = GameObject.Instantiate<GameObject>(BasicTowerPrefab, spawningPosition, Quaternion.identity);
        Assert.IsNotNull(BasicTowerObject, "BasicTowerObject is Null");
        //create Enemy
        GameObject levelManagerObject = new GameObject();
        levelManagerObject.AddComponent<LevelManager>();

        
        Vector2 pathStart = new Vector2(spawningPosition.x, spawningPosition.y);
        Vector2 pathEnd = new Vector2(10.0f, 10.0f);
        Vector2[] path = { pathStart, pathEnd };

        /*
        GameObject EnemyObject = GameObject.Instantiate<GameObject>(EnemyPrefab, spawningPosition, Quaternion.identity);
        //Assert.IsNotNull(EnemyObject);
        EnemyObject.GetComponent<Enemy>().Initialize(levelManagerObject.GetComponent<LevelManager>(), path);
        EnemyObject.GetComponent<Enemy>().SetMovementSpeed(0.0f);
        */

        yield return null;
    }
}
