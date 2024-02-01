using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUD : MonoBehaviour
{

    public GameObject DefeatScreen;
    public GameObject WinScreen;

    void Start()
    {
       //TODO: Initialize currency and PlayerLives display? 
    }


    void Update()
    {
        //TODO: Currency and PlayerLives display
    }

    public void ShowGameOverScreen(int playerLives, int bestTry)
    {
        Debug.Log("ShowGameOverScreen called");
        Time.timeScale = 0;
        if (playerLives <= 0)
        {
            if (DefeatScreen != null)
            {
                //TODO: update bestTry in PlayerInfo
                DefeatScreen.SetActive(true);
            }
            else
            {
                Debug.LogWarning("DefeatScreen is Null!");
            }
        }
        else
        {
            if(WinScreen != null)
            {
                WinScreen.SetActive(true);
            }
            else
            {
                Debug.LogWarning("WinScreen is Null!");
            }
        }
        
    }
}
