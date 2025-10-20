using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerScript : MonoBehaviour
{
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
    
    public void Quit()
    {
        Application.Quit();
    }

    public void MainPage()
    {
        GameManager.Instance.LoadSceneWithLoading("MainPage");
    }

    public void Replay()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        GameManager.Instance.RestartScene();
    }
}