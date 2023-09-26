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

    private Vector3 lastTargetPosition;
    private Vector3 nextTargetPosition;

    public void Initialize(LevelManager levelManager, Transform[] path)
    {
        this.levelManager = levelManager;
        this.path = path;

        this.transform.position = path[0].position;
        this.nextTargetPosition = this.transform.position;
        this.lastTargetPosition = this.nextTargetPosition;
        this.nextTargetIndex = 0;
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
            Vector3 delta = nextTargetPosition - currentPosition;

            float distance = delta.magnitude;

            if (distance > distanceToTravel)
            {
                delta *= distanceToTravel / distance;

                distance = distanceToTravel;
            }
            else
            {
                ++nextTargetIndex;

                lastTargetPosition = nextTargetPosition;
                nextTargetPosition = path[nextTargetIndex].position;

                Vector3 targetDelta = nextTargetPosition - lastTargetPosition;

                Vector3 targetShift = targetDelta.normalized * Random.Range(-0.2f, 0.2f);

                nextTargetPosition += targetShift;
            }

            distanceToTravel -= distance;

            currentPosition += delta;
        }

        transform.position = currentPosition;

        return reachedEnd;
    }
}
