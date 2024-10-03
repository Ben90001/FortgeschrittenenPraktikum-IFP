using UnityEngine;

/// <summary>
/// UI element to buy towers.
/// </summary>
public class TowerOptionsBar : MonoBehaviour
{
    [SerializeField]
    private LevelManager levelManager;

    private Vector3Int tilePosition;

    /// <summary>
    /// Shows the UI for the selected tile. Also stores the selected tile position.
    /// </summary>
    /// <param name="tilePosition">Position of tile in tile grid.</param>
    /// <param name="tileWorldPosition">Position of tile in world.</param>
    public void ShowForTile(Vector3Int tilePosition, Vector3 tileWorldPosition)
    {
        this.tilePosition = tilePosition;

        gameObject.transform.position = tileWorldPosition;
        gameObject.SetActive(true);

        Time.timeScale = 0.0f;
    }

    /// <summary>
    /// Hides the UI.
    /// </summary>
    public void Hide()
    {
        gameObject.SetActive(false);

        Time.timeScale = 1.0f;
    }

    /// <summary>
    /// Places the at currently selected tile.
    /// </summary>
    /// <param name="towerPrefab"></param>
    public void PlaceTower(GameObject towerPrefab)
    {
        levelManager.PlaceTowerAtTile(towerPrefab, this.tilePosition);

        Hide();
    }
}
