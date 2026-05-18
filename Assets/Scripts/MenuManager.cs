using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

#if UNITY_EDITOR
using UnityEditor;     // SceneAsset is Editor-only
#endif

public class MenuManager : MonoBehaviour
{
    // ---------------------------------------------------------
    // UI PANELS
    // ---------------------------------------------------------
    [Header("Menu Panels")]
    public Canvas startMenu;
    public Canvas quitMenu;

    // ---------------------------------------------------------
    // BUTTONS
    // ---------------------------------------------------------
    [Header("Buttons")]
    public Button playButton;
    public Button exitButton;

    // ---------------------------------------------------------
    // OPTIONAL UI TEXT
    // ---------------------------------------------------------
    [Header("Optional TMP Elements")]
    public TMP_Text titleText;

    // ---------------------------------------------------------
    // SCENE LOADING
    // ---------------------------------------------------------

#if UNITY_EDITOR
    [Header("Scene to Load (Editor Only)")]
    [Tooltip("Drag your gameplay scene here in the Editor.")]
    public SceneAsset targetScene;      // Editor-only type
#endif

    [Header("Runtime Scene Name (Used in Builds)")]
    [Tooltip("For Android/iOS/WebGL builds — enter the scene name EXACTLY as shown in Build Settings.")]
    public string sceneNameFallback;

    private string sceneName;            // Runtime-safe string


    // ---------------------------------------------------------
    // START
    // ---------------------------------------------------------
    private void Start()
    {
        quitMenu.enabled = false;

#if UNITY_EDITOR
        // Extract scene name from SceneAsset only inside the editor
        if (targetScene != null)
            sceneName = targetScene.name;
#endif

        // ⭐ Runtime fallback (all builds)
        if (string.IsNullOrEmpty(sceneName))
            sceneName = sceneNameFallback;

        if (string.IsNullOrEmpty(sceneName))
        {
            Debug.LogError("MenuManager: No scene name assigned! " +
                           "Fill in 'Runtime Scene Name' in the Inspector.");
        }
    }


    // ---------------------------------------------------------
    // QUIT MENU LOGIC
    // ---------------------------------------------------------
    public void ExitPress()
    {
        quitMenu.enabled = true;
        startMenu.enabled = false;
    }

    public void NoPress()
    {
        quitMenu.enabled = false;
        startMenu.enabled = true;
    }

    public void ExitGame()
    {
        Application.Quit();
    }


    // ---------------------------------------------------------
    // PLAY BUTTON LOGIC
    // ---------------------------------------------------------
    public void StartLevel()
    {
        if (string.IsNullOrEmpty(sceneName))
        {
            Debug.LogError("MenuManager: No scene assigned!");
            return;
        }

        // Use fade transition if available
        if (ScreenFader.Instance != null)
        {
            ScreenFader.Instance.FadeToScene(sceneName);
            return;
        }

        // Otherwise load instantly
        SceneManager.LoadScene(sceneName);
    }
}
