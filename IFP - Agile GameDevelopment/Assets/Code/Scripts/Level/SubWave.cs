using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Used to specify the spawning behavior of a single Wave. 
/// A Wave can have multiple SubWaves. 
/// Each SubWave specifies spawns for a single type of enemy.
/// </summary>
[System.Serializable]
public class SubWave
{
    // TODO: Add enemy identifier to specify which enemy should be spawned

    public float StartTime;

    public float SpawnInterval;

    public int EnemiesToSpawn;
}
