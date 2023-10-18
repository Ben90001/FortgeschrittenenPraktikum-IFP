using UnityEngine;

public struct EnemySpawner
{
    private static readonly float SECONDS_BETWEEN_WAVES = 5.0f;
    private static readonly float SPAWN_OFFSET_RANGE = 0.3f;

    private LevelManager levelManager;

    private GameObject enemyParent;

    private GameObject enemyPrefab;

    private Vector2[] enemyPath;

    private Wave[] waves;

    private PRNG offsetRandom;

    private State state;

    private int currentWaveIndex;
    private int remainingEnemyCount;

    private float timer;

    public EnemySpawner(LevelManager levelManager, Wave[] waves, Vector2[] enemyPath, GameObject enemyPrefab)
    {
        this.levelManager = levelManager;
        this.waves = waves;
        this.enemyPrefab = enemyPrefab;
        this.enemyPath = enemyPath;

        this.offsetRandom = new PRNG(0);

        this.timer = 0;
        this.currentWaveIndex = 0;
        this.remainingEnemyCount = 0;
        this.state = State.Initialized;

        this.enemyParent = new GameObject("Enemies");

        StartSpawningCurrentWave();
    }

    public void Tick(float timeStep)
    {
        this.timer += timeStep;

        switch (this.state)
        {
            case State.Initialized:
                break;
            case State.Spawning:
                HandleEnemySpawning();
                HandleEndOfWaveLogic();
                break;
            case State.BetweenWaves:
                HandleCooldownBetweenWaves();
                break;
            default:
                break;
        }
    }

    private Wave GetCurrentWave()
    {
        Wave result = null;

        if (this.waves != null && this.currentWaveIndex < this.waves.Length)
        {
            result = this.waves[this.currentWaveIndex];
        }

        return result;
    }

    private void HandleEnemySpawning()
    {
        Wave wave = GetCurrentWave();

        if (wave != null)
        {
            if (this.remainingEnemyCount > 0 && this.timer > wave.SecondsBetweenSpawns)
            {
                SpawnEnemy();

                this.timer -= wave.SecondsBetweenSpawns;

                --this.remainingEnemyCount;
            }
        }
    }

    private void HandleEndOfWaveLogic()
    {
        if (this.remainingEnemyCount <= 0 && 
            this.enemyParent.transform.childCount == 0)
        {
            LoadNextWave();
        }
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
        this.state = State.BetweenWaves;
    }

    private void StartSpawningCurrentWave()
    {
        this.timer = 0;

        Wave wave = GetCurrentWave();

        if (wave != null)
        {
            this.state = State.Spawning;
            this.remainingEnemyCount = wave.EnemyCount;
        }
        else
        {
            this.state = State.Done;
        }
    }

    private void SpawnEnemy()
    {
        Enemy enemy = Object.Instantiate(enemyPrefab, enemyParent.transform).GetComponent<Enemy>();

        float offset = GetEnemySpawnOffset();

        enemy.Initialize(this.levelManager, this.enemyPath, offset);
    }

    private float GetEnemySpawnOffset()
    {
        float result = (float)offsetRandom.NextRange(-SPAWN_OFFSET_RANGE, SPAWN_OFFSET_RANGE);

        return result;
    }

    public enum State
    {
        Initialized,
        Spawning,
        BetweenWaves,
        Done
    }
}
