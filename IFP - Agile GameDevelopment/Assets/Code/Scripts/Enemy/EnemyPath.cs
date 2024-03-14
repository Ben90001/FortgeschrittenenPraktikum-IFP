using UnityEngine;

public struct EnemyPath
{
    private Vector2[] waypoints;

    private int targetWaypoint;

    private float pathLength;
    private float remainingPathLength;

    public EnemyPath(Vector2[] path, float perpendicularOffset)
    {
        if (path != null && path.Length > 0)
        {
            waypoints = new Vector2[path.Length];

            path.CopyTo(waypoints, 0);

            ApplyPerpendicularOffsetToPath(waypoints, perpendicularOffset);
        }
        else
        {
            waypoints = new Vector2[1];
        }

        pathLength = CalculateTotalPathLength(waypoints);
        remainingPathLength = pathLength;

        targetWaypoint = 0;
    }

    public float RemainingPathLength
    {
        get { return remainingPathLength; }
    }

    public static void ApplyPerpendicularOffsetToPath(Vector2[] waypoints, float perpendicularOffset)
    {
        if (waypoints != null && waypoints.Length >= 2)
        {
            Vector2 prevOffset = default;

            for (int index = 0; index < waypoints.Length; ++index)
            {
                Vector2 waypoint0 = waypoints[index];

                if (index + 1 < waypoints.Length)
                {
                    Vector2 waypoint1 = waypoints[index + 1];

                    Vector2 normal = (waypoint1 - waypoint0).normalized;
                    Vector2 offset = Vector2.Perpendicular(normal) * perpendicularOffset;

                    if ((index & 1) == 1)
                    {
                        offset = -offset;
                    }

                    waypoint0 += offset + prevOffset;

                    prevOffset = offset;
                }
                else
                {
                    waypoint0 += prevOffset;
                }

                waypoints[index] = waypoint0;
            }
        }
    }

    public static float CalculateTotalPathLength(Vector2[] waypoints)
    {
        float result = 0.0f;

        if (waypoints != null && waypoints.Length > 1)
        {
            Vector2 waypoint0 = waypoints[0];

            for (int index = 0; index < waypoints.Length; ++index)
            {
                Vector2 waypoint1 = waypoints[index];

                Vector2 delta = waypoint1 - waypoint0;

                float distance = delta.magnitude;

                result += distance;

                waypoint0 = waypoint1;
            }
        }

        return result;
    }

    public Vector2 TargetNextWaypoint()
    {
        if (targetWaypoint < waypoints.Length)
        {
            if (targetWaypoint < waypoints.Length - 1)
            {
                Vector2 waypoint0 = waypoints[targetWaypoint];
                Vector2 waypoint1 = waypoints[targetWaypoint + 1];

                float distance = Vector2.Distance(waypoint0, waypoint1);

                remainingPathLength -= distance;
            }

            ++targetWaypoint;
        }

        Vector2 result = GetCurrentTarget();

        return result;
    }

    public Vector2 GetCurrentTarget()
    {
        Vector2 result = default;

        if (targetWaypoint >= 0 && targetWaypoint < waypoints.Length)
        {
            result = waypoints[targetWaypoint];
        }
        else
        {
            result = waypoints[waypoints.Length - 1];
        }

        return result;
    }

    public Vector2 GetStartingPosition()
    {
        return waypoints[0];
    }

    public bool HasReachedEndOfPath()
    {
        bool result = false;

        if (targetWaypoint < 0 || targetWaypoint >= waypoints.Length)
        {
            result = true;
        }

        return result;
    }
}
