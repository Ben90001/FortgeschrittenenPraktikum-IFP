using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEditor;
using UnityEngine.SceneManagement;

public class BasicTowerTests
{
    private Vector3 spawningPosition;
    private GameObject basicTowerPrefab;
    private GameObject enemyPrefab;


    [SetUp]
    public void BeforeEveryTest()
    {
        spawningPosition.Set(0.0f, 1.0f, 0.0f);

        //get Prefabs
        basicTowerPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Towers/BasicTowerPrefab.prefab");
        enemyPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Enemies/Enemy.prefab");
    }

    [UnityTest]
    public IEnumerator PerformActionFalse_IfNoEnemyInRange()
    {
        //Test SetUp code
        Assert.IsNotNull(basicTowerPrefab);

        //create Tower
        GameObject BasicTowerObject = GameObject.Instantiate<GameObject>(basicTowerPrefab, spawningPosition, Quaternion.identity);
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
        //set up scene for LevelManager
        SceneManager.LoadScene("GameScene");
        GameObject LevelManagerObject = GameObject.Find("LevelManager");
        Assert.IsNotNull(LevelManagerObject, "LevelManagerObject Null");

        //create Tower
        GameObject BasicTowerObject = GameObject.Instantiate<GameObject>(basicTowerPrefab, spawningPosition, Quaternion.identity);
        Assert.IsNotNull(BasicTowerObject, "BasicTowerObject is Null"); //message error message or message if woriking?
        //create Enemy
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
