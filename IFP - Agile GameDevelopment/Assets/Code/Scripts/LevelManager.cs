using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;
using UnityEngine.Assertions;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.Tilemaps;
using UnityEngine.EventSystems;
using static PlasticPipe.PlasticProtocol.Messages.Serialization.ItemHandlerMessagesSerialization;
using UnityEditor.Experimental.GraphView;
using UnityEngine.WSA;

public class LevelManager : MonoBehaviour
{
    public GameObject Enemy;

    public TileBase Grass;
    public TileBase Mountain;
    public TileBase Path;

    public GameObject BasicTower;
    public GameObject SniperTower;
    public GameObject IceTower;

    public TowerOptionsBar TowerOptionsBar;

    public float TimeBetweenSpawns = 1.0f;

    public HUD HUD;

    // NOTE: Level specific private data

    [SerializeField] private GameObject level;
    [SerializeField] private Dictionary<Vector2Int, GameObject> towers = new Dictionary<Vector2Int, GameObject>();

    private LevelInfo levelInfo;

    public Tilemap tilemap;

    private Transform[] path;

    private Transform spawnPoint;

    // NOTE: Gameplay logic specific data

    private float spawnTimer = 0.0f;

    public int PlayerLives = 10; //only public for game design changes during development

    private int bestTry;

    public void Awake()
    {
        GameObject loadedLevel = null;

        if (LevelSelection.LoadedLevel != null)
        {
            loadedLevel = LoadLevel(LevelSelection.LoadedLevel);
        }
        else
        {
            loadedLevel = LoadDefaultLevel();
        }

        InitializeLoadedLevel(loadedLevel);
    }

    public void Update()
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

    public void FixedUpdate()
    {
        spawnTimer -= Time.fixedDeltaTime;

        if (spawnTimer < 0)
        {
            spawnTimer += TimeBetweenSpawns;
            
            GameObject enemyObject = Instantiate(Enemy, spawnPoint.position, Quaternion.identity);

            Enemy enemy = enemyObject.GetComponent<Enemy>();

            enemy.Initialize(this, path);
        }
    }

    public void DecreasePlayerLives()
    {
        PlayerLives--;

        if (PlayerLives <= 0)
        {
            this.HUD.ShowGameOverScreen(this.PlayerLives, this.bestTry);
        }
    }

    public void PlaceTowerAtTile(GameObject towerPrefab, Vector3Int tilePosition)
    {
        if (!TilePositionHasTower(tilePosition))
        {
            Vector3 instantiationPosition = tilePosition + towerPrefab.transform.position;

            GameObject towerObject = Instantiate(towerPrefab, instantiationPosition, Quaternion.identity);

            Vector2Int tileKey = GetTileKeyFromTilePosition(tilePosition);

            towers.Add(tileKey, towerObject);
        }
        else
        {
            // TODO: What todo
        }
    }

    public GameObject GetLevel() => level;
  
    public void SetLoadedLevel(GameObject loadedLevel)
    {
        level = loadedLevel;
    }

    public Dictionary<Vector2Int, GameObject> GetTowers()
    {
        return towers;
    }
    public void HandleClickOnTileForTesting()
    {
        HandleClickOnTile();
    }

    /// <summary>
    /// Extract all relevant data from the instantiated level object.
    /// </summary>
    private void InitializeLoadedLevel(GameObject level)
    {
        this.level = level;
        this.levelInfo = level.GetComponent<LevelInfo>();
        this.tilemap = level.GetComponentInChildren<Tilemap>();
        this.path = ExtractPathFromLevel(level);
        this.spawnPoint = path[0];

        FocusCameraOnGameplayArea(Camera.main, levelInfo.GameplayArea);
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

    private bool TilePositionHasTower(Vector3Int tilePosition)
    {
        Vector2Int tileKey = GetTileKeyFromTilePosition(tilePosition);

        bool result = towers.ContainsKey(tileKey);

        return result;
    }

    private void ShowTowerOptionsBarForSelectedTile(Vector3Int tilePosition)
    {
        
    }

    private void HideTowerOptionsBar()
    {
        TowerOptionsBar.Hide();
    }

    private static Vector3Int GetTilePositionFromScreenPosition(Camera camera, Tilemap tilemap, Vector2 screenPosition)
    {
        Vector3 mouseWorldPosition = camera.ScreenToWorldPoint(screenPosition);
        Vector3Int result = tilemap.WorldToCell(mouseWorldPosition);

        return result;
    }

    private static Vector2Int GetTileKeyFromTilePosition(Vector3Int tilePosition)
    {
        Vector2Int result = ((Vector2Int)tilePosition);

        return result;
    }

    /// <summary>
    /// Extracts the path from the provided level GameObject.
    /// </summary>
    /// <returns>Array of transforms. One transform for each waypoint.</returns>
    private static Transform[] ExtractPathFromLevel(GameObject level)
    {
        Transform[] result = null;

        Transform pathObject = level.transform.Find("Path");

        if (pathObject != null) 
        {
            int waypointCount = pathObject.childCount;

            result = new Transform[waypointCount];

            for (int childIndex = 0; childIndex < waypointCount; ++childIndex)
            {
                Transform waypoint = pathObject.GetChild(childIndex);

                result[childIndex] = waypoint;
            }
        }

        return result;
    }

    /// <summary>
    /// Load the first level by default. This is for starting from the GameScene in the editor.
    /// </summary>
    /// <returns></returns>
    private static GameObject LoadDefaultLevel()
    {
        GameObject level = LevelSelection.LoadLevel(1);

        GameObject result = LoadLevel(level);

        return result;
    }

    private static GameObject LoadLevel(GameObject level)
    {
        Assert.IsNotNull(level);

        GameObject result = Instantiate(level);

        return result;
    }

    /// <summary>
    /// Sets up the camera to center on and show the entire gameplayArea of level.
    /// </summary>
    private static void FocusCameraOnGameplayArea(Camera camera, Bounds gameplayArea)
    {
        Assert.IsNotNull(camera);

        Vector3 oldCameraPosition = camera.transform.position;
        Vector3 newCameraPosition = default(Vector3);

        Vector3 gameplayAreaCenter = gameplayArea.center;

        newCameraPosition.x = gameplayAreaCenter.x;
        newCameraPosition.y = gameplayAreaCenter.y;
        newCameraPosition.z = oldCameraPosition.z;

        float minCameraSizeH = gameplayArea.extents.y;
        float minCameraSizeV = gameplayArea.extents.x / camera.aspect;

        float minCameraSize = Mathf.Max(minCameraSizeH, minCameraSizeV);

        camera.transform.position = newCameraPosition;
        camera.orthographicSize = minCameraSize;
    }
}
