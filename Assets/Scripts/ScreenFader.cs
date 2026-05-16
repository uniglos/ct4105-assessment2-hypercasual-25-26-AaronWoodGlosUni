using UnityEngine;                     // Core Unity engine features
using UnityEngine.SceneManagement;     // Lets us load scenes (levels)
using System.Collections;              // Needed for Coroutines (functions that run over time)

/// <summary>
/// SCREEN FADER — TEACHING EDITION
///
/// Purpose:
/// --------
/// This script lives on a full-screen UI panel (with a CanvasGroup) and
/// creates smooth fade transitions between scenes:
///
///     - Fade in from black when the game first starts.
///     - When asked, fade to black, load a new scene, then fade back in.
///
/// It also uses DontDestroyOnLoad so the same fader object survives
/// across all scenes and can be reused anywhere.
/// </summary>
public class ScreenFader : MonoBehaviour
{
    // ---------------------------------------------------------------------
    // 1. SINGLETON INSTANCE (ONE GLOBAL FADER)
    // ---------------------------------------------------------------------
    // Static property so any script in any scene can call:
    //     ScreenFader.Instance.FadeToScene("SomeScene");
    public static ScreenFader Instance { get; private set; }


    // ---------------------------------------------------------------------
    // 2. SETTINGS EXPOSED IN THE INSPECTOR
    // ---------------------------------------------------------------------

    [Header("Fade Settings")]
    [Tooltip("CanvasGroup on the full-screen fade panel (e.g. FadeTransitionPanel).")]
    public CanvasGroup canvasGroup;          // Controls opacity (alpha) and raycasts

    [Tooltip("How long each fade (in or out) should last in seconds.")]
    public float fadeDuration = 1f;          // Example: 1.0 = 1 second fade


    // ---------------------------------------------------------------------
    // 3. INTERNAL STATE
    // ---------------------------------------------------------------------

    // True while a fade is currently in progress.
    private bool isFading = false;



    // ---------------------------------------------------------------------
    // 4. AWAKE — CALLED BEFORE START, EVEN IF SCRIPT IS DISABLED
    // ---------------------------------------------------------------------
    private void Awake()
    {
        // ----- 4.1 Singleton enforcement -----
        // If an Instance already exists and it's not THIS one, destroy this copy.
        // This stops us having multiple faders if scenes are reloaded.
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        // Otherwise, this is now the one-and-only global ScreenFader.
        Instance = this;

        // ----- 4.2 Make this object persistent -----
        // We want the fader to exist across ALL scenes, so we mark it as
        // "do not destroy on load".
        DontDestroyOnLoad(gameObject);

        // ----- 4.3 Ensure we have a CanvasGroup reference -----
        if (canvasGroup == null)
        {
            // Try to automatically grab the CanvasGroup from this GameObject.
            canvasGroup = GetComponent<CanvasGroup>();
        }

        // If we STILL don't have one, something is wrong with the setup.
        if (canvasGroup == null)
        {
            Debug.LogError("ScreenFader: No CanvasGroup assigned or found. " +
                           "Please attach this script to a UI object with a CanvasGroup.");
            enabled = false; // Disable this script so it doesn’t spam errors.
            return;
        }

        // ----- 4.4 Initial CanvasGroup state -----
        // Start fully black (alpha = 1), because we will fade IN from black.
        canvasGroup.alpha = 1f;

        // We only want this panel to block clicks while the fade is happening.
        // We'll manage these flags in the fade routines.
        canvasGroup.interactable   = false;
        canvasGroup.blocksRaycasts = true;   // Block while we are still black on startup
    }



    // ---------------------------------------------------------------------
    // 5. START — CALLED ON THE FIRST FRAME
    // ---------------------------------------------------------------------
    private void Start()
    {
        // On first scene load, fade from black → clear.
        StartCoroutine(FadeIn());
    }



    // ---------------------------------------------------------------------
    // 6. PUBLIC API — CALL THIS FROM OTHER SCRIPTS
    // ---------------------------------------------------------------------
    /// <summary>
    /// Starts the fade-to-black → load scene → fade-from-black sequence.
    /// Example call:
    ///     ScreenFader.Instance.FadeToScene("GameScene");
    /// </summary>
    public void FadeToScene(string sceneName)
    {
        // Guard clauses: don’t start if already fading, or if name is invalid.
        if (isFading)
            return;

        if (string.IsNullOrEmpty(sceneName))
        {
            Debug.LogError("ScreenFader: sceneName is null or empty.");
            return;
        }

        StartCoroutine(FadeOutLoadFadeIn(sceneName));
    }



    // ---------------------------------------------------------------------
    // 7. FADE IN — BLACK → TRANSPARENT (USED ON STARTUP)
    // ---------------------------------------------------------------------
    private IEnumerator FadeIn()
    {
        isFading = true;

        // During fade we block raycasts so the user cannot click through.
        canvasGroup.blocksRaycasts = true;
        canvasGroup.interactable   = false;

        float t = 0f;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float normalizedTime = Mathf.Clamp01(t / fadeDuration);

            // Lerp from alpha 1 (black) to 0 (clear)
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, normalizedTime);

            yield return null; // Wait for the next frame
        }

        // Make sure final values are exact and clean.
        canvasGroup.alpha        = 0f;
        canvasGroup.blocksRaycasts = false;  // <-- Fix B: allow UI behind it to be clickable
        canvasGroup.interactable   = false;

        isFading = false;
    }



    // ---------------------------------------------------------------------
    // 8. MAIN SEQUENCE — FADE OUT → LOAD SCENE → FADE IN
    // ---------------------------------------------------------------------
    private IEnumerator FadeOutLoadFadeIn(string sceneName)
    {
        isFading = true;

        // While fading, we want to block UI interaction.
        canvasGroup.blocksRaycasts = true;
        canvasGroup.interactable   = false;

        // -------- Phase 1: fade OUT (visible → black) --------
        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float normalizedTime = Mathf.Clamp01(t / fadeDuration);

            // alpha: 0 (clear) → 1 (black)
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, normalizedTime);

            yield return null;
        }
        canvasGroup.alpha = 1f; // ensure fully black at end


        // -------- Phase 2: load the new scene in the background --------
        AsyncOperation async = SceneManager.LoadSceneAsync(sceneName);
        async.allowSceneActivation = false; // wait until we say go

        while (!async.isDone)
        {
            // async.progress goes from 0 → 0.9 while loading.
            // When it's >= 0.9, the scene is ready.
            if (async.progress >= 0.9f)
            {
                async.allowSceneActivation = true; // now actually switch scenes
            }

            yield return null;
        }

        // Give the new scene one frame to render.
        yield return null;


        // -------- Phase 3: fade IN (black → visible new scene) --------
        t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float normalizedTime = Mathf.Clamp01(t / fadeDuration);

            // alpha: 1 (black) → 0 (clear)
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, normalizedTime);

            yield return null;
        }

        // Final tidy state
        canvasGroup.alpha          = 0f;
        canvasGroup.blocksRaycasts = false;   // <-- Fix B: stop blocking UI once fade finishes
        canvasGroup.interactable   = false;

        isFading = false;
    }
}
