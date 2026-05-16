using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

/// <summary>
/// MENU RETURN (Ultra-Annotated, Build-Safe Version)
/// --------------------------------------------------
/// PURPOSE:
/// This script is attached to a UI Button.
/// When clicked, it returns the player to the Main Menu scene.
///
/// WHY THIS VERSION EXISTS:
/// • SceneAsset ONLY exists in the Unity Editor
/// • Builds cannot reference project assets
/// • Builds can ONLY load scenes by:
///     ✔ Scene name (string)
///     ✔ Scene build index (int)
///
/// Therefore:
/// 👉 We store the scene name as a plain string.
/// 👉 This works in BOTH the Editor and final builds.
/// 👉 No preprocessor directives required.
///
/// This is the recommended version for:
/// • Teaching
/// • Student projects
/// • Production-safe UI navigation
/// </summary>
public class MenuReturn : MonoBehaviour
{
    // ---------------------------------------------------------------------
    //  SECTION 1 — SCENE NAME (RUNTIME SAFE)
    // ---------------------------------------------------------------------

    [Header("Scene Navigation")]

    [Tooltip(
        "Name of the Main Menu scene.\n" +
        "IMPORTANT:\n" +
        "• Must EXACTLY match the scene file name\n" +
        "• Must be added to File > Build Settings"
    )]
    [SerializeField]
    private string menuSceneName = "MainMenu";


    // ---------------------------------------------------------------------
    //  SECTION 2 — OPTIONAL UI ELEMENTS
    // ---------------------------------------------------------------------

    [Header("Optional UI Elements")]

    [Tooltip("Optional TextMeshPro label on the button.")]
    public TMP_Text buttonLabel;


    // ---------------------------------------------------------------------
    //  UNITY START — VALIDATION ONLY
    // ---------------------------------------------------------------------

    private void Start()
    {
        // Safety check to catch common setup errors early
        if (string.IsNullOrEmpty(menuSceneName))
        {
            Debug.LogWarning(
                "MenuReturn WARNING:\n" +
                "No menu scene name has been assigned.\n" +
                "The button will NOT function in a build."
            );
        }
    }


    // ---------------------------------------------------------------------
    //  BUTTON CALLBACK — RETURN TO MAIN MENU
    // ---------------------------------------------------------------------

    /// <summary>
    /// Called by the UI Button OnClick() event.
    /// Safely returns the player to the Main Menu scene.
    /// </summary>
    public void ReturnToMenu()
    {
        // -----------------------------------------------------------------
        //  STEP 1 — VALIDATE INPUT
        // -----------------------------------------------------------------

        if (string.IsNullOrEmpty(menuSceneName))
        {
            Debug.LogError(
                "MenuReturn ERROR:\n" +
                "menuSceneName is empty.\n" +
                "Assign a valid scene name in the Inspector."
            );
            return;
        }

        // -----------------------------------------------------------------
        //  STEP 2 — OPTIONAL SCREEN FADER (IF PRESENT)
        // -----------------------------------------------------------------

        // If your project includes a ScreenFader singleton,
        // we use it automatically for smooth transitions.
        if (ScreenFader.Instance != null)
        {
            ScreenFader.Instance.FadeToScene(menuSceneName);
            return;
        }

        // -----------------------------------------------------------------
        //  STEP 3 — FALLBACK: DIRECT SCENE LOAD
        // -----------------------------------------------------------------

        SceneManager.LoadScene(menuSceneName);
    }
}
