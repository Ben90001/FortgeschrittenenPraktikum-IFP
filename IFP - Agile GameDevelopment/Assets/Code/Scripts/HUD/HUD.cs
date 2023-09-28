using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUD : MonoBehaviour
{

    public GameObject DefeatScreen;

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
        //TODO: update bestTry in PlayerInfo
        Time.timeScale(0);
        DefeatScreen.SetActive(true);
    }
}
