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
    public void PlaceTower(GameObject towerPrefab)
    {
        
        levelManager.PlaceTowerAtTile(towerPrefab, this.tilePosition);
        gameObject.SetActive(false);
        Time.timeScale = 1f;

    }

    private GameObject GetUpgradedTowerPrefab(GameObject tower)
    {
        // Bestimmen Sie hier, welches Upgrade-Prefab verwendet werden soll
        // Basierend auf dem Typ des aktuellen Turms
        // Zum Beispiel:
        if (tower.CompareTag("BasicTower")) return UpgradedBasicTower;
        if (tower.CompareTag("IceTower")) return UpgradedIceTower;
        if (tower.CompareTag("SniperTower")) return UpgradedSniperTower;
        return null;
    }

    public void SellTower()
    {
        if (currentTower != null)
        {
            // F�gen Sie hier Logik hinzu, um die Spielerw�hrung zu erh�hen
            Destroy(currentTower);
            Hide();
        }
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
