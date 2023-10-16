public struct EnemySpawner
{
    private Wave[] waves;

    private int currentWaveIndex;
    private int remainingEnemyCount;

    private float enemyTimer;

    public EnemySpawner(Wave[] waves)
    {
        this.waves = waves;

        this.currentWaveIndex = 0;
        this.remainingEnemyCount = 0;
        this.enemyTimer = 0;
    }

    public Wave GetCurrentWave()
    {
        Wave result = null

        if (this.waves != null && this.currentWaveIndex < this.waves.Length)
        {
            result = this.waves[this.currentWaveIndex];
        }

        return result;
    }

    public void Tick(float timeStep)
    {
        Wave wave = GetCurrentWave();

        if (wave != null)
        {
            this.enemyTimer += timeStep;

            if (this.enemyTimer > wave.SecondsBetweenSpawns)
            {
                this.enemyTimer -= wave.SecondsBetweenSpawns;

                // TODO: Spawn
            }
        }
    }
}
