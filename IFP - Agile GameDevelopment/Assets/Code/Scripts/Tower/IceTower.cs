using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceTower : BaseTower
{
  
    public float SlowFactor = 0.0001f;

    protected override void TowerUpgrade()
    {
    }

    protected override bool PerformAction()
    {
        bool success = false;

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
    }
}

