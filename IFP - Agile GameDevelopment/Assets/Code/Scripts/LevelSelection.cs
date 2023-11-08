using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;

public class LevelSelection : MonoBehaviour
{
    public static GameObject LoadedLevel;

    public static string GetLevelKey(int levelID)
    {
        string result = "Assets/Prefabs/Levels/Level" + levelID.ToString() + ".prefab";

        return result;
    }

    public static GameObject LoadGameObject(string key)
    {
        AsyncOperationHandle<GameObject> handle = Addressables.LoadAssetAsync<GameObject>(key);

        GameObject result = handle.WaitForCompletion();

        return result;
    }

    public static GameObject LoadLevel(int levelID)
    {
        string levelKey = GetLevelKey(levelID);

        GameObject result = LoadGameObject(levelKey);

        return result;
    }

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
}
