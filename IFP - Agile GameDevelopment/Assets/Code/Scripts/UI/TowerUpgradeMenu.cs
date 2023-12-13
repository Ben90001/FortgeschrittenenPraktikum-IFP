using UnityEngine;
using UnityEngine.Tilemaps;

public class TowerUpgradeMenu : MonoBehaviour
{
    public GameObject UpgradedBasicTower;
    public GameObject UpgradedIceTower;
    public GameObject UpgradedSniperTower;
    public LevelManager levelManager;

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
                levelManager.PlaceTowerAtTile(UpgradedBasicTower, this.tilePosition);
                gameObject.SetActive(false);
                Time.timeScale = 1f;
            }
            if (currentTower.CompareTag("SniperTower"))
            {
                Debug.Log("Upgrading to UpgradedSniperrTower");
                levelManager.PlaceTowerAtTile(UpgradedSniperTower, this.tilePosition);
                gameObject.SetActive(false);
                Time.timeScale = 1f;
            }
            if (currentTower.CompareTag("IceTower"))
            {
                Debug.Log("Upgrading to UpgradedIceTower");
                levelManager.PlaceTowerAtTile(UpgradedIceTower, this.tilePosition);
                gameObject.SetActive(false);
                Time.timeScale = 1f;
            }
            
           
        }
        else
        {
            Debug.LogError("SetCurrentTower wurde mit einem null-GameObject aufgerufen.");
        }
    }

   


    private GameObject GetUpgradedTowerPrefab(GameObject tower)
    {
        if (tower.CompareTag("BasicTower")) return UpgradedBasicTower;
        if (tower.CompareTag("IceTower")) return UpgradedIceTower;
        if (tower.CompareTag("SniperTower")) return UpgradedSniperTower;
        return null;
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
}
