using UnityEngine;

/// <summary>
/// Script to handle quitting the game.
/// </summary>
public class QuitGame : MonoBehaviour
{
    /// <summary>
    /// Exit the game.
    /// </summary>
    public void Exit()
    {
        Debug.Log("Quit Application");
        Application.Quit();
    }
}
