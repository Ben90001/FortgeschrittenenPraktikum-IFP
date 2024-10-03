using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The base blass each tower inherits. 
/// </summary>
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

    /// <summary>
    /// To be inplemented by children. Called once every action intervall.
    /// </summary>
    /// <returns></returns>
    protected abstract bool PerformAction();

    /// <summary>
    /// Finds the best target to shoot. The best target is the one closest to its path end.
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

    /// <summary>
    /// Gets all targets within the circular area defined by radius centered at tower.
    /// </summary>
    /// <param name="radius">The radius to use.</param>
    /// <returns>List of enemies in area.</returns>
    protected Enemy[] FindAllTargets(float radius)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius);

        List<Enemy> targets = new List<Enemy>();

        foreach (Collider2D collider in colliders)
        {
            Enemy target = collider.GetComponent<Enemy>();

            if (target != null)
            {
                targets.Add(target);
            }
        }

        return targets.ToArray();
    }

    /// <summary>
    /// Instantiates a bullet object to target the Enemy target.
    /// </summary>
    /// <param name="bulletPrefab">The bullet prefab to instantiate.</param>
    /// <param name="target">The enemy to target.</param>
    protected void ShootBulletAtTarget(GameObject bulletPrefab, Enemy target)
    {
        GameObject bulletObject = Instantiate(bulletPrefab, transform.position, Quaternion.identity);

        Bullet bullet = bulletObject.GetComponent<Bullet>();

        bullet.Initialize(target, bulletSpeed, bulletDamage);
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