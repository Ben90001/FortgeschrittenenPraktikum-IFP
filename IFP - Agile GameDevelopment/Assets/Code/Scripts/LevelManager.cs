using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class LevelManager : MonoBehaviour
{
    // TODO: Support loading a level prefab based on selected level

    public GameObject Level;

    public GameObject Enemy;

    public float TimeBetweenSpawns = 10.0f;

    private LevelInfo levelInfo;
    
    private Transform[] path;

    private Transform spawnPoint;

    private float spawnTimer = 0.0f;

    public void Awake()
    {
        levelInfo = Level.GetComponent<LevelInfo>();

        LoadLevel(Level);

        FocusCameraOnGameplayArea(Camera.main, levelInfo.GameplayArea);

        path = ExtractPathFromLevel(Level);

        spawnPoint = path[0];
    }

    public void FixedUpdate()
    {
        // TODO: Move logic to EnemySpawner when implemented

        spawnTimer -= Time.deltaTime;

        if (spawnTimer < 0)
        {
            spawnTimer += TimeBetweenSpawns;
            
            GameObject enemyObject = Instantiate(Enemy, spawnPoint.position, Quaternion.identity);

            Enemy enemy = enemyObject.GetComponent<Enemy>();

            enemy.Path = path;
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

    private static void LoadLevel(GameObject level)
    {
        Assert.IsNotNull(level);

        Instantiate(level);
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
