using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [HideInInspector]
    public Transform[] Path;
    
    public float MovementSpeed = 2.0f;


    public float Health = 10.0f;

    private int nextTargetIndex;


    public void FixedUpdate()
    {
        bool reachedEnd = FollowPath();

        if (reachedEnd)
        {
            //LevelManager.DecreasePlayerLives();
            //fix by instanciating enemies with LevelManager Reference

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

    private bool FollowPath()
    {
        bool reachedEnd = false;

        Vector3 currentPosition = transform.position;

        float distanceToTravel = MovementSpeed * Time.fixedDeltaTime;

        while (distanceToTravel > 0)
        {
            if (nextTargetIndex < Path.Length)
            {
                Vector3 targetPosition = Path[nextTargetIndex].position;

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
