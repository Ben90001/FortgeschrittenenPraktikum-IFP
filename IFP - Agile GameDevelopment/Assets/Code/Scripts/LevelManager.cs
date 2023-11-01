using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Tilemaps;
using UnityEngine.EventSystems;


public class LevelManager : MonoBehaviour
{
    public GameObject Enemy;

    public TileBase Grass;
    public TileBase Mountain;
    public TileBase Path;

    public GameObject BasicTower;
    public GameObject SniperTower;
    public GameObject IceTower;
    public int currency;
    public TextMesh Anzeige;
    public TowerOptionsBar TowerOptionsBar;



    public HUD HUD;

    // NOTE: Level specific data

    private GameObject levelInstance;

    private Dictionary<Vector2Int, GameObject> towers = new Dictionary<Vector2Int, GameObject>();

    private LevelInfo levelInfo;


    public Tilemap tilemap;

    private Vector2[] path;


    // NOTE: Gameplay logic specific data

    private float spawnTimer = 0.0f;

    public int PlayerLives = 10; //only public for game design changes during development


    private int bestTry;

    private int playerLives;


    // 

    private GameObject enemyParent;

    private Vector2[] enemyPath;

    private EnemySpawner enemySpawner;

    public static Vector2Int GetTileKeyFromTilePosition(Vector3Int tilePosition)
    {
        Vector2Int result = new Vector2Int(tilePosition.x, tilePosition.y);

        return result;

    }

    public void OnEnemyDestroyed()
    {

    }

    public void OnEmeyReachedEndOfPath()
    {

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
        if (!TilePositionHasTower(tilePosition) && currency >= 50)
        {
            Vector3 instantiationPosition = tilePosition + towerPrefab.transform.position;

            GameObject towerObject = Instantiate(towerPrefab, instantiationPosition, Quaternion.identity);

            Vector2Int tileKey = GetTileKeyFromTilePosition(tilePosition);

            SpendCurrency(50);

            towers.Add(tileKey, towerObject);


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
        currency = 100;
        UpdateUI();

        InitializeEnemySpawning();

        FocusCameraOnGameplayArea(Camera.main, levelInfo.GameplayArea);

        // TODO: Figure out where these should be initialized

        this.playerLives = levelInfo.playerLives;
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
        HandleEnemySpawning();
    }

    private void HandleClickOnTile()
    {
        Vector3Int tilePosition = GetTilePositionFromScreenPosition(Camera.main, this.tilemap, Input.mousePosition);

        TileBase tile = tilemap.GetTile(tilePosition);

        if (!TilePositionHasTower(tilePosition))
        {
            if (tile == this.Grass)
            {
                Vector3 tileWorldPosition = tilemap.GetCellCenterWorld(tilePosition);

                TowerOptionsBar.ShowForTile(tilePosition, tileWorldPosition);
            }
        }
        else
        {
            // TODO: Tile already has a tower
        }
    }

    private void ShowTowerOptionsBarForSelectedTile(Vector3Int tilePosition)
    {

    }

    private void HideTowerOptionsBar()
    {
        TowerOptionsBar.Hide();
    }

    private void InitializeEnemySpawning()
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
    }

    /// <summary>
    /// Spawns an enemy at the first waypoint with an offset.
    /// </summary>
    private void SpawnEnemy()
    {
        Enemy enemy = Instantiate(Enemy, enemyParent.transform).GetComponent<Enemy>();

        float offset = enemySpawner.GetNextEnemySpawnPositionOffset();

        enemy.Initialize(this, this.enemyPath, offset);
    }

    /// <summary>
    /// Checks if the current wave has been defeated and triggers the next wave to spawn.
    /// </summary>
    private void CheckForEndOfCurrentWave()
    {
        if (enemyParent.transform.childCount == 0)
        {
            enemySpawner.CurrentWaveIsOver();
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
    /// Instantiates the provided Prefab level
    /// </summary>
    private static GameObject InstantiateLevel(GameObject level)
    {
        Assert.IsNotNull(level);

        GameObject result = Instantiate(level);

        return result;
    }

    public void IncreaseCurrency(int amount)
    {
        currency += amount;
        UpdateUI();
    }

    public bool SpendCurrency(int amount)
    {
        if (amount <= currency)
        {
            currency -= amount;
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



    public void UpdateUI()
    {

        if (Anzeige != null)
        {

            Anzeige.text = "Currency: " + currency;
        }
        else
        {

        }
    }


#if UNITY_EDITOR

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


