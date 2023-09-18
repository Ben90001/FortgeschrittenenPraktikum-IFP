using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperTower : BaseTower
{
    private Transform firingPoint;
    private float attackRate;
    private int damage;
    private Color bulletColor;

    public override void TowerUpgrade()
    {
        // Upgrade-Logik für den Sniper Tower
    }

    public override void FixedUpdate()
    {
    }

    protected override void PerformAction()
    {
    }
}
