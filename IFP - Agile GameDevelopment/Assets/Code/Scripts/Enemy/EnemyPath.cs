using System.IO;
using Unity.VisualScripting.YamlDotNet.Core.Tokens;
using UnityEngine;

public struct EnemyPath
{
    private Vector2[] waypoints;

    private int targetWaypoint;

    public EnemyPath(Vector2[] path, float perpendicularOffset)
    {
        if (path != null && path.Length > 0)
        {
            this.waypoints = new Vector2[path.Length];

            path.CopyTo(this.waypoints, 0);

            ApplyPerpendicularOffsetToPath(this.waypoints, perpendicularOffset);
        }
        else
        {
            this.waypoints = new Vector2[1];
        }

        this.targetWaypoint = 0;
    }

    public Vector2 TargetNextWaypoint()
    {
        if (targetWaypoint < waypoints.Length)
        {
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
            }
        }
    }
}
