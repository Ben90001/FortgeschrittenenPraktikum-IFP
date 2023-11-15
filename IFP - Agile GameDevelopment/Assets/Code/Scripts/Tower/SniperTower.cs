using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperTower : BaseTower
{
    public GameObject BulletPrefab;

    private Enemy target;

    public float Damage;
    public float BulletSpeed;

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
            bullet.Damage = this.Damage;
            bullet.Speed = this.BulletSpeed;
            bullet.Target = target;

            success = true;
        }


        return success;
    }
}
