using TMPro;
using UnityEngine;

public class HUD : MonoBehaviour
{
    public GameObject DefeatScreen;
    public GameObject WinScreen;
    public GameObject PauseMenuScreen;

    public bool GameIsPaused = false;

    [SerializeField]
    private TextMeshProUGUI currencyUI;

    [SerializeField]
    private TextMeshProUGUI playerLivesUI;

    public void UpdateHUD(int currency, int playerLives)
    {
        if (currencyUI != null)
        {
            currencyUI.text = "" + currency;
        }
        else
        {
            Debug.LogWarning("Currency is null");
        }

        if (playerLivesUI != null)
        {
            playerLivesUI.text = "" + playerLives;
        }
        else
        {
            Debug.LogWarning("PlayerLives is null");
        }
    }

    public void ShowGameOverScreen(int playerLives, int bestTry)
    {
        Debug.Log("ShowGameOverScreen called");

        Time.timeScale = 0;

        if (playerLives <= 0)
        {
            if (DefeatScreen != null)
            {
                // TODO: Update bestTry in PlayerInfo

                DefeatScreen.SetActive(true);
            }
            else
            {
                Debug.LogWarning("DefeatScreen is Null!");
            }
        }
        else
        {
            if (WinScreen != null)
            {
                WinScreen.SetActive(true);
            }
            else
            {
                Debug.LogWarning("WinScreen is Null!");
            }
        }
    }

    public void TogglePauseMenu()
    {
        if (GameIsPaused)
        {
            ClosePauseMenu();

            GameIsPaused = false;
        }
        else
        {
            OpenPauseMenu();

            GameIsPaused = true;
        }
    }

    private void OpenPauseMenu()
    {
        Debug.Log("OpenPauseMenue called");

        Time.timeScale = 0;

        PauseMenuScreen.SetActive(true);
    }

    private void ClosePauseMenu()
    {
        Debug.Log("ClosePauseMenue called");

        Time.timeScale = 1;

        PauseMenuScreen.SetActive(false);
    }
}
