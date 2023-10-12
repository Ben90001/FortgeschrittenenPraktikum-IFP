using UnityEngine;

public class Blockade: BaseTower
{
    
    public float GetHealth() {
        return 0.0f; //Temporary placeholder value
    }
    public void ApplyDamage(float damage) { }

    public bool IsAlive()
    {
        return true;
    }
    protected override bool PerformAction() {
        return false;
    }
    protected override void TowerUpgrade()
    {
        //TODO: add funtionality
    }
}

