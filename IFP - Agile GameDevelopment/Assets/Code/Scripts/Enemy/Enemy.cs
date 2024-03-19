using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float movementSpeed = 10.0f;

    [SerializeField] 
    private int currencyWorth = 10;
    
    private LevelManager levelManager;

    private EnemyPath path;

    private new Rigidbody2D rigidbody;

    private float health;

    private float slowFactor;

    /// <summary>
    /// Initializes the enemy.
    /// </summary>
    /// <param name="levelManager">Reference to LevelManager in Scene.</param>
    /// <param name="waypoints">List of waypoints the enemy should follow.</param>
    /// <param name="offset">The perpendicular offset that should be applied to the waypoints.</param>
    /// <param name="health">The health of the enemy.</param>
    public void Initialize(LevelManager levelManager, Vector2[] waypoints, float offset, float health)
    {
        this.levelManager = levelManager;
        this.health = health;
        this.rigidbody = GetComponent<Rigidbody2D>();
        this.slowFactor = 1.0f;

        InitializePath(waypoints, offset);

        InitializePositionFromPath(path);
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

    /// <summary>
    /// Apply specified amount of damage to self. Destroys self if health falls below 0.
    /// </summary>
    /// <param name="amount">Amount of damage.</param>
    public void ApplyDamage(float amount)
    {
        health -= amount;

        if (health <= 0.0f)
        {
            levelManager.IncreaseCurrency(currencyWorth);

            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Apply slow effect to self.
    /// </summary>
    /// <param name="slowFactor">The slow factor to apply.</param>
    public void ApplySlow(float slowFactor)
    {
        if (slowFactor < this.slowFactor)
        {
            this.slowFactor = slowFactor;
        }
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

    private void FixedUpdate()
    {
        bool reachedPathEnd = FollowPath();

        if (reachedPathEnd)
        {
            if (levelManager != null)
            {
                levelManager.DecreasePlayerLives();
            }

            Destroy(gameObject);
        }

        slowFactor = 1.0f;
    }

    private float GetCurrentMovementSpeed()
    {
        return movementSpeed * slowFactor;
    }

    private bool FollowPath()
    {
        bool hasReachedEnd = false;

        float distanceToTravel = Time.fixedDeltaTime * GetCurrentMovementSpeed();

        if (distanceToTravel > 0.0f)
        {
            hasReachedEnd = MoveDistanceAlongPath(distanceToTravel);
        }

        return hasReachedEnd;
    }

    private void InitializePath(Vector2[] waypoints, float offset)
    {
        path = new EnemyPath(waypoints, offset);
    }

    private void InitializePositionFromPath(EnemyPath path)
    {
        transform.position = path.GetStartingPosition();
    }

    /// <summary>
    /// Moves the enemy transform along its path.
    /// </summary>
    /// <param name="distanceToTravel">The distance the enemy should move by.</param>
    /// <returns>True if enemy has reached end of path.</returns>
    private bool MoveDistanceAlongPath(float distanceToTravel)
    {
        bool hasReachedEnd = path.HasReachedEndOfPath();

        Vector2 position = rigidbody.position;

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

        rigidbody.MovePosition(position);

        return hasReachedEnd;
    }

#if UNITY_EDITOR

#pragma warning disable SA1202

    public static bool Test_MovePositionTowardsTarget(Vector2 target, ref Vector2 position, ref float distanceToTravel)
    {
        return MovePositionTowardsTarget(target, ref position, ref distanceToTravel);
    }

    public void Initialize(LevelManager levelManager, Vector2[] waypoints, Vector2 startingPosition, float health)
    {
        Initialize(levelManager, waypoints, 0.0f, health);

        transform.position = startingPosition;
    }

    public void Test_MoveDistanceAlongPath(float distanceToTravel)
    {
        MoveDistanceAlongPath(distanceToTravel);
    }
#endif
}
