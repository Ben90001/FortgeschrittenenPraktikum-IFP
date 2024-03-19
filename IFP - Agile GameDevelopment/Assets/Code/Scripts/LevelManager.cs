using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

public class LevelManager : MonoBehaviour
{
    [SerializeField]
    private GameObject enemy;

    [SerializeField]
    private TileBase grass;

    [SerializeField]
    private TileBase mountain;

    [SerializeField]
    private TileBase path;

    [SerializeField]
    private GameObject basicTower;

    [SerializeField]
    private GameObject sniperTower;

    [SerializeField]
    private GameObject iceTower;

    [SerializeField]
    private TowerOptionsBar towerOptionsBar;

    [SerializeField]
    private TowerUpgradeMenu towerMenu;

    [SerializeField]
    private HUD hud;

    private Dictionary<Vector2Int, GameObject> towers = new Dictionary<Vector2Int, GameObject>();

    private GameObject selectedTower;

    private GameObject levelInstance;

    private LevelInfo levelInfo;

    private Tilemap tilemap;

    private Vector2[] enemyPath;

    private EnemySpawner enemySpawner;

    private int currency = 100;

    private int playerLives;

    private GameObject enemyParent;

    private int bestTry;

    /// <summary>
    /// Returns a unique key generated for the tiles position.
    /// </summary>
    /// <param name="tilePosition">The tile position.</param>
    /// <returns>The unique key.</returns>
    public static Vector2Int GetTileKeyFromTilePosition(Vector3Int tilePosition)
    {
        Vector2Int result = new Vector2Int(tilePosition.x, tilePosition.y);

        return result;
    }

    public void SetLoadedLevel(GameObject loadedLevel)
    {
        levelInstance = loadedLevel;
    }

    /// <summary>
    /// Decrease the player lives by one.
    /// </summary>
    public void DecreasePlayerLives()
    {
        playerLives--;

        if (playerLives <= 0)
        {
            this.hud.ShowGameOverScreen(playerLives, this.bestTry);
        }
    }

    /// <summary>
    /// Places tower at the given tile position. 
    /// </summary>
    /// <param name="towerPrefab">The tower GameObject to place.</param>
    /// <param name="tilePosition">The tile position to place the tower at.</param>
    public void PlaceTowerAtTile(GameObject towerPrefab, Vector3Int tilePosition)
    {
        if (!TilePositionHasTower(tilePosition) && currency >= 30)
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

    public void UpgradeTower()
    {
        towerMenu.SetCurrentTower(selectedTower);
        SpendCurrency(20);
    }

    public void SellPlacedTower()
    {
        if (selectedTower != null)
        {
            towerMenu.SellTower(selectedTower);
            Vector3Int towerTilePosition = tilemap.WorldToCell(selectedTower.transform.position);
            Vector2Int tileKey = GetTileKeyFromTilePosition(towerTilePosition);

            if (towers.ContainsKey(tileKey))
            {
                towers.Remove(tileKey);
            }

            IncreaseCurrency(15);
            selectedTower = null;
        }
    }

    public void IncreaseCurrency(int amount)
    {
        currency += amount;
    }

    public bool SpendCurrency(int amount)
    {
        if (amount <= currency)
        {
            currency -= amount;
            return true;
        }
        else
        {
            Debug.Log("Not enough money to purchase this item");
            return false;
        }
    }

    public GameObject GetLevelInstance()
    {
        return levelInstance;
    }

    private static Vector3Int GetTilePositionFromScreenPosition(Camera camera, Tilemap tilemap, Vector2 screenPosition)
    {
        Vector3 mouseWorldPosition = camera.ScreenToWorldPoint(screenPosition);

        Vector3Int result = tilemap.WorldToCell(mouseWorldPosition);

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
        currency = 100;
        hud.UpdateHUD(currency, playerLives);
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
                if (towerMenu.gameObject.activeSelf)
                {
                    HideTowerMenu();
                }
                else if (!towerOptionsBar.gameObject.activeSelf)
                {
                    HandleClickOnTile();
                }
                else
                {
                    HideTowerOptionsBar();
                }
            }
        }

        hud.UpdateHUD(currency, playerLives);
    }

    private void FixedUpdate()
    {
        HandleEnemySpawning(); // also calls WinMessage
    }

    private bool TilePositionHasTower(Vector3Int tilePosition)
    {
        Vector2Int tileKey = GetTileKeyFromTilePosition(tilePosition);

        bool result = towers.ContainsKey(tileKey);

        return result;
    }

    private GameObject GetTowerForTilePosition(Vector3Int tilePosition)
    {
        GameObject result = null;

        Vector2Int tileKey = GetTileKeyFromTilePosition(tilePosition);

        if (towers.ContainsKey(tileKey))
        {
            result = towers[tileKey];
        }

        return result;
    }

    private void HideTowerMenu()
    {
        //TODO: paused game until first tower is placed
        towerMenu.Hide();
        if (!hud.GameIsPaused)
        {
            Time.timeScale = 1;
        }
        
    }

    private void HandleClickOnTile()
    {
        Vector3Int tilePosition = GetTilePositionFromScreenPosition(Camera.main, this.tilemap, Input.mousePosition);

        GameObject tower = GetTowerForTilePosition(tilePosition);

        if (tower != null) 
        {
            selectedTower = tower;

            Vector3 tileWorldPosition = tilemap.GetCellCenterWorld(tilePosition);

            towerMenu.ShowTowerTile(tilePosition, tileWorldPosition);
        }
        else
        {
            TileBase tile = tilemap.GetTile(tilePosition);

            if (tile == this.grass)
            {
                Vector3 tileWorldPosition = tilemap.GetCellCenterWorld(tilePosition);

                towerOptionsBar.ShowForTile(tilePosition, tileWorldPosition);
            }
        }
    }

    private void HideTowerOptionsBar()
    {
        towerOptionsBar.Hide();
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
            this.hud.ShowGameOverScreen(playerLives, bestTry);
        }
    }

    /// <summary>
    /// Spawns an enemy at the first waypoint with an offset.
    /// </summary>
    private void SpawnEnemy()
    {
        Vector3 spawnPosition = new Vector3(1000, 0, 0);

        Enemy enemyInstance = Instantiate(enemy, spawnPosition, Quaternion.identity, enemyParent.transform).GetComponent<Enemy>();

        float offset = enemySpawner.GetNextEnemySpawnPositionOffset();

        int health = enemySpawner.GetNextEnemyHealth();

        enemyInstance.Initialize(this, enemyPath, offset, health);
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

            // TODO: Remove not needed
        }
    }

    /// <summary>
    /// Extract all relevant data from the instantiated level object.
    /// </summary>
    private void LoadDataFromInstantiatedLevel(GameObject level)
    {
        this.levelInstance = level;
        this.levelInfo = level.GetComponent<LevelInfo>();
        this.tilemap = level.GetComponentInChildren<Tilemap>();
        this.enemyPath = ExtractPathFromLevel(level);
        this.playerLives = levelInfo.PlayerLives;
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

    public int Test_Currency
    {
        get
        {
            return currency;
        }
    }

    public Dictionary<Vector2Int, GameObject> Test_Towers
    {
        get
        {
            return this.towers;
        }
    }

    public bool Test_TilePositionHasTower(Vector3Int tilePosition)
    {
        return TilePositionHasTower(tilePosition);
    }
#endif
}