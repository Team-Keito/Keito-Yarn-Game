using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Common UI functionality exposed for game objects
/// </summary>
public class StaticUIFunctionality : MonoBehaviour
{
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
