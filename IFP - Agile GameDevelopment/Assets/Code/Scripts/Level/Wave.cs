using UnityEngine;

/// <summary>
/// Holds all relevant data to specify the behavior of a single wave.
/// </summary>
[System.Serializable]
public struct Wave
{
    [Min(0)]
    public int CurrencyWorth;

    [Min(1)]
    public int EnemyCount;

    [Min(1)]
    public int EnemyHealth;

    [Min(0.1f)]
    public float SecondsBetweenSpawns;
}
