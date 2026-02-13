using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenu : MonoBehaviour
{
    public GameObject mainMenuPanel;
    public GameObject returnButton; // Reference to the return button
    private CanvasGroup canvasGroup;

void Start()
	{
    canvasGroup = mainMenuPanel.GetComponent<CanvasGroup>();

    if (PlayerPrefs.GetInt("Restarted", 0) == 1)
    {
        mainMenuPanel.SetActive(false);  
        returnButton.SetActive(true);
        PlayerPrefs.SetInt("Restarted", 0);
        PlayerPrefs.Save();
        
        Time.timeScale = 1; // ✅ Ensure game is running after restart
    }
    else
    {
        ShowMenu();
        Time.timeScale = 0; // ✅ Pause only if it's a fresh session
    }
	}

	public void PlayGame()
	{
		StartCoroutine(FadeOutMenu());
		Time.timeScale = 1; // Resume game when starting
	}

	public GameObject canvasHUD; // Ensure this is assigned in the Inspector

public void RestartGame()
	{
		returnButton.SetActive(true);
		PlayerPrefs.SetInt("Restarted", 1);
		PlayerPrefs.Save();
    
		Time.timeScale = 1; // Ensure the game unpauses
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    
		// Ensure canvas_HUD is active after the scene reloads
		StartCoroutine(ActivateHUDAfterReload());
	}

IEnumerator ActivateHUDAfterReload()
{
    yield return new WaitForSeconds(0.1f); // Wait briefly to ensure scene loads
    canvasHUD.SetActive(true);
}

    public void ExitGame()
    {
        Application.Quit();
    }

    public void ShowMenu()
    {
        mainMenuPanel.SetActive(true);
        returnButton.SetActive(false); // Hide return button when menu is open
        canvasGroup.alpha = 0; 
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
        StartCoroutine(FadeInMenu());
    }

	public void ReturnToMainMenu(int scene)
	{
		Time.timeScale = 0; // Pause game when returning to main menu
		SceneManager.LoadScene(scene); // Replace "MainMenuScene" with your actual main menu scene name
	}

    IEnumerator FadeOutMenu()
    {
        float duration = 0.5f;
        float elapsedTime = 0f;

        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(1, 0, elapsedTime / duration);
            yield return null;
        }

        canvasGroup.alpha = 0;
        mainMenuPanel.SetActive(false);
        returnButton.SetActive(true); // ✅ Ensure return button appears
    }

    IEnumerator FadeInMenu()
    {
        float duration = 0.0f;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(0, 1, elapsedTime / duration);
            yield return null;
        }

        canvasGroup.alpha = 1;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }
}
