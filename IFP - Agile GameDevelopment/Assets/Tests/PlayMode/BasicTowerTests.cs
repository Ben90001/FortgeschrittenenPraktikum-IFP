using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;

public class BasicTowerTests
{
    public class PerformActionMethodTests
    {
        private LevelManager levelManager;
        private GameObject basicTowerPrefab;
        private GameObject basicTowerObject;
        private Enemy enemy;// = new GameObject().AddComponent<Enemy>();

        private Vector3 spawningPosition = new Vector3(0.0f,1.0f,0.0f);


        [SetUp]
        public void BeforeEveryTest()
        {
            SceneManager.LoadScene("GameScene");

            levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();

            basicTowerPrefab = levelManager.GetBasicTowerPrefab();
            basicTowerObject = GameObject.Instantiate(basicTowerPrefab, spawningPosition, Quaternion.identity);
            Debug.Log("Position of Tower:" + basicTowerObject.GetComponent<Transform>().position);

        }

        [Test]
        //Asserting FindBestTarget only delivers null or enemy
        public void PerformActionFalseIfEnemieNull()
        {
            //assert no enemy in range (FindBestTarget = Null)
            Assert.IsNull(basicTowerObject.GetComponent<BasicTower>().FindBestTarget(basicTowerObject.GetComponent<BasicTower>().GetActionRadius()));

            //Assert.False(basicTowerObject.GetComponent<BasicTower>().PerformActionForTesting());
        }

        [UnityTest]
        //Asserting FindBestTarget only delivers null or enemy
        public void PerformActionTrueIfEnemieNotNull()
        {
            



            //TODO: not working yet
            Assert.IsNotNull(levelManager);
            //GameObject enemyPrefab = levelManager.GetEnemy();
            //Assert.AreEqual(enemyPrefab, null);
            //GameObject enemy = Instantiate(enemyPrefab, spawningPosition, Quaternion.identity);

            //assert enemy in range
            //Assert.AreNotEqual(null, basicTower.FindBestTarget(basicTower.GetActionRadius()));

            //Assert.False(basicTower.PerformActionForTesting());
        }
    }
}
