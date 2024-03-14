using System.Collections.Generic;
using UnityEngine;

public class IceTower : BaseTower
{
    [SerializeField]
    private float slowFactor;

    protected override void TowerUpgrade()
    {
    }

    protected override bool PerformAction()
    {
        return true;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        Enemy target = collision.GetComponent<Enemy>();

        if (target != null)
        {
            target.ApplySlow(slowFactor);
        }
    }
}
