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
        bool reachedEnd = followPath();

        if (reachedEnd)
        {
            // TODO: Handle life points etc.

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
