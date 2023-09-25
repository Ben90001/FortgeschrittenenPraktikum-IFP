using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;


public class EnemyTests
{
    [Test]
    public void Enemy_FollowPath()
    {
        GameObject enemyObject = new GameObject();
        Enemy enemy = enemyObject.AddComponent<Enemy>();

        Transform waypoint1 = new GameObject("Waypoint1").transform;
        waypoint1.position = new Vector3(0f, 0f, 0f);
        Transform waypoint2 = new GameObject("Waypoint2").transform;
        waypoint2.position = new Vector3(1f, 0f, 0f);

        enemy.Path = new Transform[] { waypoint1, waypoint2 };

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
    public IEnumerator Enemy_DestroyedWhenReachedEnd()
    {
        GameObject enemyObject = new GameObject();
        Enemy enemy = enemyObject.AddComponent<Enemy>();

        Transform waypoint1 = new GameObject("Waypoint1").transform;
        enemy.Path = new Transform[] { waypoint1 };

        enemy.FixedUpdate();

        // Wait a short amount of time to make sure the destruction is processed
        yield return new WaitForSeconds(0.1f);

        // Check if the enemy has been destroyed
        Assert.IsTrue(enemyObject == null, "Der Feind wurde nicht zerstört.");
    }



}