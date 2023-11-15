public struct EnemySpawner
{
    private static readonly float SECONDS_BETWEEN_WAVES = 5.0f;
    private static readonly float SPAWN_OFFSET_RANGE = 0.3f;

    private SpawnerState state;

    private PRNG random;

    private Wave[] waves;
    
    private int currentWaveIndex;
    private int remainingEnemyCount;

    private float timer;

    public SpawnerState State 
    {  
        get { return state; } 
    }

    public int WaveIndex 
    { 
        get { return currentWaveIndex; } 
    }

    public EnemySpawner(Wave[] waves)
    {
        this.waves = waves;
        this.state = SpawnerState.Initialized;
        this.timer = 0;
        this.currentWaveIndex = 0;
        this.remainingEnemyCount = 0;
        this.random = new PRNG(0);
    }

    public bool Tick(float timeStep)
    {
        bool spawn = false;

        this.timer += timeStep;

        switch (this.state)
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

        if (this.state == SpawnerState.Spawning && this.remainingEnemyCount <= 0)
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
        float result = (float)random.NextRange(-SPAWN_OFFSET_RANGE, SPAWN_OFFSET_RANGE);

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

        if (this.waves != null && this.currentWaveIndex < this.waves.Length)
        {
            result = this.waves[this.currentWaveIndex];
        }

        return result;
    }

    private bool HasCurrentWave()
    {
        bool result = false;

        if (this.waves != null && this.currentWaveIndex < this.waves.Length)
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

            if (this.remainingEnemyCount > 0 && this.timer > wave.SecondsBetweenSpawns)
            {
                spawn = true;

                this.timer -= wave.SecondsBetweenSpawns;

                --this.remainingEnemyCount;
            }
        }

        return spawn;
    }

    private void HandleCooldownBetweenWaves()
    {
        if (this.timer > SECONDS_BETWEEN_WAVES)
        {
            StartSpawningCurrentWave();
        }
    }

    private void LoadNextWave()
    {
        ++this.currentWaveIndex;

        this.timer = 0;
        this.state = SpawnerState.BetweenWaves;
    }

    private void StartSpawningCurrentWave()
    {
        this.timer = 0;

        bool hasCurrentWave = HasCurrentWave();

        if (hasCurrentWave)
        {
            Wave wave = GetCurrentWave();

            this.state = SpawnerState.Spawning;
            this.remainingEnemyCount = wave.EnemyCount;
        }
        else
        {
            this.state = SpawnerState.Done;
        }
    }

    public enum SpawnerState
    {
        Initialized,
        Spawning,
        BetweenWaves,
        Done
    }

    public SpawnerState GetState()
    {
        return this.state;
    }
}
