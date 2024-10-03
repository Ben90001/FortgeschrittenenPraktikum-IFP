using UnityEngine;

/// <summary>
/// Holds all relevant data to specify the behavior of a single wave.
/// </summary>
[System.Serializable]
public struct Wave
{
    /// <summary>
    /// The money the wave should grant per enemy.
    /// </summary>
    [Min(0)]
    public int CurrencyWorth;

    /// <summary>
    /// The number of enemies in this wave.
    /// </summary>
    [Min(1)]
    public int EnemyCount;

    /// <summary>
    /// The health each enemy should have.
    /// </summary>
    [Min(1)]
    public int EnemyHealth;

    /// <summary>
    /// The seconds between each spawning enemy.
    /// </summary>
    [Min(0.1f)]
    public float SecondsBetweenSpawns;
}
