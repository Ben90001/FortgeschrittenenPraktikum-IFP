using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradedBasicTower : BaseTower
{
    public GameObject BulletPrefab;

    private Enemy target;

    public void Awake()
    {
        actionRadius = 2.0f;
    }

    protected override void TowerUpgrade()
    {
        //TODO: add funtionality
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

    //Methodes for testing only

    public bool PerformActionForTests()
    {
        return PerformAction();
    }
    public Enemy FindBestTargetForTests()
    {
        return FindBestTarget(actionRadius);
    }
}
