using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;
using UnityEngine.Tilemaps;
using UnityEngine.EventSystems;

public class LevelManager : MonoBehaviour
{
    // TODO: Support loading a level prefab based on selected level

    public GameObject Level;

    public GameObject Enemy;

    // TODO: Move to BuildManager
    // TODO: Support handling tiles differently

    public TileBase Grass;
    public TileBase Mountain;
    public TileBase Path;

    // TODO: Handle tower references differently

    public GameObject Tower;

    public GameObject TowerOptionsBar;

    public float TimeBetweenSpawns = 1.0f;

    private GameObject loadedLevel;

    private LevelInfo levelInfo;
    
    private Transform[] path;

    private Transform spawnPoint;

    private float spawnTimer = 0.0f;

    private Tilemap tilemap;

    private Vector2Int tileKey;

    private TileBase selectedTile;

    private Vector3 clickPosition;

    private Dictionary<Vector2Int, GameObject> towers = new Dictionary<Vector2Int, GameObject>();

    public int PlayerLives = 10; //only public for game design changes during development

    private int bestTry;

    public HUD HUD;

    public void Awake()
    {
        loadedLevel = LoadLevel(Level);

        levelInfo = loadedLevel.GetComponent<LevelInfo>();

        FocusCameraOnGameplayArea(Camera.main, levelInfo.GameplayArea);

        path = ExtractPathFromLevel(loadedLevel);

        spawnPoint = path[0];

        tilemap = loadedLevel.GetComponentInChildren<Tilemap>();

    }

    public void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector3Int mouseTilePosition = tilemap.WorldToCell(mouseWorldPosition);

                selectedTile = tilemap.GetTile(mouseTilePosition);

                if (TowerOptionsBar.activeSelf)
                {
                    Time.timeScale = 1;
                    TowerOptionsBar.SetActive(false);
                }

                if (selectedTile == Grass)
                {
                    Vector2Int tileKey = new Vector2Int(mouseTilePosition.x, mouseTilePosition.y);

                    GameObject tower = towers.GetValueOrDefault(tileKey);

                    if (tower == null)
                    {
                        clickPosition = new Vector3(mouseTilePosition.x + 0.5f, mouseTilePosition.y + 0.5f, 0);

                        TowerOptionsBar.SetActive(true);
                        Time.timeScale = 0;
                    }
                    else
                    {
                        // Wenn ein Turm bereits auf dem Tile steht, k�nnten Sie hier andere Aktionen ausf�hren.
                    }
                }
            }
        }
    }

    public void PlaceTowerAndResetOptionsBar()
    {
        if (selectedTile == null)
        {
            Debug.LogWarning("Kein Tile ausgew�hlt, Turm kann nicht platziert werden.");
            return;
        }

        if (selectedTile == Grass)
        {
            GameObject towerObject = Instantiate(Tower, clickPosition, Quaternion.identity);
            towers.Add(new Vector2Int((int)clickPosition.x, (int)clickPosition.y), towerObject);
        }

        if (TowerOptionsBar.activeSelf)
        {
            TowerOptionsBar.SetActive(false);
        }

        Time.timeScale = 1;
    }

    public void FixedUpdate()
    {
        // TODO: Move logic to EnemySpawner when implemented

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
        PlayerLives --;

        if (PlayerLives <= 0)
        {
            this.HUD.ShowGameOverScreen(this.PlayerLives,this.bestTry);
        }
    }

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
        // TODO: This might need to be called every frame in case we want to support changing aspect ratio during gameplay.

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
