using System.Collections.Generic;
using UnityEngine;

public class Blockade : BaseTower
{
    private float currentHealth;
    public float maxHealth = 4f;
    private List<Enemy> stoppedEnemiesList = new List<Enemy>();
    private bool resettingSpeed = false;
    private float resetSpeedDelay = 0.25f;

    public Blockade()
    {
        currentHealth = maxHealth;
    }

    public float GetHealth()
    {
        return currentHealth;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Collision");

        Enemy target = collision.gameObject.GetComponent<Enemy>();

        target.MovementSpeed = 0f;
        stoppedEnemiesList.Add(target);
        ApplyDamage(1);

        if (currentHealth <= 0)
        {
            StartResettingSpeed();
        }
    }

    private void Update()
    {
        /* The problem is that you are using FindBestTarget() to get the enemy you want to stop.
         * What happens when this function returns an enemy that is already stopped?
         * This would cause the new enemy to not be stopped until it is randomly selected by 
         * FindBestTarget(). 
         * I'd suggest using a trigger collider the size of the tower and OnTriggerEnter() to stop
         * enemies. 
         * Also it might then still be useful to set the stopped enemy position to exactly the border
         * of this tower.
         */
#if false
        if (currentHealth == 0)
        {
            
            gameObject.SetActive(false);
            return;
        }

        Enemy target = FindBestTarget(0.5f);

        if (target != null && !stoppedEnemiesList.Contains(target))
        {
            target.MovementSpeed = 0f;
            stoppedEnemiesList.Add(target);
            ApplyDamage(1); 

            
            if (currentHealth <= 0)
            {
                StartResettingSpeed();
            }
        }
#endif
    }

    private void StartResettingSpeed()
    {
        resettingSpeed = true;
        InvokeRepeating("ResetNextEnemySpeed", resetSpeedDelay, resetSpeedDelay);
    }

    private void ResetNextEnemySpeed()
    {
        if (stoppedEnemiesList.Count > 0)
        {
            Enemy stoppedEnemy = stoppedEnemiesList[0];

            stoppedEnemy.MovementSpeed = 5f;
            stoppedEnemiesList.RemoveAt(0);

            
            if (stoppedEnemiesList.Count == 0)
            {
                resettingSpeed = false;
                CancelInvoke("ResetNextEnemySpeed");
            }
        }
    }

    public void ApplyDamage(float damage)
    {
        currentHealth -= damage;
        Debug.Log("Blockade currentHealth: " + currentHealth);
    }

    public bool IsAlive()
    {
        return currentHealth > 0;
    }

    protected override bool PerformAction()
    {
        return false;
    }

    protected override void TowerUpgrade()
    {
        // TODO: Fügen Sie Funktionalität hinzu
    }
}
