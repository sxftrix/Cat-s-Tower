using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerScript : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject pauseMenuPanel;
    public GameObject shopPanel;

    private bool isPaused = false;
    private bool isShopOpen = false;

    // Play button from main menu
    public void PlayGame()
    {
        GameManager.Instance.LoadSceneWithLoading("Game");
    }

    public void Credits()
    {
        SceneManager.LoadScene("Creds");
    }

    public void Back()
    {
        SceneManager.LoadScene("MainPage");
    }

    // Quit works in both editor and build
    public void Quit()
    {
        // UnityEditor.EditorApplication.isPlaying = false;

        Application.Quit();
    }

    public void MainPage()
    {
        isPaused = false;
        Time.timeScale = 1f;
        if (pauseMenuPanel != null)
            pauseMenuPanel.SetActive(false);

        GameManager.Instance.LoadSceneWithLoading("MainPage");
    }

    // Pause the game (toggle)
    public void TogglePause()
    {
        isPaused = !isPaused;
        if (pauseMenuPanel != null)
            pauseMenuPanel.SetActive(isPaused);

        Time.timeScale = isPaused ? 0f : 1f;
    }

    public void Resume()
    {
        isPaused = false;
        Time.timeScale = 1f;
        if (pauseMenuPanel != null)
            pauseMenuPanel.SetActive(false);
    }

    // SHOP UI
    public void ToggleShop()
    {
        // Prevent opening shop while paused
        if (isPaused) return;

        isShopOpen = !isShopOpen;

        if (shopPanel != null)
            shopPanel.SetActive(isShopOpen);

        // Optional: freeze game while shop is open
        Time.timeScale = isShopOpen ? 0f : 1f;
    }

    public void CloseShop()
    {
        isShopOpen = false;
        if (shopPanel != null)
            shopPanel.SetActive(false);

        Time.timeScale = 1f;
    }
}
