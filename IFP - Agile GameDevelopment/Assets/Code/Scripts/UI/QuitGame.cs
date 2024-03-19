using UnityEngine;

public class QuitGame : MonoBehaviour
{
    public void Exit()
    {
        Debug.Log("Quit Application");
        Application.Quit();
    }
}
