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

    public void Initialize(LevelManager levelManager, Vector2[] waypoints)
    {
        this.levelManager = levelManager;

        InitializePathWithRandomOffset(waypoints);

        transform.position = this.path.GetStartingPosition();
    }

    public void Initialize(LevelManager levelManager, Vector2[] waypoints, Vector2 startingPosition)
    {
        Initialize(levelManager, waypoints);

        transform.position = startingPosition;
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

    private void InitializePathWithRandomOffset(Vector2[] waypoints)
    {
        float offset = UnityEngine.Random.Range(-0.3f, 0.3f);

        this.path = new EnemyPath(waypoints, offset);
    }

    private bool FollowPath()
    {
        float distanceToTravel = Time.fixedDeltaTime * this.MovementSpeed;

        bool hasReachedEnd = MoveDistanceAlongPath(distanceToTravel);

        return hasReachedEnd;
    }

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
