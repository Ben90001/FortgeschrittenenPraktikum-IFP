using UnityEngine;

public class TowerUpgradeMenu : MonoBehaviour
{
    public LevelManager levelManager;
    public GameObject upgradedTowerPrefab; // Prefab des upgegradeten Turms

    private GameObject currentTower; // Aktueller Turm, der upgegraded werden soll
    private Vector3Int tilePosition;

    public void ShowForTower(GameObject tower, Vector3Int tilePosition)
    {
        this.currentTower = tower;
        this.tilePosition = tilePosition;

        Vector3 towerWorldPosition = tower.transform.position;
        gameObject.transform.position = towerWorldPosition;
        gameObject.SetActive(true);

        Time.timeScale = 0.0f; // Pausiert das Spiel, w�hrend das Men� aktiv ist
    }

    public void Hide()
    {
        gameObject.SetActive(false);
        Time.timeScale = 1.0f; // Setzt das Spiel fort, wenn das Men� versteckt wird
    }

    public void UpgradeTower()
    {
        if (currentTower != null)
        {
            levelManager.PlaceTowerAtTile(upgradedTowerPrefab, tilePosition);
            Destroy(currentTower); // Zerst�rt den aktuellen Turm

            Hide();
        }
    }

    public void SellTower()
    {
        if (currentTower != null)
        {
            // Logik f�r den Verkauf des Turms
            // Zum Beispiel: Spielerw�hrung erh�hen, aktuellen Turm zerst�ren
            Destroy(currentTower);

            Hide();
        }
    }
}


