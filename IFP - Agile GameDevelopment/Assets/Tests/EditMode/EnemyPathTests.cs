using NUnit.Framework;
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
}
