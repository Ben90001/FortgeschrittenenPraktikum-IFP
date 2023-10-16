using System.Collections.Generic;
using UnityEngine;

public class Blockade : BaseTower
{
    private float currentHealth;
    public float maxHealth = 4f;
    private List<Enemy> stoppedEnemiesList = new List<Enemy>();
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
       

        if (currentHealth == 0)
        {

            gameObject.SetActive(false);
            return;
        }

    }

    private void StartResettingSpeed()
    {
       
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
        
    }
}
