using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Assertions;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;

/// <summary>
/// Static management of selected level to transfer information between scenes.
/// </summary>
public class LevelSelection : MonoBehaviour
{
    /// <summary>
    /// The last level that exists.
    /// </summary>
    public const int LastLevelID = 8;

    /// <summary>
    /// The currently loaded level.
    /// </summary>
    public static GameObject LoadedLevel;

    /// <summary>
    /// Generates the level key from its ID.
    /// </summary>
    /// <param name="levelID">The level ID to load.</param>
    /// <returns>The level key.</returns>
    public static string GetLevelKey(int levelID)
    {
        string result = "Assets/Prefabs/Levels/Level" + levelID.ToString() + ".prefab";

        return result;
    }

    /// <summary>
    /// Loads the addressable GameObject identified by key directly.
    /// </summary>
    /// <param name="key">The key to the GameObject</param>
    /// <returns>The loaded GameObject</returns>
    public static GameObject LoadGameObject(string key)
    {
        AsyncOperationHandle<GameObject> handle = Addressables.LoadAssetAsync<GameObject>(key);

        GameObject result = handle.WaitForCompletion();

        return result;
    }

    /// <summary>
    /// Loads the level identified by ID
    /// </summary>
    /// <param name="levelID">The level ID to load.</param>
    /// <returns>The loaded level GameObject</returns>
    public static GameObject LoadLevel(int levelID)
    {
        string levelKey = GetLevelKey(levelID);

        GameObject result = LoadGameObject(levelKey);

        return result;
    }

    /// <summary>
    /// Loads the level with ID and switches the level scene.
    /// </summary>
    /// <param name="levelID">The level ID to load.</param>
    public void LoadLevelAndSwitchScene(int levelID)
    {
        GameObject level = LoadLevel(levelID);

        if (level != null)
        {
            LoadedLevel = level;

            SceneManager.LoadScene("GameScene");
        }
        else
        {
            // TODO: Error: Failed to load the level.
            Debug.LogWarning("Failed to load level. level is Null.");
        }
    }

    /// <summary>
    /// Loads the next level.
    /// </summary>
    public void LoadNextLevel()
    {
        int oldLevelID = GetCurrentLevelID();

        if (oldLevelID < LastLevelID)
        {
            LoadLevelAndSwitchScene(oldLevelID + 1);
        }
        else
        {
            SwitchToMainMenu();
        }
    }

    /// <summary>
    /// Restarts the current level.
    /// </summary>
    public void RestartLevel()
    {
        int levelID = GetCurrentLevelID();

        LoadLevelAndSwitchScene(levelID);
    }

    private void SwitchToMainMenu()
    {
        SceneManager.LoadScene("MenuScene");
    }

    private int GetCurrentLevelID()
    {
        Assert.IsNotNull(LoadedLevel, "LoadedLevel is Null");

        string levelName = LoadedLevel.name;
        int levelID = int.Parse(levelName.Substring(5, 1));

        Debug.Log("Loaded Level has ID: " + levelID);

        return levelID;
    }
}
