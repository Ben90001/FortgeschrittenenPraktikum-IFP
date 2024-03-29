using UnityEngine;

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

    public void ShowTowerTile(Vector3Int tilePosition, Vector3 tileWorldPosition)
    {
        this.tilePosition = tilePosition;

        gameObject.transform.position = tileWorldPosition;
        gameObject.SetActive(true);
       
        Time.timeScale = 0.0f;
    }

    public void UpgradeTower()
    {
        if (currentTower != null)
        {
            GameObject upgradedTowerPrefab = GetUpgradedTowerPrefab(currentTower);
            if (upgradedTowerPrefab != null)
            {
                Destroy(currentTower);
                Instantiate(upgradedTowerPrefab, tilePosition, Quaternion.identity);
                Hide();
            }
        }
    }
   
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

    public void SellTower(GameObject selectedtower)
    {
        currentTower = selectedtower;
        if (currentTower != null)
        {
            Destroy(currentTower);
            Hide();
            Time.timeScale = 1f;
        }
    }

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
