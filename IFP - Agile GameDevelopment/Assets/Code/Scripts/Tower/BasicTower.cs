using UnityEngine;

public class BasicTower : BaseTower
{
    public GameObject BulletPrefab;

    private Enemy target;

    public void Awake()
    {
        actionRadius = 2.0f;
    }

    protected override void TowerUpgrade()
    {
        // TODO: Add funtionality
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

#if UNITY_EDITOR

#pragma warning disable SA1202

    public bool Test_PerformAction()
    {
        return PerformAction();
    }

    public Enemy Test_FindBestTarget()
    {
        return FindBestTarget(actionRadius);
    }

#endif
}