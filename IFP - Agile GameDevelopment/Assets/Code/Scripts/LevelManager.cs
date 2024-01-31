using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

public class LevelManager : MonoBehaviour
{
    public GameObject Enemy;

    public TileBase Grass;
    public TileBase Mountain;
    public TileBase Path;

    public GameObject BasicTower;
    public GameObject SniperTower;
    public GameObject IceTower;

    public GameObject UpgradedBasicTower;
    public GameObject UpgradedIceTower;
    public GameObject UpgradedSniperTower;

    public GameObject Blockade;

    public int Currency = 100;

    // TODO: Use english language
    public TextMesh Anzeige;
    public TowerOptionsBar TowerOptionsBar;
    public TowerUpgradeMenu TowerMenu;

    public HUD HUD;

    private GameObject selectedTower;

    // NOTE: Level specific data

    private GameObject levelInstance;

    private Dictionary<Vector2Int, GameObject> towers = new Dictionary<Vector2Int, GameObject>();

    private LevelInfo levelInfo;

    public Tilemap Tilemap;

    private Vector2[] path;

    // NOTE: Gameplay logic specific data

    private int bestTry;

    [SerializeField]
    private int playerLives;

    private GameObject enemyParent;

    private Vector2[] enemyPath;

    private EnemySpawner enemySpawner;

    public static Vector2Int GetTileKeyFromTilePosition(Vector3Int tilePosition)
    {
        Vector2Int result = new Vector2Int(tilePosition.x, tilePosition.y);

        return result;
    }

    /// <summary>
    /// Extracts the path from the provided level GameObject.
    /// </summary>
    /// <returns>Array of Positions. One for each waypoint.</returns>
    private static Vector2[] ExtractPathFromLevel(GameObject level)
    {
        Vector2[] result;

        Transform pathObject = level.transform.Find("Path");

        if (pathObject != null)
        {
            int waypointCount = pathObject.childCount;

            result = new Vector2[waypointCount];

            for (int childIndex = 0; childIndex < waypointCount; ++childIndex)
            {
                Transform waypoint = pathObject.GetChild(childIndex);

                result[childIndex] = waypoint.position;
            }
        }
        else
        {
            result = new Vector2[0];
        }

        return result;
    }

    /// <summary>
    /// Instantiates the provided Prefab level.
    /// </summary>
    private static GameObject InstantiateLevel(GameObject level)
    {
        Assert.IsNotNull(level);

        GameObject result = Instantiate(level);

        return result;
    }

    public void DecreasePlayerLives()
    {
        playerLives--;

        if (playerLives <= 0)
        {
            this.HUD.ShowGameOverScreen(playerLives, this.bestTry);
        }
    }

    public void PlaceTowerAtTile(GameObject towerPrefab, Vector3Int tilePosition)
    {
        if (!TilePositionHasTower(tilePosition) && Currency >= 30)
        {
            Vector3 instantiationPosition = tilePosition + towerPrefab.transform.position;

            GameObject towerObject = Instantiate(towerPrefab, instantiationPosition, Quaternion.identity);

            Vector2Int tileKey = GetTileKeyFromTilePosition(tilePosition);

            SpendCurrency(30);

            towers.Add(tileKey, towerObject);
        }
        else if (TilePositionHasTower(tilePosition))
        {
            Vector2Int tileKey = GetTileKeyFromTilePosition(tilePosition);
            if (towers.TryGetValue(tileKey, out GameObject existingTower))
            {
                Destroy(existingTower);

                towers.Remove(GetTileKeyFromTilePosition(tilePosition));
                Vector3 instantiationPosition = tilePosition + towerPrefab.transform.position;

                GameObject towerObject = Instantiate(towerPrefab, instantiationPosition, Quaternion.identity);

                SpendCurrency(30);

                towers.Add(tileKey, towerObject);
            }
        }
        else
        {
            Debug.Log("not enough money to purchase the item");
        }
    }

    public void SetLoadedLevel(GameObject loadedLevel)
    {
        levelInstance = loadedLevel;
    }

    public void HandleClickOnTileForTesting()
    {
        HandleClickOnTile();
    }

    public bool TilePositionHasTower(Vector3Int tilePosition)
    {
        Vector2Int tileKey = GetTileKeyFromTilePosition(tilePosition);

        bool result = towers.ContainsKey(tileKey);

        return result;
    }

