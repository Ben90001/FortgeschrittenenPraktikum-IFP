using NUnit.Framework;
using UnityEngine;

public class EnemyTests
{
    [Test]
    public void MovePositionTowardsTarget_DoesntMoveWhenAlreadyAtTarget()
    {
        Vector2 target = Vector2.one;
        Vector2 position = target;
        Vector2 original = position;

        float distanceToTravel = 2.0f;

        Enemy.Test_MovePositionTowardsTarget(target, ref position, ref distanceToTravel);

        Assert.AreEqual(original, position);
    }

    [Test]
    public void MovePositionTowardsTarget_ReachesTargetWhenAlreadyAtTarget()
    {
        Vector2 target = Vector2.one;
        Vector2 position = target;
        Vector2 original = position;

        float distanceToTravel = 0.0f;

        bool reachedTarget = Enemy.Test_MovePositionTowardsTarget(target, ref position, ref distanceToTravel);

        Assert.IsTrue(reachedTarget);
    }

    [Test]
    public void MovePositionTowardsTarget_ReachesTargetWhenCloseEnough()
    {
        Vector2 target = Vector2.up;
        Vector2 position = Vector2.zero;

        float distanceToTravel = 1.5f;

        bool reachedTarget = Enemy.Test_MovePositionTowardsTarget(target, ref position, ref distanceToTravel);

        Assert.IsTrue(Mathf.Approximately(target.x, position.x));
        Assert.IsTrue(Mathf.Approximately(target.y, position.y));

        Assert.IsTrue(reachedTarget);

        Assert.IsTrue(Mathf.Approximately(distanceToTravel, 0.5f));
    }

    [Test]
    public void MovePositionTowardsTarget_DoesNotReachTargetWhenTooFarAway()
    {
        Vector2 target = Vector2.one;
        Vector2 position = Vector2.zero;

        float distanceToTravel = 1.0f;

        bool reachedTarget = Enemy.Test_MovePositionTowardsTarget(target, ref position, ref distanceToTravel);

        Assert.IsFalse(Mathf.Approximately(target.x, position.x));
        Assert.IsFalse(Mathf.Approximately(target.y, position.y));

        Assert.IsFalse(reachedTarget);

        Assert.IsTrue(Mathf.Approximately(distanceToTravel, 0.0f));
    }

    [Test]
    public void MovePositionTowardsTarget_UnreachableTargetDoesNotModifyPositionAndAreDeclaredAsReached()
    {
        Vector2 target = Vector2.zero;
        Vector2 position = Vector2.zero;
        Vector2 original = position;

        target.x = float.PositiveInfinity;

        float distanceToTravel = 1.0f;

        bool reachedTarget = Enemy.Test_MovePositionTowardsTarget(target, ref position, ref distanceToTravel);

        Assert.AreEqual(position, original);

        Assert.IsTrue(reachedTarget);
    }

    [Test]
    public void MovePositionTowardsTarget_NaNTargetDoesNotModifyPositionAndAreDeclaredAsReached()
    {
        Vector2 target = Vector2.zero;
        Vector2 position = Vector2.zero;
        Vector2 original = position;

        target.x = float.NaN;

        float distanceToTravel = 1.0f;

        bool reachedTarget = Enemy.Test_MovePositionTowardsTarget(target, ref position, ref distanceToTravel);

        Assert.AreEqual(position, original);

        Assert.IsTrue(reachedTarget);
    }

    [Test]
    public void GetRemainingDistanceAlongPath_ProducesExpectedResultOnSimplePath()
    {
        LevelManager levelManager = new GameObject().AddComponent<LevelManager>();

        Vector2 waypoint0 = new Vector2(0, 0);
        Vector2 waypoint1 = new Vector2(0, 2);
        
        Vector2[] waypoints = new Vector2[] { waypoint0, waypoint1 };

        Enemy enemy = new GameObject().AddComponent<Enemy>();

        enemy.Initialize(levelManager, waypoints);

        float distance = enemy.GetRemainingDistanceAlongPath();

        Assert.IsTrue(Mathf.Approximately(distance, 2.0f));
    }

    [Test]
    public void GetRemainingDistanceAlongPath_ProducesExpectedResultAfterMovement()
    {
        LevelManager levelManager = new GameObject().AddComponent<LevelManager>();

        Vector2 waypoint0 = new Vector2(0, 0);
        Vector2 waypoint1 = new Vector2(0, 2);

        Vector2[] waypoints = new Vector2[] { waypoint0, waypoint1 };

        Enemy enemy = new GameObject().AddComponent<Enemy>();

        enemy.Initialize(levelManager, waypoints);

        enemy.Test_MoveDistanceAlongPath(0.5f);

        float distance = enemy.GetRemainingDistanceAlongPath();

        Assert.IsTrue(Mathf.Approximately(distance, 1.5f));
    }
}
