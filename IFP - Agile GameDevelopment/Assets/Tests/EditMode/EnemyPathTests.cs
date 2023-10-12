using NUnit.Framework;
using System.IO;
using UnityEngine;

public class EnemyPathTests
{
    [Test]
    public void ApplyPerpendicularOffsetToPath_AppliesCorrectOffsetForTwoWaypoints()
    {
        Vector2 waypoint0 = new Vector2(0, 0);
        Vector2 waypoint1 = new Vector2(0, 1);

        float perpendicularOffset = 1.0f;

        Vector2[] path = new Vector2[] { waypoint0, waypoint1 };

        EnemyPath.ApplyPerpendicularOffsetToPath(path, perpendicularOffset);

        Vector2 offsetVector = new Vector2(-1.0f, 0.0f);

        Vector2 offsetWaypoint0 = waypoint0 + offsetVector;
        Vector2 offsetWaypoint1 = waypoint1 + offsetVector;

        Assert.IsTrue(Mathf.Approximately(path[0].x, offsetWaypoint0.x));
        Assert.IsTrue(Mathf.Approximately(path[0].y, offsetWaypoint0.y));

        Assert.IsTrue(Mathf.Approximately(path[1].x, offsetWaypoint1.x));
        Assert.IsTrue(Mathf.Approximately(path[1].y, offsetWaypoint1.y));
    }

    [Test]
    public void ApplyPerpendicularOffsetToPath_AppliesCorrectOffsetForThreeWaypoints()
    {
        Vector2 waypoint0 = new Vector2(0, 0);
        Vector2 waypoint1 = new Vector2(0, 1);
        Vector2 waypoint2 = new Vector2(1, 1);

        float perpendicularOffset = 0.5f;

        Vector2[] path = new Vector2[] { waypoint0, waypoint1, waypoint2 };

        EnemyPath.ApplyPerpendicularOffsetToPath(path, perpendicularOffset);

        Vector2 offsetVector0 = new Vector2(-0.5f, 0.0f);
        Vector2 offsetVector1 = new Vector2(0.0f, -0.5f);

        Vector2 offsetWaypoint0 = waypoint0 + offsetVector0;
        Vector2 offsetWaypoint1 = waypoint1 + offsetVector0 + offsetVector1;
        Vector2 offsetWaypoint2 = waypoint2 + offsetVector1;

        Assert.IsTrue(Mathf.Approximately(path[0].x, offsetWaypoint0.x));
        Assert.IsTrue(Mathf.Approximately(path[0].y, offsetWaypoint0.y));

        Assert.IsTrue(Mathf.Approximately(path[1].x, offsetWaypoint1.x));
        Assert.IsTrue(Mathf.Approximately(path[1].y, offsetWaypoint1.y));

        Assert.IsTrue(Mathf.Approximately(path[2].x, offsetWaypoint2.x));
        Assert.IsTrue(Mathf.Approximately(path[2].y, offsetWaypoint2.y));
    }

    private static float GetPathLength(Vector2[] waypoints)
    {
        float totalLength = 0.0f;

        Vector2 oldPosition = waypoints[0];

        for (int index = 1; index < waypoints.Length; ++index)
        {
            Vector2 newPosition = waypoints[index];
            
            Vector2 delta = newPosition - oldPosition;

            float distance = delta.magnitude;

            totalLength += distance;

            oldPosition = newPosition;
        }

        return totalLength;
    }

    [Test]
    public void ApplyPerpendicularOffsetToPath_ShiftedPathHasSameLengthAsOriginal()
    {
        Vector2 waypoint0 = new Vector2(0, 0);
        Vector2 waypoint1 = new Vector2(0, 2);
        Vector2 waypoint2 = new Vector2(2, 2);
        Vector2 waypoint3 = new Vector2(2, 4);
        Vector2 waypoint4 = new Vector2(4, 4);

        float perpendicularOffset = 0.5f;
        
        Vector2[] path = new Vector2[] { waypoint0, waypoint1, waypoint2, waypoint3, waypoint4 };

        float oldLength = GetPathLength(path);

        EnemyPath.ApplyPerpendicularOffsetToPath(path, perpendicularOffset);

        float newLength = GetPathLength(path);

        Assert.IsTrue(Mathf.Approximately(oldLength, newLength));
    }

