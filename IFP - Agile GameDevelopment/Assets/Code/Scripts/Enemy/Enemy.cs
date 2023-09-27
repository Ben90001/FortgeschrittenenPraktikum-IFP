using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Properties;
using UnityEditor;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float MovementSpeed = 1.0f;

    public float Health = 10.0f;

    private LevelManager levelManager;

    private Vector2[] path;

    private Vector2 nextTargetPosition;

    private float pathOffset;

    private int nextTargetIndex;

    public void Initialize(LevelManager levelManager, Transform[] path)
    {
        this.levelManager = levelManager;
        this.path = new Vector2[path.Length];

        for (int index = 0; index < path.Length; ++index)
        {
            this.path[index] = path[index].position;
        }

        pathOffset = UnityEngine.Random.Range(-0.3f, 0.3f);
        nextTargetPosition = GetNextTargetPosition(this.path, nextTargetIndex, pathOffset);

        ++nextTargetIndex;

        transform.position = this.path[0] + Vector2.Perpendicular((this.path[1] - this.path[0]).normalized) * pathOffset;
    }

    public void FixedUpdate()
    {
        bool reachedEnd = followPath();

        if (reachedEnd)
        {
            // TODO: Handle life points etc.

            Destroy(gameObject);
        }
    }

    public void ApplyDamage(float amount)
    {
        Health -= amount;

        if (Health <= 0.0f)
        {
            // TODO: Handle destroyed enemy

            Destroy(gameObject);
        }
    }

    private bool followPath()
    {
        bool reachedEnd = false;

        Vector2 currentPosition = transform.position;

        float distanceToTravel = MovementSpeed * Time.fixedDeltaTime;

        for (int iteration = 0; iteration < 3; ++iteration)
        {
            if (distanceToTravel >= 0.0f)
            {
                Vector2 delta = nextTargetPosition - currentPosition;

                float distance = delta.magnitude;

                if (distance > distanceToTravel)
                {
                    delta *= distanceToTravel / distance;

                    distance = distanceToTravel;
                }
                else
                {
                    if (nextTargetIndex + 1 < path.Length)
                    {
                        nextTargetPosition = GetNextTargetPosition(path, nextTargetIndex, pathOffset);

                        ++nextTargetIndex;
                    }
                    else
                    {
                        reachedEnd = true;
                    }
                }

                distanceToTravel -= distance;

                currentPosition += delta;
            }
            else
            {
                break;
            }
        }

        transform.position = currentPosition;

        return reachedEnd;
    }

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
}
