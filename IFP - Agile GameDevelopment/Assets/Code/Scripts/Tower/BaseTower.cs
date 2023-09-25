using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseTower : MonoBehaviour
{
    private int level;
    private List<int> upgradeCosts;
    private List<Sprite> levelLook;
    private float secondsBetweenActions;
    private float damage;
    private float nextActionTime;

    // private Bullet bullet;

    private Color bulletColor;
    private Transform bulletSpawnPosition;
    private Transform target;

    public abstract void TowerUpgrade();

    public abstract void FixedUpdate();

    protected virtual void FindBestTarget()
    {
        // find best target
    }

    protected abstract void PerformAction();

    private void OnDrawGizmosSelected()
    {
        // visualize range in unity editor
    }
}