using System.Collections.Generic;
using UnityEngine;

public class IceTower : BaseTower
{
    [SerializeField]
    public float SlowFactor;

    private List<Enemy> slowedEnemies = new List<Enemy>();

    protected override void TowerUpgrade()
    {
        // TODO: Add funtionality
    }

    protected override bool PerformAction()
    {
#if false
        bool success = false;

        for (int i = slowedEnemies.Count - 1; i >= 0; i--)
        {
            Enemy slowedEnemy = slowedEnemies[i];
            if (slowedEnemy == null || Vector2.Distance(transform.position, slowedEnemy.transform.position) > 2.0f)
            {
                // If the slowed enemy has gone out of range, call RemoveSlow and remove it from the list
                slowedEnemy.RemoveSlow();
                slowedEnemies.RemoveAt(i);
            }
        }

        Enemy target = FindBestTarget(2.0f);

        if (target != null)
        {
            AttackIceTower(target);
        }

        return success;
#endif
        return true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy target = collision.GetComponent<Enemy>();

        if (target != null)
        {
            FreezeTarget(target);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Enemy target = collision.GetComponent<Enemy>();

        if (target != null)
        {
            target.RemoveSlow();
        }
    }

    private void FreezeTarget(Enemy target)
    {
        target.ApplySlow(SlowFactor);

        // slowedEnemies.Add(target);
    }
}
