using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class BaseTower : MonoBehaviour
{
    public float SecondsBetweenActions;

    private float actionTimer;

    [SerializeField] protected float actionRadius;

    // private int level;
    // private List<int> upgradeCosts;
    // private List<Sprite> levelLook;

    // private float damage;
    // private float nextActionTime;
    // private Color bulletColor;
    // private Transform bulletSpawnPosition;

    public void FixedUpdate()
    {
        float oldActionTimer = actionTimer;

        actionTimer -= Time.fixedDeltaTime;

        if (actionTimer <= 0.0f)
        {
            if (PerformAction())
            {
                actionTimer = SecondsBetweenActions;
            }
            else
            {
                actionTimer = oldActionTimer;
            }
        }
    }

    protected abstract void TowerUpgrade();

    protected abstract bool PerformAction();

    protected Enemy FindBestTarget(float radius)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius);

        Enemy bestTarget = null;
        float bestTargetPathDistance = float.MaxValue;

        foreach (Collider2D collider in colliders)
        {
            Enemy enemy = collider.GetComponent<Enemy>();

            if (enemy != null)
            {
                float pathDistance = enemy.GetRemainingDistanceAlongPath();

                if (bestTarget == null || pathDistance < bestTargetPathDistance)
                {
                    bestTargetPathDistance = pathDistance;
                    bestTarget = enemy;
                }
            }
        }

        return bestTarget;
    }

    //Methods used only for testing
    public float GetActionTimer() 
    {
        return actionTimer;
    }
}