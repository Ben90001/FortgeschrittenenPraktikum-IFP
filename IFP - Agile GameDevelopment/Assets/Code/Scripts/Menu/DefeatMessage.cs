using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class DefeatMessage : MonoBehaviour
{
    public GameObject LevelManager;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("DefeatMessage Started");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RestartLevel()
    {
        GameObject LevelInstance = LevelManager.GetComponent<LevelManager>().GetLevelInstance();
        Assert.IsNotNull(LevelInstance, "DefeatMessage.RestartLevel: LevelInstance Null");
        string LevelKeyString = LevelInstance.name.Substring(5,2);
        Debug.Log("Trying to restart with LevelKey: " + LevelKeyString);
        int LevelKey = int.Parse(LevelKeyString);
        LevelManager.GetComponent<LevelSelection>().LoadLevelAndSwitchScene(LevelKey);
    }
}
