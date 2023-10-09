using Codice.CM.Client.Differences;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Properties;
using UnityEditor;
using UnityEngine;

public class Enemy : MonoBehaviour
{ 
    public float MovementSpeed = 10.0f;
    public float Health;

    private LevelManager levelManager;

    private EnemyPosition position;

    private bool isSlowed = false;

    private float slowFactor = 1.0f;
    private float originalMovementSpeed;

    public void Initialize(LevelManager levelManager, Transform[] path)
    {
        this.levelManager = levelManager;
        
        // TODO: Clean this code up

        Vector2[] waypoints = new Vector2[path.Length];

        for (int index = 0; index < path.Length; ++index)
        {
            waypoints[index] = path[index].position;
        }

        position = new EnemyPosition(waypoints, waypoints[0]);

        transform.position = waypoints[0];

        // pathOffset = UnityEngine.Random.Range(-0.3f, 0.3f);
        // nextTargetPosition = GetNextTargetPosition(this.path, nextTargetIndex, pathOffset);
        // ++nextTargetIndex;
        // transform.position = this.path[0] + Vector2.Perpendicular((this.path[1] - this.path[0]).normalized) * pathOffset;
    }

    public void FixedUpdate()
    {
        bool reachedEnd = FollowPathAndUpdateTransform();

        if (reachedEnd)
        {
            levelManager.DecreasePlayerLives();

            Destroy(gameObject);
        }
    }

    public void ApplyDamage(float amount)
    {
        Health -= amount;

        if (Health <= 0.0f)
        {
            // TODO: Handle destroyed enemy
                // TODO: Handel Currency for Kill.

            Destroy(gameObject);
        }
    }

    public void ApplySlow(float factor)
    {
        if (!isSlowed)
        {
            isSlowed = true;
            slowFactor = factor;
            originalMovementSpeed = MovementSpeed;
            MovementSpeed *= slowFactor;

            Debug.Log("Enemy slowed down!");
        }
    }

    public void RemoveSlow()
    {
        if (isSlowed)
        {
            isSlowed = false;
            MovementSpeed = originalMovementSpeed;

            Debug.Log("Slow removed from Enemy!");
        }
    }

    public void RestoreSpeed()
    {
        MovementSpeed = 10f;
    }

    private bool FollowPathAndUpdateTransform()
    {
        bool reachedEnd = position.FollowPath(Time.fixedDeltaTime * MovementSpeed);

        transform.position = position.Position;

        return reachedEnd;
    }

    /* 
    private static int GetSignFromLastBit(int value)
    {
        int result = ((value << 31) >> 31) + (~value & 1);

        return result;
    }

    private static Vector2 GetNextTargetPosition(Vector2[] path, int lastTargetIndex, float pathOffset)
    {
        Vector2 result = default(Vector2);

        Vector2 waypoint0 = path[lastTargetIndex];

        float directedPathOffset = GetSignFromLastBit(lastTargetIndex) * pathOffset;

        if (lastTargetIndex + 1 < path.Length)
        {
            Vector2 waypoint1 = path[lastTargetIndex + 1];
            Vector2 oldOffset = Vector2.Perpendicular((waypoint1 - waypoint0).normalized);

            result = waypoint1 + directedPathOffset * oldOffset;

            if (lastTargetIndex + 2 < path.Length)
            {
                Vector2 waypoint2 = path[lastTargetIndex + 2];
                Vector2 newOffset = Vector2.Perpendicular((waypoint2 - waypoint1).normalized);

                result -= directedPathOffset * newOffset;
            }
        }

        return result;
    }
    */
}
