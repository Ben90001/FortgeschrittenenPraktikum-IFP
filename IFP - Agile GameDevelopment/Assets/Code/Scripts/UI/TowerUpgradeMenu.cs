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
        // Speichern des �bergebenen Turm-GameObjects in der privaten Variable
        currentTower = tower;

        // �berpr�fen, ob das �bergebene Turm-GameObject g�ltig ist
        if (currentTower != null)
        {
            Debug.Log("Ausgew�hlter Turm: " + currentTower.name);
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
            // Hier k�nnen Sie Aktionen durchf�hren, die notwendig sind, wenn ein Turm ausgew�hlt wird
            // Zum Beispiel das Anzeigen eines Upgrade-Men�s oder das Aktivieren von spezifischen UI-Elementen
            ShowTowerOptions();
        }
        else
        {
            Debug.LogError("SetCurrentTower wurde mit einem null-GameObject aufgerufen.");
        }
    }

    private void ShowTowerOptions()
    {
        // Logik zum Anzeigen von Turm-Optionen, z. B. Upgrade-Optionen
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
