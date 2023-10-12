using UnityEngine;
using NUnit.Framework;

public class BlockadeTowerTests
{
   
     private Blockade blockade = new GameObject().AddComponent<Blockade>();


    [Test]
    public void Blockade_InitialHealth_IsMaxHealth()
    {
        float currentHealth = blockade.GetHealth();
        Assert.AreEqual(blockade.MaxHealth, currentHealth); // Überprüfen, ob currentHealth zu Beginn MaxHealth entspricht.
    }

    [Test]
    public void ApplyDamage_DecreasesHealth()
    {
        float maxHealth = blockade.GetHealth();
        float damage = 30.0f;
        blockade.ApplyDamage(damage);
        Assert.AreEqual(maxHealth - damage, blockade.GetHealth());
    }

    [Test]
    public void ApplyDamage_ExceedingHealth_Collapses()
    {
        float maxHealth = blockade.GetHealth();
        blockade.ApplyDamage(maxHealth + 10.0f); // Damage exceeding health
        Assert.IsFalse(blockade.IsAlive()); // Implement a method IsAlive() to check if the blockade is still standing.
    }

   
}

