using UnityEngine;

public class Enemy : MonoBehaviour
{ 
    public float MovementSpeed = 10.0f;
    public float Health;

    private bool isSlowed = false;

    private float slowFactor = 1.0f;
    private float originalMovementSpeed;

    //

    private LevelManager levelManager;

    private EnemyPath path;

    /// <summary>
    /// Initializes the enemy.
    /// </summary>
    /// <param name="levelManager">Reference to LevelManager in Scene.</param>
    /// <param name="waypoints">List of waypoints the enemy should follow.</param>
    /// <param name="offset">The perpendicular offset that should be applied to the waypoints.</param>
    public void Initialize(LevelManager levelManager, Vector2[] waypoints, float offset)
    {
        this.levelManager = levelManager;

        InitializePath(waypoints, offset);

        InitializePositionFromPath(this.path);
    }

    /// <summary>
    /// Calculates the distance the enemy has to move to reach the last waypoint.
    /// </summary>
    /// <returns>The calculated distance.</returns>
    public float GetRemainingDistanceAlongPath()
    {
        Vector2 position = transform.position;

        Vector2 target = path.GetCurrentTarget();

        float pathLength = path.RemainingPathLength;
        float targetDistance = Vector2.Distance(position, target);

        float result = pathLength + targetDistance;

        return result;
    }

    private void FixedUpdate()
    {
        bool reachedPathEnd = FollowPath();

        if (reachedPathEnd)
        {
            levelManager.DecreasePlayerLives();

            Destroy(gameObject);
        }
    }

    private bool FollowPath()
    {
        float distanceToTravel = Time.fixedDeltaTime * this.MovementSpeed;

        bool hasReachedEnd = MoveDistanceAlongPath(distanceToTravel);

        return hasReachedEnd;
    }

    private void InitializePath(Vector2[] waypoints, float offset)
    {
        this.path = new EnemyPath(waypoints, offset);
    }

    private void InitializePositionFromPath(EnemyPath path)
    {
        transform.position = path.GetStartingPosition();
    }

    /// <summary>
    /// Moves the enemy transform along its path.
    /// </summary>
    /// <param name="distanceToTravel">The distance the enemy should move by.</param>
    /// <returns></returns>
    private bool MoveDistanceAlongPath(float distanceToTravel)
    {
        bool hasReachedEnd = path.HasReachedEndOfPath();

        Vector2 position = transform.position;

        Vector2 target = path.GetCurrentTarget();

        while (!hasReachedEnd && !Mathf.Approximately(distanceToTravel, 0.0f))
        {
            bool hasReachedTarget = MovePositionTowardsTarget(target, ref position, ref distanceToTravel);

            if (hasReachedTarget)
            {
                target = path.TargetNextWaypoint();
            }

            hasReachedEnd = path.HasReachedEndOfPath();
        }

        transform.position = position;

        return hasReachedEnd;
    }

    /// <summary>
    /// Moves the provided position towards the target. Position is at most moved by amount distanceToTravel or until 
    /// it reached the target. The position and distanceToTravel are modified to reflect the state after the position
    /// was moved.
    /// </summary>
    /// <returns>True when the target was reached.</returns>
    private static bool MovePositionTowardsTarget(Vector2 target, ref Vector2 position, ref float distanceToTravel)
    {
        bool reachedTarget = false;

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
            reachedTarget = true;
        }

        return reachedTarget;
    }

    // TODO: Refactor

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

    public void ApplySlow(float factor)
    {
        if (!isSlowed)
        {
            isSlowed = true;
            slowFactor = factor;
            originalMovementSpeed = MovementSpeed;
            MovementSpeed *= slowFactor;

            Debug.Log("Enemy slowed down!");
        }
    }

    public void RemoveSlow()
    {
        if (isSlowed)
        {
            isSlowed = false;
            MovementSpeed = originalMovementSpeed;

            Debug.Log("Slow removed from Enemy!");
        }
    }

    public void RestoreSpeed()
    {
        MovementSpeed = 10f;
    }

#if UNITY_EDITOR

    /// <summary>
    /// Initializes the enemy with a specified starting position.
    /// </summary>
    public void Initialize(LevelManager levelManager, Vector2[] waypoints, Vector2 startingPosition)
    {
        Initialize(levelManager, waypoints, 0.0f);

        transform.position = startingPosition;
    }

    public static bool Test_MovePositionTowardsTarget(Vector2 target, ref Vector2 position, ref float distanceToTravel)
    {
        return MovePositionTowardsTarget(target, ref position, ref distanceToTravel);
    }

    public void Test_MoveDistanceAlongPath(float distanceToTravel)
    {
        MoveDistanceAlongPath(distanceToTravel);
    }

#endif
}
