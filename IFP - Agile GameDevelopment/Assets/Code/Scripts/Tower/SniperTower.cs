using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperTower : BaseTower
{
   public GameObject BulletPrefab;

    private Enemy target;

    protected override void TowerUpgrade()
    {
    }

    protected override bool PerformAction()
    {
        bool success = false;

        Enemy target = FindBestTarget(actionRadius);

        if (target != null)
        {
            GameObject bulletObject = Instantiate(BulletPrefab, transform.position, Quaternion.identity);

            Bullet bullet = bulletObject.GetComponent<Bullet>();

            bullet.Target = target;

            success = true;
        }

        return success;
    }
}
