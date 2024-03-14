using UnityEngine;

public abstract class BaseTower : MonoBehaviour
{
    [SerializeField]
    protected GameObject bulletPrefab;

    [SerializeField]
    protected float secondsBetweenActions;

    [SerializeField]
    protected float actionRadius;

    [SerializeField]
    protected float bulletDamage;

    [SerializeField]
    protected float bulletSpeed;

    private float actionTimer;

    public void FixedUpdate()
    {
        if (secondsBetweenActions > 0.0f)
        {
            float newActionTimer = actionTimer - Time.fixedDeltaTime;

            if (newActionTimer <= 0.0f)
            {
                if (PerformAction())
                {
                    newActionTimer = secondsBetweenActions;
                }
            }

            actionTimer = newActionTimer;
        }
    }

    protected abstract void TowerUpgrade();

    protected abstract bool PerformAction();

    /// <summary>
    /// Finds the best target to shoot. The best target is the one closest to its goal.
    /// </summary>
    /// <param name="radius">The radius to search.</param>
    /// <returns>The found target. Null if no target was found.</returns>
    protected Enemy FindBestTarget(float radius)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius);

        Enemy bestTarget = null;
        float bestTargetPathDistance = float.MaxValue;

        foreach (Collider2D collider in colliders)
        {
            Enemy enemy = collider.GetComponent<Enemy>();

            if (enemy != null)
            {
                float pathDistance = enemy.GetRemainingDistanceAlongPath();

                if (bestTarget == null || pathDistance < bestTargetPathDistance)
                {
                    bestTargetPathDistance = pathDistance;
                    bestTarget = enemy;
                }
            }
        }

        return bestTarget;
    }

    protected void ShootBulletAtTarget(GameObject bulletPrefab, Enemy target)
    {
        GameObject bulletObject = Instantiate(bulletPrefab, transform.position, Quaternion.identity);

        Bullet bullet = bulletObject.GetComponent<Bullet>();

        bullet.Target = target;
        bullet.Damage = bulletDamage;
        bullet.Speed = bulletSpeed;
    }

#if UNITY_EDITOR

#pragma warning disable SA1202
#pragma warning disable SA1201

    public float Test_SecondsBetweenActions
    {
        get { return secondsBetweenActions; }
    }

    public float Test_GetActionTimer()
    {
        return actionTimer;
    }

    public bool Test_PerformAction()
    {
        return PerformAction();
    }

    public Enemy Test_FindBestTarget()
    {
        return FindBestTarget(actionRadius);
    }
#endif
}