    [Test]
    public void CalculateTotalPathLength_ProducesExpectedResult()
    {
        Vector2 waypoint0 = new Vector2(0, 0);
        Vector2 waypoint1 = new Vector2(0, 2);
        Vector2 waypoint2 = new Vector2(2, 2);
        Vector2 waypoint3 = new Vector2(2, 4);
        Vector2 waypoint4 = new Vector2(4, 4);

        Vector2[] path = new Vector2[] { waypoint0, waypoint1, waypoint2, waypoint3, waypoint4 };

        float reference = GetPathLength(path);
        float result = EnemyPath.CalculateTotalPathLength(path);

        Assert.IsTrue(Mathf.Approximately(result, reference));
    }

    [Test]
    public void CalculateTotalPathLength_ReturnsZeroOnNull()
    {
        float result = EnemyPath.CalculateTotalPathLength(null);

        Assert.AreEqual(0, result);
    }

    [Test]
    public void CalculateTotalPathLength_ReturnsZeroWithLessThanTwoWaypoints()
    {
        Vector2 waypoint = new Vector2(0, 0);

        Vector2[] path = new Vector2[] { waypoint };

        float result = EnemyPath.CalculateTotalPathLength(path);

        Assert.AreEqual(0, result);
    }

    [Test]
    public void ApplyPerpendicularOffsetToPath_DoesNothingWithSingleWaypoint()
    {
        Vector2 waypoint0 = new Vector2(0, 0);

        Vector2[] path = new Vector2[] { waypoint0 };

        EnemyPath.ApplyPerpendicularOffsetToPath(path, 1.0f);

        Assert.AreEqual(path[0], waypoint0);
    }

    [Test]
    public void ApplyPerpendicularOffsetToPath_DoesNothingOnNull()
    {
        EnemyPath.ApplyPerpendicularOffsetToPath(null, 0.0f);
    }

    [Test]
    public void TargetNextWaypoint_AdjustsRemainingDistanceProperly()
    {
        Vector2 waypoint0 = new Vector2(0, 0);
        Vector2 waypoint1 = new Vector2(0, 2);
        Vector2 waypoint2 = new Vector2(2, 2);
        Vector2 waypoint3 = new Vector2(2, 4);
        Vector2 waypoint4 = new Vector2(4, 4);

        Vector2[] waypoints = new Vector2[] { waypoint0, waypoint1, waypoint2, waypoint3, waypoint4 };

        float length = GetPathLength(waypoints);

        EnemyPath path = new EnemyPath(waypoints, 0.0f);

        path.TargetNextWaypoint();

        float remainingLength = path.RemainingPathLength;

        float distance01 = Vector2.Distance(waypoint0, waypoint1);

        Assert.IsTrue(Mathf.Approximately(distance01 + remainingLength, length));
    }

    [Test]
    public void TargetNextWaypoint_ReachesZeroRemainingDistanceWhenEndIsReached()
    {
        Vector2 waypoint0 = new Vector2(0, 0);
        Vector2 waypoint1 = new Vector2(0, 2);
        Vector2 waypoint2 = new Vector2(2, 2);
        Vector2 waypoint3 = new Vector2(2, 4);
        Vector2 waypoint4 = new Vector2(4, 4);

        Vector2[] waypoints = new Vector2[] { waypoint0, waypoint1, waypoint2, waypoint3, waypoint4 };

        EnemyPath path = new EnemyPath(waypoints, 0.0f);

        for (int index = 0; index < 4; ++index)
        {
            path.TargetNextWaypoint();
        }

        float remainingLength = path.RemainingPathLength;

        Assert.IsTrue(Mathf.Approximately(remainingLength, 0.0f));
    }
}
