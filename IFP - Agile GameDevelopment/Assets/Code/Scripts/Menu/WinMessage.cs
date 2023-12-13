using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class WinMessage : MonoBehaviour
{
    public GameObject LevelManager;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("WinMessage Started");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NextLevel()
    {
        GameObject OldLevelInstance = LevelManager.GetComponent<LevelManager>().GetLevelInstance();
        Assert.IsNotNull(OldLevelInstance, "WinMessage.NextLevel: OldLevelInstance Null");
        string OldLevelKeyString = OldLevelInstance.name.Substring(5, 2);
        Debug.Log("Trying to load next level with OldLevelKey: " + OldLevelKeyString);
        int OldLevelKey = int.Parse(OldLevelKeyString);

        //load next level, if 
        if(true)//(OldLevelKey <= 8) 
        {
            LevelManager.GetComponent<LevelSelection>().LoadLevelAndSwitchScene(OldLevelKey+1);
        }
        else //show win message: "you won the game, congratulations"?
        {
            LevelManager.GetComponent<LevelSelection>().LoadLevelAndSwitchScene(1);
        }
        
    }
}
