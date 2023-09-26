#if false
using UnityEngine;

public class Blockade : BaseTower
{
    private int blockadeHealth;
    private int maxBlockadeHealth;
    private bool isBlockadeActive;

    public override void TowerUpgrade()
    {
    }

    public override void FixedUpdate()
    {
    }

    public void TakeDamage(int damage)
    {
        if (this.isBlockadeActive)
        {
            this.blockadeHealth -= damage;

            if (this.blockadeHealth <= 0)
            {
                this.DeactivateBlockade();
            }
        }
    }

    public void ActivateBlockade()
    {
        this.isBlockadeActive = true;
        this.blockadeHealth = this.maxBlockadeHealth;
    }

    public void DeactivateBlockade()
    {
        this.isBlockadeActive = false;
    }

    protected override void PerformAction()
    {
    }
}
#endif
