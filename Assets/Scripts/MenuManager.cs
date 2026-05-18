using UnityEngine;                     // Core Unity engine features
using UnityEngine.UI;                  // Allows this script to reference UI Buttons & Canvases
using UnityEngine.SceneManagement;     // Required for loading scenes
using TMPro;                          // Required for TextMeshPro UI elements

#if UNITY_EDITOR
using UnityEditor;                     // Gives access to SceneAsset (Editor-only)
#endif


/// <summary>
/// MENU MANAGER — TEACHING EDITION
///
/// This script controls everything that happens on the **main menu screen**:
/// - Shows/hides menu panels (Start menu + Quit confirmation)
/// - Responds to button clicks
/// - Loads the next gameplay scene
/// - Uses the *ScreenFader* system (if present) for smooth transitions
///
/// IMPORTANT:
/// This script only runs in the Main Menu scene.
/// It should NOT be marked DontDestroyOnLoad — only the ScreenFader is persistent.
/// </summary>
public class MenuManager : MonoBehaviour
{
    // -------------------------------------------------------------------------
    //  SECTION 1 — UI PANELS
    // -------------------------------------------------------------------------
    // These are Canvas objects that contain entire groups of UI elements.
    // We toggle them ON/OFF when the player tries to quit the game.

    [Header("Menu Panels")]
    [Tooltip("Canvas containing the main menu buttons.")]
    public Canvas startMenu;

    [Tooltip("Canvas containing the 'Are you sure you want to quit?' popup.")]
    public Canvas quitMenu;


    // -------------------------------------------------------------------------
    //  SECTION 2 — BUTTON REFERENCES
    // -------------------------------------------------------------------------
    // These allow Unity to run functions when a player presses a button.

    [Header("Buttons")]
    [Tooltip("Button that starts the game.")]
    public Button playButton;

    [Tooltip("Button that opens the quit confirmation panel.")]
    public Button exitButton;


    // -------------------------------------------------------------------------
    //  SECTION 3 — OPTIONAL TEXTMESH PRO UI
    // -------------------------------------------------------------------------
    // You may have a TMP title or subtitle on the menu.
    // This is optional: leave empty if not used.

    [Header("Optional TMP Elements")]
    [Tooltip("Main title text (TextMeshPro). Leave empty if unused.")]
    public TMP_Text titleText;


    // -------------------------------------------------------------------------
    //  SECTION 4 — SCENE LOADING (Drag-and-Drop Scene Picker)
    // -------------------------------------------------------------------------
    // Instead of typing the name of the scene manually (prone to typos),
    // we use a SceneAsset field which only exists in the Unity Editor.
    //
    // When the game starts, we convert the SceneAsset into a *string name*
    // which can be safely passed to SceneManager.LoadScene().

    [Header("Scene to Load (Drag SceneAsset Here)")]
    [Tooltip("Drag your target scene from the Project window here.\n" +
             "Unity will extract the scene's name at runtime.")]
    public SceneAsset targetScene;     // Editor-only reference to a scene file

    private string sceneName;           // Runtime string version of the scene name



    // -------------------------------------------------------------------------
    //  UNITY START FUNCTION
    //  Called automatically by Unity when the scene begins.
    // -------------------------------------------------------------------------
    private void Start()
    {
        // Make sure the quit confirmation panel is hidden at the start.
        quitMenu.enabled = false;

#if UNITY_EDITOR
        // Convert SceneAsset → string
        // This is Editor-only because SceneAsset doesn't exist in a built game.
        if (targetScene != null)
        {
            sceneName = targetScene.name;
        }
#endif
    }


    // -------------------------------------------------------------------------
    //  SECTION 5 — BUTTON HANDLERS (Called by UI Buttons)
    // -------------------------------------------------------------------------

    /// <summary>
    /// Called when the player presses the EXIT button.
    /// Hides the main menu and shows the quit confirmation window.
    /// </summary>
    public void ExitPress()
    {
        quitMenu.enabled = true;      // Show confirmation panel
        startMenu.enabled = false;    // Hide main menu panel
    }

    /// <summary>
    /// Called when the player presses "No" on the quit confirmation.
    /// Restores the main menu.
    /// </summary>
    public void NoPress()
    {
        quitMenu.enabled = false;     // Hide confirmation panel
        startMenu.enabled = true;     // Show main menu panel
    }

    /// <summary>
    /// Called when the player presses YES on the quit confirmation.
    /// Works only in builds (ignored in the Editor).
    /// </summary>
    public void ExitGame()
    {
        Application.Quit();
    }


    // -------------------------------------------------------------------------
    //  SECTION 6 — START GAME
    // -------------------------------------------------------------------------
    /// <summary>
    /// Called when the player presses the PLAY button.
    /// This loads the next scene using ScreenFader if present.
    /// </summary>
    public void StartLevel()
    {
        // Check we actually have a scene assigned.
        if (string.IsNullOrEmpty(sceneName))
        {
            Debug.LogError("MenuManager: No target scene assigned! " +
                           "Please drag a scene asset into 'Target Scene'.");
            return;
        }

        // If the ScreenFader exists, use it for smooth fade transitions.
        if (ScreenFader.Instance != null)
        {
            ScreenFader.Instance.FadeToScene(sceneName);
        }
        else
        {
            // Fallback behaviour (only used if the fader prefab wasn't added):
            // Load instantly with no fade.
            SceneManager.LoadScene(sceneName);
        }
    }
}
