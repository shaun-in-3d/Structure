using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening; // Make sure to include the DOTween namespace
using UnityEngine.UI; // Required for working with UI components like Image

public class GlobalActions : MonoBehaviour
{
    public string gameSceneName = "Level_02";

    [SerializeField] 
    private CanvasGroup settingsMenu; // Reference to the Image component

    public float fadeDuration = 0.5f;


    private void Start()
    {
       // HideSettingsPanel();
    }
    public void StartGameX()
    {
        SceneManager.LoadScene(gameSceneName);
    }

    public void QuitGameX()
    {
        Application.Quit();
    }

    public void StartGame()
    {
        SceneManager.LoadScene(gameSceneName);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    
    public void RestartGame()
    {
        SceneManager.LoadScene("MainMenu");
    }
    
    
}