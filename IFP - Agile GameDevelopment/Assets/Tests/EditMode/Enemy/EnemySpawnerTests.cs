using NUnit.Framework;

public class EnemySpawnerTests
{
    private Wave[] waves;

    [SetUp]
    public void BeforeEveryTest()
    {
        waves = new Wave[2];

        waves[0].CurrencyWorth = 100;
        waves[0].EnemyCount = 1;
        waves[0].SecondsBetweenSpawns = 0.1f;
        waves[0].EnemyHealth = 10;

        waves[1].CurrencyWorth = 100;
        waves[1].EnemyCount = 1;
        waves[1].SecondsBetweenSpawns = 0.1f;
        waves[1].EnemyHealth = 10;
    }

    [Test]
    public void EnemySpawner_InitializedSpawnerDoesNothing()
    {
        EnemySpawner spawner = new EnemySpawner(waves);

        Assert.AreEqual(spawner.State, EnemySpawner.SpawnerState.Initialized);

        bool spawn = spawner.Tick(1.0f);

        Assert.AreEqual(spawner.State, EnemySpawner.SpawnerState.Initialized);

        Assert.IsFalse(spawn);
    }

    [Test]
    public void BeginSpawning_TriggeresFirstWave()
    {
        EnemySpawner spawner = new EnemySpawner(waves);

        spawner.BeginSpawning();

        Assert.AreEqual(spawner.State, EnemySpawner.SpawnerState.Spawning);

        bool spawn = spawner.Tick(1.0f);

        Assert.IsTrue(spawn);
    }

    [Test]
    public void WaitingForEndOfCurrentWave_ReturnsFalseIfWaveHasMoreEnemies()
    {
        EnemySpawner spawner = new EnemySpawner(waves);

        spawner.BeginSpawning();

        Assert.AreEqual(spawner.State, EnemySpawner.SpawnerState.Spawning);

        bool waitingForEndOfWave = spawner.WaitingForEndOfCurrentWave();

        Assert.IsFalse(waitingForEndOfWave);
    }

    [Test]
    public void WaitingForEndOfCurrentWave_ReturnsTrueWhenWaveIsOver()
    {
        EnemySpawner spawner = new EnemySpawner(waves);

        spawner.BeginSpawning();

        Assert.AreEqual(spawner.State, EnemySpawner.SpawnerState.Spawning);

        bool spawn = spawner.Tick(1.0f);

        Assert.IsTrue(spawn);

        bool waitingForEndOfWave = spawner.WaitingForEndOfCurrentWave();

        Assert.IsTrue(waitingForEndOfWave);
    }

    [Test]
    public void CurrentWaveIsOver_TriggersNextWaveWhenCalled()
    {
        EnemySpawner spawner = new EnemySpawner(waves);

        spawner.BeginSpawning();

        Assert.AreEqual(spawner.WaveIndex, 0);

        bool spawn = spawner.Tick(1.0f);

        Assert.AreEqual(spawner.WaveIndex, 0);

        spawner.CurrentWaveIsOver();

        Assert.AreEqual(spawner.WaveIndex, 1);
    }
}
