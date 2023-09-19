using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Holds all relevant data to specify the behavior of a single wave.
/// </summary>
[System.Serializable]
public class Wave
{
    public int CurrencyWorth;

    public SubWave[] SubWaves;
}
