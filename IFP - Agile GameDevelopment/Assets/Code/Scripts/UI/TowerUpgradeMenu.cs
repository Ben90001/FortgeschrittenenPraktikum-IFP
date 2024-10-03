using UnityEngine;

/// <summary>
/// Menu to upgrade and sell towers.
/// </summary>
public class TowerUpgradeMenu : MonoBehaviour
{
    [SerializeField]
    private LevelManager levelManager;

    [SerializeField]
    private GameObject upgradedBasicTower;

    [SerializeField]
    private GameObject upgradedIceTower;

    [SerializeField] 
    private GameObject upgradedSniperTower;

    private GameObject currentTower;
    private Vector3Int tilePosition;

    /// <summary>
    /// Show UI for selected tile. Also stores the selected tile for later.
    /// </summary>
    /// <param name="tilePosition">The tile position in grid.</param>
    /// <param name="tileWorldPosition">The tile position in world.</param>
    public void ShowTowerTile(Vector3Int tilePosition, Vector3 tileWorldPosition)
    {
        this.tilePosition = tilePosition;

        gameObject.transform.position = tileWorldPosition;
        gameObject.SetActive(true);
       
        Time.timeScale = 0.0f;
    }

    /// <summary>
    /// Upgrade the tower at selected tile.
    /// </summary>
    public void UpgradeTower()
    {
        if (currentTower != null)
        {
            GameObject upgradedTowerPrefab = GetUpgradedTowerPrefab(currentTower);
            if (upgradedTowerPrefab != null)
            {
                Destroy(currentTower);
                Instantiate(upgradedTowerPrefab, tilePosition, Quaternion.identity);
            }
        }

        Hide();
    }
   
    /// <summary>
    /// Checks if the tower is basic and can be upgraded.
    /// </summary>
    /// <param name="tower">The tower object to check.</param>
    /// <returns>True if the tower is basic</returns>
    public bool IsBasicTower(GameObject tower)
    {
        bool result = false;

        if (tower != null)
        {
            if (tower.CompareTag("BasicTower"))
            {
                result = true;
            }

            if (tower.CompareTag("SniperTower"))
            {
                result = true;
            }

            if (tower.CompareTag("IceTower"))
            {
                result = true;
            }
        }

        return result;
    }

    /// <summary>
    /// Selects the tower to be worked with in later calls.
    /// </summary>
    /// <param name="tower">The tower object to set.</param>
    public void SetCurrentTower(GameObject tower)
    {
        currentTower = tower;
        
        if (currentTower != null)
        {
            Debug.Log("Ausgewählter Turm: " + currentTower.name);
            
            if (currentTower.CompareTag("BasicTower"))
            {
                Debug.Log("Upgrading to UpgradedBasicTower");
                levelManager.PlaceTowerAtTile(upgradedBasicTower, this.tilePosition);
                gameObject.SetActive(false);
                Time.timeScale = 1f;
            }
            
            if (currentTower.CompareTag("SniperTower"))
            {
                Debug.Log("Upgrading to UpgradedSniperTower");
                levelManager.PlaceTowerAtTile(upgradedSniperTower, this.tilePosition);
                gameObject.SetActive(false);
                Time.timeScale = 1f;
            }
            
            if (currentTower.CompareTag("IceTower"))
            {
                Debug.Log("Upgrading to UpgradedIceTower");
                levelManager.PlaceTowerAtTile(upgradedIceTower, this.tilePosition);
                gameObject.SetActive(false);
                Time.timeScale = 1f;
            }
        }
        else
        {
            Debug.LogError("SetCurrentTower wurde mit einem null-GameObject aufgerufen.");
        }
    }

    /// <summary>
    /// Sells the selected tower.
    /// </summary>
    /// <param name="selectedtower">the tower to sell.</param>
    public void SellTower(GameObject selectedtower)
    {
        currentTower = selectedtower;
        if (currentTower != null)
        {
            Destroy(currentTower);
            
            Time.timeScale = 1f;
        }

        Hide();
    }

    /// <summary>
    /// Hides the current UI.
    /// </summary>
    public void Hide()
    {
        gameObject.SetActive(false);
    }

    private GameObject GetUpgradedTowerPrefab(GameObject tower)
    {
        if (tower.CompareTag("BasicTower"))
        {
            return upgradedBasicTower;
        }

        if (tower.CompareTag("IceTower")) 
        {
            return upgradedIceTower;
        }

        if (tower.CompareTag("SniperTower"))
        {
            return upgradedSniperTower;
        }

        return null;
    }
}
