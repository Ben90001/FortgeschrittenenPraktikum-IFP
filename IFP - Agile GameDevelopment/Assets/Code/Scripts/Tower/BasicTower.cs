using UnityEngine;

public class BasicTower : BaseTower
{
    public GameObject BulletPrefab;

    private Enemy target;

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

    //Used for Tests only
    public bool PerformActionForTesting()
    {
        return PerformAction();
    }
}