using UnityEngine;

/// <summary>
/// Holds an enemies local copy of the path that it follows.
/// </summary>
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

    /// <summary>
    /// The remaining distance the enemy has to travel along its path.
    /// </summary>
    public float RemainingPathLength
    {
        get { return remainingPathLength; }
    }

    /// <summary>
    /// Applies a prependicular offset to the path represented by the waypoint array.
    /// This is done in a way to not change the overall path length.
    /// </summary>
    /// <param name="waypoints">The path waypoints.</param>
    /// <param name="perpendicularOffset">Offset to apply. Positive numbers are in travel direction to the right.</param>
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

    /// <summary>
    /// Calculates the total path length.
    /// </summary>
    /// <param name="waypoints">Path representation.</param>
    /// <returns>Total length of path.</returns>
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

    /// <summary>
    /// Sets the next waypoint in this path as its current target waypoint.
    /// </summary>
    /// <returns>The position of the selected waypoint.</returns>
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

    /// <summary>
    /// Gets the current target.
    /// </summary>
    /// <returns>The current target.</returns>
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

    /// <summary>
    /// Returns the starting position.
    /// </summary>
    /// <returns>The starting position.</returns>
    public Vector2 GetStartingPosition()
    {
        return waypoints[0];
    }

    /// <summary>
    /// Returns whether the enemy has reached the end of the path.
    /// </summary>
    /// <returns>True if the enemy has reached the end of its path.</returns>
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