    private static Vector3Int GetTilePositionFromScreenPosition(Camera camera, Tilemap tilemap, Vector2 screenPosition)
    {
        Vector3 mouseWorldPosition = camera.ScreenToWorldPoint(screenPosition);

        Vector3Int result = tilemap.WorldToCell(mouseWorldPosition);

        return result;
    }

    private void Awake()
    {
        GameObject loadedLevel;

        if (LevelSelection.LoadedLevel != null)
        {
            loadedLevel = InstantiateLevel(LevelSelection.LoadedLevel);
        }
        else
        {
            loadedLevel = InstantiateDefaultLevel();
        }

        LoadDataFromInstantiatedLevel(loadedLevel);
        Currency = 100;
        UpdateUI();

        BeginEnemySpawning();

        FocusCameraOnGameplayArea(Camera.main, levelInfo.GameplayArea);
    }

    private void Update()
    {
        FocusCameraOnGameplayArea(Camera.main, levelInfo.GameplayArea);

        if (Input.GetMouseButtonDown(0))
        {
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                if (!TowerOptionsBar.gameObject.activeSelf)
                {
                    HandleClickOnTile();
                }
                else
                {
                    HideTowerOptionsBar();
                }
            }
        }
    }

    private void FixedUpdate()
    {
        HandleEnemySpawning(); // also calls WinMessage
    }

    public void UpgradeTower()
    {
        TowerMenu.SetCurrentTower(selectedTower);
        SpendCurrency(20);
    }

    public void SellPlacedTower()
    {
        if (selectedTower != null)
        {
            TowerMenu.SellTower(selectedTower);
            Vector3Int towerTilePosition = Tilemap.WorldToCell(selectedTower.transform.position);
            Vector2Int tileKey = GetTileKeyFromTilePosition(towerTilePosition);

            if (towers.ContainsKey(tileKey))
            {
                towers.Remove(tileKey);
            }

            IncreaseCurrency(15);
            selectedTower = null;
        }
    }

    private void HandleClickOnTile()
    {
        Vector3Int tilePosition = GetTilePositionFromScreenPosition(Camera.main, this.Tilemap, Input.mousePosition);

        TileBase tile = Tilemap.GetTile(tilePosition);
        if (TilePositionHasTower(tilePosition))
        {
            Vector2Int tileKey = GetTileKeyFromTilePosition(tilePosition);
            selectedTower = towers[tileKey];
          
            Vector3 tileWorldPosition = Tilemap.GetCellCenterWorld(tilePosition);
            TowerMenu.ShowTowerTile(tilePosition, tileWorldPosition);
        }
        else if (tile == this.Grass)
        {
            Vector3 tileWorldPosition = Tilemap.GetCellCenterWorld(tilePosition);
            TowerOptionsBar.ShowForTile(tilePosition, tileWorldPosition);
        }

        if (!TilePositionHasTower(tilePosition))
        {
            Vector3 tileWorldPosition = Tilemap.GetCellCenterWorld(tilePosition);

            if (tile == this.Grass)
            {
                TowerOptionsBar.ShowForTile(tilePosition, tileWorldPosition);
            }
            else if (tile == this.Path)
            {
                PlaceTowerAtTile(this.Blockade, tilePosition);
            }
        }
        else
        {
            // TODO: Tile already has a tower
        }
    }

    private void HideTowerOptionsBar()
    {
        TowerOptionsBar.Hide();
    }

    private void BeginEnemySpawning()
    {
        this.enemySpawner = new EnemySpawner(levelInfo.Waves);

        this.enemyParent = new GameObject("Enemies");

        enemySpawner.BeginSpawning();
    }

    /// <summary>
    /// Updates spawner. Spawns Enemies when necessary and updates the wave count.
    /// </summary>
    private void HandleEnemySpawning()
    {
        bool spawn = enemySpawner.Tick(Time.fixedDeltaTime);

        if (spawn)
        {
            SpawnEnemy();
        }

        if (enemySpawner.WaitingForEndOfCurrentWave())
        {
            CheckForEndOfCurrentWave();
        }

        if (enemySpawner.State == EnemySpawner.SpawnerState.Done) // win case
        {
            this.HUD.ShowGameOverScreen(playerLives, bestTry);
        }
    }

    /// <summary>
    /// Spawns an enemy at the first waypoint with an offset.
    /// </summary>
    private void SpawnEnemy()
    {
        Vector3 spawnPosition = new Vector3(1000, 0, 0);

        Enemy enemy = Instantiate(Enemy, spawnPosition, Quaternion.identity, enemyParent.transform).GetComponent<Enemy>();

        float offset = enemySpawner.GetNextEnemySpawnPositionOffset();

        int health = enemySpawner.GetNextEnemyHealth();

        enemy.Initialize(this, enemyPath, offset, health);
    }

    /// <summary>
    /// Checks if the current wave has been defeated and triggers the next wave to spawn.
    /// </summary>
    private void CheckForEndOfCurrentWave()
    {
        if (enemyParent.transform.childCount == 0)
        {
            enemySpawner.CurrentWaveIsOver();

            ResetTowers();
        }
    }

    /// <summary>
    /// Resets towers between waves.
    /// </summary>
    private void ResetTowers()
    {
        foreach (var towerObject in towers.Values)
        {
            BaseTower tower = towerObject.GetComponent<BaseTower>();

            if (tower is Blockade)
            {
                Blockade blockade = (Blockade)tower;

                blockade.Reset();
            }
        }
    }

    /// <summary>
    /// Extract all relevant data from the instantiated level object.
    /// </summary>
    private void LoadDataFromInstantiatedLevel(GameObject level)
    {
        this.levelInstance = level;
        this.levelInfo = level.GetComponent<LevelInfo>();
        this.Tilemap = level.GetComponentInChildren<Tilemap>();
        this.enemyPath = ExtractPathFromLevel(level);
        this.playerLives = levelInfo.playerLives;
    }

    public void IncreaseCurrency(int amount)
    {
        Currency += amount;
        UpdateUI();
    }

    public bool SpendCurrency(int amount)
    {
        if (amount <= Currency)
        {
            Currency -= amount;
            UpdateUI();
            return true;
        }
        else
        {
            Debug.Log("Not enough money to purchase this item");
            return false;
        }
    }

    /// <summary>
    /// Load the first level by default. This is for starting from the GameScene in the editor.
    /// </summary>
    private static GameObject InstantiateDefaultLevel()
    {
        GameObject level = LevelSelection.LoadLevel(1);

        GameObject result = InstantiateLevel(level);

        return result;
    }

    /// <summary>
    /// Sets up the camera to center on and show the entire gameplayArea of level.
    /// </summary>
    private static void FocusCameraOnGameplayArea(Camera camera, Bounds gameplayArea)
    {
        Vector3 oldCameraPosition = camera.transform.position;
        Vector3 newCameraPosition;

        Vector3 gameplayAreaCenter = gameplayArea.center;

        if (float.IsFinite(gameplayAreaCenter.x) &&
            float.IsFinite(gameplayAreaCenter.x))
        {
            newCameraPosition.x = gameplayAreaCenter.x;
            newCameraPosition.y = gameplayAreaCenter.y;
            newCameraPosition.z = oldCameraPosition.z;

            float minCameraSizeH = gameplayArea.extents.y;
            float minCameraSizeV = gameplayArea.extents.x / camera.aspect;

            float minCameraSize = Mathf.Max(minCameraSizeH, minCameraSizeV);

            if (minCameraSize >= 0.1f && minCameraSize < 1000.0f)
            {
                camera.transform.position = newCameraPosition;
                camera.orthographicSize = minCameraSize;
            }
        }
    }

    public GameObject GetLevelInstance()
    {
        return levelInstance;
    }

    public void UpdateUI()
    {
        if (Anzeige != null)
        {
            Anzeige.text = "Currency: " + Currency;
        }
        else
        {
        }
    }

#if UNITY_EDITOR

#pragma warning disable SA1201

    public GameObject Test_Level
    {
        get
        {
            return this.levelInstance;
        }
    }

    public int Test_PlayerLives
    {
        get
        {
            return this.playerLives;
        }

        set
        {
            this.playerLives = value;
        }
    }

    public Dictionary<Vector2Int, GameObject> Test_Towers
    {
        get
        {
            return this.towers;
        }
    }
#endif
}