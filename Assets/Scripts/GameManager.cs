using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadSceneWithLoading(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void RestartScene()
    {
        // Add game state reset logic here (e.g., reset score, health)
    }
}