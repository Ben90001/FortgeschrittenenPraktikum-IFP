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

    public void BeginSpawning()
    {
        StartSpawningCurrentWave();
    }

    public bool WaitingForEndOfCurrentWave()
    {
        bool result = false;

        if (state == SpawnerState.Spawning && remainingEnemyCount <= 0)
        {
            result = true;
        }

        return result;
    }

    public void CurrentWaveIsOver()
    {
        LoadNextWave();
    }

    public float GetNextEnemySpawnPositionOffset()
    {
        float result = (float)random.NextRange(-SpawnOffsetRange, SpawnOffsetRange);

        return result;
    }

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
