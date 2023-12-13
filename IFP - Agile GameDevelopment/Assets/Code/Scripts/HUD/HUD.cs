using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HUD : MonoBehaviour
{

    public GameObject DefeatScreen;
    public GameObject WinScreen;
    [SerializeField] private TextMeshPro ScoreText;

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
        Debug.Log("ShowGameOverScreen called: "+ playerLives + " playerLives remaining");
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
                GameObject ScoreTextObject =  WinScreen.transform.GetChild(0).Find("ScoreText").gameObject;
                ScoreTextObject.GetComponent<TextMeshPro>();
                //TODO: finish this
                WinScreen.SetActive(true);
            }
            else
            {
                Debug.LogWarning("WinScreen is Null!");
            }
        }
        
    }
}
