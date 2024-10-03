using TMPro;
using UnityEngine;

/// <summary>
/// Handles the in game HUD.
/// </summary>
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

    /// <summary>
    /// Update the HUD with new player information.
    /// </summary>
    /// <param name="currency">The player currency to display.</param>
    /// <param name="playerLives">The player lives to display.</param>
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
    
    /// <summary>
    /// Display the Game Over screen.
    /// </summary>
    /// <param name="playerLives">The current player lives.</param>
    /// <param name="bestTry">The players score.</param>
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

    /// <summary>
    /// Toggle the pause game menu.
    /// </summary>
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
