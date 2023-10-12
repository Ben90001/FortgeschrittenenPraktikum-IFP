using UnityEngine;

public class Blockade: BaseTower
{
    private float currentHealth;
    public float MaxHealth = 100.0f;
    public Blockade()
    {
        currentHealth = MaxHealth; 
    }

    public float GetHealth() {
        return currentHealth;
    }
    public void ApplyDamage(float damage) {

        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Collapse();
        }
    }

    private void Collapse()
    {
        
        gameObject.SetActive(false);
    }

    public bool IsAlive()
    {
        return currentHealth > 0;
    }
    protected override bool PerformAction() {
        return false;
    }
    protected override void TowerUpgrade()
    {
        //TODO: add funtionality
    }
}

