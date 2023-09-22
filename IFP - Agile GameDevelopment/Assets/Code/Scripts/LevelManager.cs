using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class LevelManager : MonoBehaviour
{
    // TODO: Support loading a level prefab based on selected level

    public GameObject LoadedLevel;

    public void Awake()
    {
        LoadLevel(LoadedLevel);

        FocusCameraOnLevel(Camera.main, LoadedLevel);
    }

    private void LoadLevel(GameObject level)
    {
        Assert.IsNotNull(level);

        Instantiate(level);
    }

    private void FocusCameraOnLevel(Camera camera, GameObject level)
    {
        Assert.IsNotNull(camera);
        Assert.IsNotNull(level);

        LevelInfo levelInfo = level.GetComponent<LevelInfo>();

        Bounds gameplayArea = levelInfo.GameplayArea;

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
