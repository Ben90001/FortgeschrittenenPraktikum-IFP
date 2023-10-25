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
        Assert.IsNotNull(basicTowerPrefab, "basicTowerPrefab is Null");
        
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
        
        //set up scene for LevelManager (needed for Enemey spawing)
        SceneManager.LoadScene("GameScene");
        yield return new WaitForSeconds(2.5f);
        GameObject LevelManagerObject = GameObject.Find("/LevelManager");
        Assert.IsNotNull(LevelManagerObject, "LevelManagerObject Null");
        //disable loosing
        LevelManagerObject.GetComponent<LevelManager>().PlayerLives = 100000; //not so nice


        //create Tower
        GameObject BasicTowerObject = GameObject.Instantiate<GameObject>(basicTowerPrefab, spawningPosition, Quaternion.identity);
        Assert.IsNotNull(BasicTowerObject, "BasicTowerObject is Null");
        //create Enemy



        yield return null;
    }
}
