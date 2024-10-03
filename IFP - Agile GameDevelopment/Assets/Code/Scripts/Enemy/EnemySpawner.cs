/// <summary>
/// Holds all relevant data to handle enemy spawning at a single spawn point.
/// </summary>
public struct EnemySpawner
{
    private static readonly float SecondsBetweenWaves = 5.0f;
    private static readonly float SpawnOffsetRange = 0.3f;

    private SpawnerState state;

    private PRNG random;

    private Wave[] waves;
    
    private int currentWaveIndex;
    private int remainingEnemyCount;

    private float timer;

    public EnemySpawner(Wave[] waves)
    {
        this.waves = waves;
        this.state = SpawnerState.Initialized;
        this.timer = 0;
        this.currentWaveIndex = 0;
        this.remainingEnemyCount = 0;
        this.random = new PRNG(0);
    }

    public enum SpawnerState
    {
        Initialized,
        Spawning,
        BetweenWaves,
        Done,
    }

    public SpawnerState State 
    {  
        get { return state; } 
    }

    public int WaveIndex 
    { 
        get { return currentWaveIndex; } 
    }

    /// <summary>
    /// Advance this spawners time.
    /// </summary>
    /// <param name="timeStep">Time to advance this spawners time by.</param>
    /// <returns>True if the spawn should spawn an enemy.</returns>
    public bool Tick(float timeStep)
    {
        bool spawn = false;

        timer += timeStep;

        switch (state)
        {
            case SpawnerState.Initialized:
                break;
            case SpawnerState.Spawning:
                spawn = HandleEnemySpawning();
                break;
            case SpawnerState.BetweenWaves:
                HandleCooldownBetweenWaves();
                break;
            default:
                break;
        }

        return spawn;
    }

    /// <summary>
    /// Tells the spawner to start spawning enemies.
    /// </summary>
    public void BeginSpawning()
    {
        StartSpawningCurrentWave();
    }

    /// <summary>
    /// Wait until all enemies are killed to advance to the next wave.
    /// </summary>
    /// <returns>True if the end of current wave condition is met.</returns>
    public bool WaitingForEndOfCurrentWave()
    {
        bool result = false;

        if (state == SpawnerState.Spawning && remainingEnemyCount <= 0)
        {
            result = true;
        }

        return result;
    }

    /// <summary>
    /// Advance state to next wave.
    /// </summary>
    public void CurrentWaveIsOver()
    {
        LoadNextWave();
    }

    /// <summary>
    /// Get this enemies random perpendicular offset.
    /// </summary>
    /// <returns>The calculated offset.</returns>
    public float GetNextEnemySpawnPositionOffset()
    {
        float result = (float)random.NextRange(-SpawnOffsetRange, SpawnOffsetRange);

        return result;
    }

    /// <summary>
    /// Get the next enemy that should be spawned health.
    /// </summary>
    /// <returns>The next enemy health.</returns>
    public int GetNextEnemyHealth()
    {
        Wave wave = GetCurrentWave();

        int result = wave.EnemyHealth;

        return result;
    }

    private Wave GetCurrentWave()
    {
        Wave result = default;

        if (waves != null && currentWaveIndex < waves.Length)
        {
            result = waves[currentWaveIndex];
        }

        return result;
    }

    private bool HasCurrentWave()
    {
        bool result = false;

        if (waves != null && currentWaveIndex < waves.Length)
        {
            result = true;
        }

        return result;
    }

    private bool HandleEnemySpawning()
    {
        bool spawn = false;

        bool hasCurrentWave = HasCurrentWave();

        if (hasCurrentWave)
        {
            Wave wave = GetCurrentWave();

            if (remainingEnemyCount > 0 && timer > wave.SecondsBetweenSpawns)
            {
                spawn = true;

                timer -= wave.SecondsBetweenSpawns;

                --remainingEnemyCount;
            }
        }

        return spawn;
    }

    private void HandleCooldownBetweenWaves()
    {
        if (timer > SecondsBetweenWaves)
        {
            StartSpawningCurrentWave();
        }
    }

    private void LoadNextWave()
    {
        ++currentWaveIndex;

        timer = 0;
        state = SpawnerState.BetweenWaves;
    }

    private void StartSpawningCurrentWave()
    {
        timer = 0;

        bool hasCurrentWave = HasCurrentWave();

        if (hasCurrentWave)
        {
            Wave wave = GetCurrentWave();

            state = SpawnerState.Spawning;
            remainingEnemyCount = wave.EnemyCount;
        }
        else
        {
            state = SpawnerState.Done;
        }
    }
}
