using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceTower : BaseTower
{
 

    public float SlowFactor = 0.0001f;
    private List<Enemy> slowedEnemies = new List<Enemy>();


    protected override void TowerUpgrade()
    {
    }

    protected override bool PerformAction()
    {
        bool success = false;

        
        for (int i = slowedEnemies.Count - 1; i >= 0; i--)
        {
            Enemy slowedEnemy = slowedEnemies[i];
            if (slowedEnemy == null || Vector2.Distance(transform.position, slowedEnemy.transform.position) > 2.0f)
            {
                //If the slowed enemy has gone out of range, call RemoveSlow and remove it from the list
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
    }


    private void AttackIceTower(Enemy target)
    {
        target.ApplySlow(SlowFactor);

        slowedEnemies.Add(target);
    }

}

