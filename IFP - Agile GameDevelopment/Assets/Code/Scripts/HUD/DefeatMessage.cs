using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DefeatMessage : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("DefeatMessage Started");
    }



    // Methode für den "Quit"-Button    
    public void QuitGame()
    {
        // Beende das Spiel (nur im Build-Modus)
        Application.Quit();
        SceneManager.LoadScene("MenuScene");
    }
    public void TryAgain()
    {

        string currentSceneName = SceneManager.GetActiveScene().name;


        SceneManager.LoadScene(currentSceneName);


    }
}
