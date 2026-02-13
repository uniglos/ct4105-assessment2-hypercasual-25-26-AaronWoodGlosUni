using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class SceneLoader : MonoBehaviour
{
    public CanvasGroup fadeCanvas; // Assign a UI Panel with a black image and CanvasGroup
    public float fadeDuration = 2f;

void Start()
{
    fadeCanvas.alpha = 1;  // Ensure it's fully opaque initially
}

public void LoadScene(string sceneName)
{
    StartCoroutine(FadeOutAndLoad(sceneName));
}

IEnumerator FadeOutAndLoad(string sceneName)
{
    float t = 2;
    while (t < fadeDuration)
    {
        t += Time.deltaTime;
        fadeCanvas.alpha = t / fadeDuration;  // Fade from 0 to 1
        yield return null;
    }

    SceneManager.LoadScene(sceneName);
}

    IEnumerator FadeOut()
    {
        float t = 2;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            fadeCanvas.alpha = t / fadeDuration;
            yield return null;
        }
        fadeCanvas.alpha = 0;
    }

IEnumerator FadeIn()
{
    fadeCanvas.alpha = 1; // Ensure it starts fully opaque
    float t = 2;
    while (t < fadeDuration)
    {
        t += Time.deltaTime;
        fadeCanvas.alpha = 1 - (t / fadeDuration); // Fade from 1 to 0
        yield return null;
    }
    fadeCanvas.alpha = 0;
}


    public void QuitGame()
    {
        Application.Quit();
    }
}
