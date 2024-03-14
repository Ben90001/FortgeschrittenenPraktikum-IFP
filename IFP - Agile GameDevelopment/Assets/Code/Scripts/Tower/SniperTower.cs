public class SniperTower : BaseTower
{
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
            ShootBulletAtTarget(bulletPrefab, target);

            success = true;
        }

        return success;
    }
}
