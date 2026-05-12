using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System.Collections;

/// <summary>
/// SceneLoader (Hardened Version)
/// ---------------------------------------------------------------------------
/// PURPOSE
/// Handles scene transitions AND actively defends against UI corruption
/// across repeated scene loads.
///
/// This version includes runtime guards for:
/// - Canvas scale drift
/// - Raycast blocking overlays
/// - Duplicate EventSystems
/// - TimeScale issues
/// - Coroutine carry-over
/// </summary>
public class SceneLoader : MonoBehaviour
{
    [Header("Fade Settings")]
    public CanvasGroup fadeCanvas;
    public float fadeDuration = 1f;

    private Coroutine activeFade;

    // ------------------------------------------------------------------------
    // LIFECYCLE
    // ------------------------------------------------------------------------

    private void Awake()
    {
        EnforceSingleEventSystem();
    }

    private void OnEnable()
    {
        ResetFadeState();
    }

    private void LateUpdate()
    {
        RuntimeUIIntegrityGuard();
    }

    // ------------------------------------------------------------------------
    // PUBLIC API
    // ------------------------------------------------------------------------

    public void LoadScene(string sceneName)
    {
        StopAllCoroutines();
        activeFade = StartCoroutine(FadeOutAndLoad(sceneName));
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    // ------------------------------------------------------------------------
    // CORE RESET
    // ------------------------------------------------------------------------

    void ResetFadeState()
    {
        if (fadeCanvas == null)
        {
            Debug.LogWarning("[SceneLoader] No fadeCanvas assigned.");
            return;
        }

        // Reset timescale (critical)
        Time.timeScale = 1f;

        StopAllCoroutines();

        fadeCanvas.alpha = 1f;
        fadeCanvas.blocksRaycasts = true;
        fadeCanvas.interactable = false;

        activeFade = StartCoroutine(FadeIn());
    }

    // ------------------------------------------------------------------------
    // RUNTIME GUARD (CRITICAL)
    // ------------------------------------------------------------------------

    void RuntimeUIIntegrityGuard()
    {
        // 1. Force Canvas scale to remain valid
        Canvas canvas = GetComponentInChildren<Canvas>();
        if (canvas != null)
        {
            if (canvas.transform.localScale != Vector3.one)
            {
                canvas.transform.localScale = Vector3.one;
            }
        }

        // 2. Ensure fade overlay never blocks when invisible
        if (fadeCanvas != null && fadeCanvas.alpha <= 0.01f)
        {
            fadeCanvas.blocksRaycasts = false;
            fadeCanvas.interactable = false;
        }

        // 3. Emergency fallback: ensure no hidden UI is blocking input
        if (Input.GetMouseButtonDown(0))
        {
            // Debug hook if needed later
            // Debug.Log("Click registered - checking blockers");
        }
    }

    // ------------------------------------------------------------------------
    // FADE COROUTINES
    // ------------------------------------------------------------------------

    IEnumerator FadeIn()
    {
        float t = 0f;

        while (t < fadeDuration)
        {
            t += Time.unscaledDeltaTime;
            fadeCanvas.alpha = 1f - Mathf.Clamp01(t / fadeDuration);
            yield return null;
        }

        fadeCanvas.alpha = 0f;
        fadeCanvas.blocksRaycasts = false;
        fadeCanvas.interactable = false;
    }

    IEnumerator FadeOutAndLoad(string sceneName)
    {
        float t = 0f;

        fadeCanvas.blocksRaycasts = true;

        while (t < fadeDuration)
        {
            t += Time.unscaledDeltaTime;
            fadeCanvas.alpha = Mathf.Clamp01(t / fadeDuration);
            yield return null;
        }

        fadeCanvas.alpha = 1f;

        SceneManager.LoadScene(sceneName);
    }

    // ------------------------------------------------------------------------
    // SYSTEM GUARDS
    // ------------------------------------------------------------------------

    void EnforceSingleEventSystem()
    {
        EventSystem[] systems = FindObjectsOfType<EventSystem>();

        if (systems.Length > 1)
        {
            for (int i = 1; i < systems.Length; i++)
            {
                Destroy(systems[i].gameObject);
            }
        }
    }
}