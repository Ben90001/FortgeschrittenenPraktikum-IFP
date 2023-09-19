using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceTower : BaseTower
{
    private float nextActionTime;
    private float targetingRange;
    private Transform firingPoint;
    private float attackRate;
    private int damage;
    private float freezeTime;

    public override void TowerUpgrade()
    {
        // Upgrade-Logik für den IceTower.
        // Anpassen der Eigenschaften des Turms beim Upgrade.
    }

    public override void FixedUpdate()
    {
    }

    protected override void PerformAction()
    {
        // Überprüfe, ob es Zeit für eine Aktion ist.
        if (Time.time >= this.nextActionTime)
        {
            // Finde das beste Ziel im Zielbereich.
            this.FindBestTarget();

            // Führe die Aktion aus, z.B. das Einfrieren der Feinde.
            this.FreezeEnemies();

            // Setze die nächste Aktionszeit basierend auf der Angriffsrate.
            this.nextActionTime = Time.time + this.attackRate;
        }
    }

    private void FreezeEnemies()
    {
        // hier die Aktion zum Einfrieren der Feinde.
        // Verlangsamung der Bewegung der Feinde für 'freezeTime'.
    }
}
