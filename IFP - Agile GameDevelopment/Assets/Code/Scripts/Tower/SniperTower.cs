public class SniperTower : BaseTower
{
    protected override void TowerUpgrade()
    {
    }

    protected override bool PerformAction()
    {
        bool success = false;

        Enemy target = FindBestTarget(actionRadius);

        if (target != null)
        {
            ShootBulletAtTarget(bulletPrefab, target);

            success = true;
        }

        return success;
    }
}
