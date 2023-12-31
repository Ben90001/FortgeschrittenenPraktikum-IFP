using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class EnemyTests
{
    [UnityTest]
    public IEnumerator Enemy_Test()
    {
        SceneManager.LoadScene("GameScene");

        return null;
    }

    /*
    [Test]
    public void Enemy_FollowPath()
    {
        LevelManager levelManger = new LevelManager();

        GameObject enemyObject = new GameObject();
        Enemy enemy = enemyObject.AddComponent<Enemy>();

        Vector2 waypoint1 = new Vector2(0f, 0f, 0f);
        Vector2 waypoint2 = new Vector2(1f, 0f, 0f);

        enemy.Initialize(levelManger, new Vector2[] { waypoint1, waypoint2 });

        int maxIterations = 10000;
        int iterations = 0;

        while (iterations < maxIterations)
        {
            enemy.FixedUpdate();
            iterations++;

            // Check if the enemy has reached the waypoint and exit the loop
            if (enemyObject.transform.position == waypoint2.position)
            {
                break;
            }
        }

        // Check if the enemy has moved to the second waypoint
        Assert.AreEqual(waypoint2.position, enemyObject.transform.position);
    }
    [UnityTest]
    public IEnumerator Enemy_DestroyedAfterTenHits()
    {
       
        GameObject enemyObject = new GameObject();
        Enemy enemy = enemyObject.AddComponent<Enemy>();
        enemy.Health = 10.0f; 

       
        GameObject bulletObject = new GameObject();
        Bullet bullet = bulletObject.AddComponent<Bullet>();
        bullet.Target = enemy;

        for (int i = 0; i < 10; i++)
        {
            
            bullet.FixedUpdate();
        }

        
        yield return null;

        
        Assert.IsTrue(enemyObject == null, "Der Feind wurde nicht nach 10 Treffern zerst�rt.");
    }

    [UnityTest]
    public IEnumerator Enemy_DestroyedWhenReachedEnd()
    {
        LevelManager levelManger = new LevelManager();

        GameObject enemyObject = new GameObject();
        Enemy enemy = enemyObject.AddComponent<Enemy>();

        Transform waypoint1 = new GameObject("Waypoint1").transform;

        enemy.Initialize(levelManger, new Transform[] { waypoint1 });

        enemy.FixedUpdate();

        // Wait a short amount of time to make sure the destruction is processed
        yield return new WaitForSeconds(0.1f);

        // Check if the enemy has been destroyed
        Assert.IsTrue(enemyObject == null, "Der Feind wurde nicht zerst�rt.");
    }
    */
}