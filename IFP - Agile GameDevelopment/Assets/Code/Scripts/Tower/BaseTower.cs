using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class BaseTower : MonoBehaviour
{
    public float SecondsBetweenActions;

    private float actionTimer;

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
                actionTimer += SecondsBetweenActions;
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
        Enemy result = null;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius);

        foreach (Collider2D collider in colliders)
        {
            Enemy enemy = collider.GetComponent<Enemy>();

            if (enemy != null)
            {
                result = enemy;
                break;
            }
        }

        return result;
    }

    //Methods exclusively for testing
    public float GetActionTimer() 
    {
        return actionTimer;
    }
}