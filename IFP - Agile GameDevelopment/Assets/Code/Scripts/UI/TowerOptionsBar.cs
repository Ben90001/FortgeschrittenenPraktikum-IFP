using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerOptionsBar : MonoBehaviour
{
    public LevelManager levelManager;

    public Vector3Int tilePosition;

    public void ShowForTile(Vector3Int tilePosition, Vector3 tileWorldPosition)
    {
        this.tilePosition = tilePosition;

        gameObject.transform.position = tileWorldPosition;
        gameObject.SetActive(true);

        Time.timeScale = 0.0f;
    }

    public void Hide()
    {
        gameObject.SetActive(false);

        Time.timeScale = 1.0f;
    }

    public void PlaceTower(GameObject towerPrefab)
    {
        levelManager.PlaceTowerAtTile(towerPrefab, this.tilePosition);

        Hide();
    }
}
