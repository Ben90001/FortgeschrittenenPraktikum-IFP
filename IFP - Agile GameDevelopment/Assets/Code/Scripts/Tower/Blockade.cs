using UnityEngine;

public class Blockade : BaseTower
{
    private int blockadeHealth;
    private int maxBlockadeHealth;
    private bool isBlockadeActive;

    public override void TowerUpgrade()
    {
        // Upgrade-Logik für die Blockade.
    }

    public override void FixedUpdate()
    {
        // periodische Aktualisierungen der Blockade
        // Überprüfung des Blockadezustands
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
        // Aktiviere die Blockade, z. B. ändere den Zustand und setze die Gesundheit zurück.
        this.isBlockadeActive = true;
        this.blockadeHealth = this.maxBlockadeHealth;
    }

    public void DeactivateBlockade()
    {
        // Deaktiviere die Blockade, z. B. ändere den Zustand.
        this.isBlockadeActive = false;
    }

    protected override void PerformAction()
    {
        // Blockade hat keine spezielle Aktion, da sie nicht angreift.
    }
}
