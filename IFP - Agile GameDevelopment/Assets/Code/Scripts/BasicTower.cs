using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicTower : BaseTower
{
    private Transform target;
    private Transform firingPoint;
    private float attackRate;
    private int damage;
    private Color bulletColor;

    public override void TowerUpgrade()
    {
    }

    public override void FixedUpdate()
    {
    }

    protected override void PerformAction()
    {
        // Angriffsverhalten für den Basic Tower.
    }
}