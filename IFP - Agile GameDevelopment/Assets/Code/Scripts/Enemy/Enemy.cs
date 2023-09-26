using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float MovementSpeed = 1.0f;

    public float Health = 10.0f;

    private LevelManager levelManager;

    private Transform[] path;

    private int nextTargetIndex;

    public void Initialize(LevelManager levelManager, Transform[] path)
    {
        this.levelManager = levelManager;
        this.path = path;
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

        Vector3 currentPosition = transform.position;

        float distanceToTravel = MovementSpeed * Time.fixedDeltaTime;

        while (distanceToTravel > 0)
        {
            if (nextTargetIndex < path.Length)
            {
                Vector3 targetPosition = path[nextTargetIndex].position;

                Vector3 delta = targetPosition - currentPosition;

                float distance = delta.magnitude;

                if (distance > distanceToTravel)
                {
                    delta *= distanceToTravel / distance;

                    distance = distanceToTravel;
                }
                else
                {
                    ++nextTargetIndex;
                }

                distanceToTravel -= distance;

                currentPosition += delta;
            }
            else
            {
                reachedEnd = true;

                break;
            }
        }

        transform.position = currentPosition;

        return reachedEnd;
    }
}
