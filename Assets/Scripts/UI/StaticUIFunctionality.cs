using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Common UI functionality exposed for game objects
/// </summary>
public class StaticUIFunctionality : MonoBehaviour
{
    /// <summary>
    /// Go to a scene via its build index
    /// </summary>
    /// <param name="buildIndex"></param>
    public static void GoToSceneByIndex(int buildIndex)
    {
        AkSoundEngine.StopAll();
        SceneManager.LoadScene(buildIndex);
    }

    /// <summary>
    /// Go to a scene via its name
    /// </summary>
    /// <param name="buildIndex"></param>
    public static void GoToSceneByName(string sceneName)
    {
        AkSoundEngine.StopAll();
        SceneManager.LoadScene(sceneName);
    }

    /// <summary>
    /// Quit out of the game (will also stop editor).
    /// </summary>
    public static void QuitGame()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }
}
