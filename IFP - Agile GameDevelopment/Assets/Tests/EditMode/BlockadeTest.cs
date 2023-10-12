using UnityEngine;
using NUnit.Framework;

public class BlockadeTowerTests
{
   
     private Blockade blockade = new GameObject().AddComponent<Blockade>();
  

    [Test]
    public void InitialHealth_IsMaxHealth()
    {
        float maxHealth = 100.0f;
        Assert.AreEqual(maxHealth, blockade.GetHealth());
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

