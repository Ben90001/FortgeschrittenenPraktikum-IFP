using Codice.CM.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public struct EnemyPosition
{
    private Vector2 position;

    private Vector2[] waypoints;

    private int nextWaypoint;

    public Vector2 Position
    {
        get { return position; }
    }

    public EnemyPosition(Vector2[] waypoints, Vector2 position)
    {
        this.waypoints = waypoints;
        this.position = position;
        this.nextWaypoint = 0;
    }

    public static bool MovePositionTowardsTarget(Vector2 target, ref Vector2 position, ref float distanceToTravel)
    {
        bool reachedTarget = false;

        if (!Mathf.Approximately(distanceToTravel, 0.0f))
        {
            Vector2 delta = target - position;

            float distance = delta.magnitude;

            if (!float.IsNaN(distance) && !float.IsInfinity(distance))
            {
                if (distance > distanceToTravel)
                {
                    Vector2 movementDelta = delta.normalized * distanceToTravel;

                    position = position + movementDelta;
                    distanceToTravel = 0.0f;
                }
                else
                {
                    position = target;
                    distanceToTravel = distanceToTravel - distance;

                    reachedTarget = true;
                }
            }
            else
            {
                // Skip the waypoint if it is unreachable

                reachedTarget = true;
            }
        }
        else
        {
            distanceToTravel = 0.0f;
        }

        return reachedTarget;
    }

    public bool FollowPath(float distanceToTravel)
    {
        bool hasReachedEnd = HasReachedEnd();

        Vector2 currentPosition = position;
        
        Vector2 target = GetCurrentTarget();

        while (!hasReachedEnd && !Mathf.Approximately(distanceToTravel, 0.0f))
        {
            bool hasReachedTarget = MovePositionTowardsTarget(target, ref currentPosition, ref distanceToTravel);

            if (hasReachedTarget)
            {
                TargetNextWaypoint();

                target = GetCurrentTarget();
            }

            hasReachedEnd = HasReachedEnd();
        }

        position = currentPosition;

        return hasReachedEnd;
    }

    public bool HasReachedEnd()
    {
        bool result = true;

        if (waypoints != null && nextWaypoint >= 0 && nextWaypoint < waypoints.Length)
        {
            result = false;
        }

        return result;
    }

    public Vector2 GetCurrentTarget()
    {
        Vector2 result = default;

        if (waypoints != null && waypoints.Length > 0)
        {
            if (nextWaypoint >= 0 && nextWaypoint < waypoints.Length)
            {
                result = waypoints[nextWaypoint];
            }
            else
            {
                result = waypoints[waypoints.Length - 1];
            }
        }

        return result;
    }

    private void TargetNextWaypoint()
    {
        if (nextWaypoint < waypoints.Length)
        {
            ++nextWaypoint;
        }
    }
}
