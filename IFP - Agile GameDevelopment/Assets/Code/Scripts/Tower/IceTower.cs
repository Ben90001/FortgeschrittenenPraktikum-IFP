using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceTower : BaseTower
{
    private float nextActionTime;
    private float targetingRange;
    private Transform firingPoint;
    private float attackRate;
    private int damage;
    private float freezeTime;

    public override void TowerUpgrade()
    {
    }

    public override void FixedUpdate()
    {
    }

    protected override void PerformAction()
    {
        if (Time.time >= this.nextActionTime)
        {
            this.FindBestTarget();

            this.FreezeEnemies();

            this.nextActionTime = Time.time + this.attackRate;
        }
    }

    private void FreezeEnemies()
    {
    }
}
