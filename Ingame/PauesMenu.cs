using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauesMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    public static bool isPaused; //faulse by default
    
    void Start()
    {
        pauseMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
        
    }
    public void PauseGame()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }
    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }
    public void toMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
        isPaused = false;
    }
    public void SelectLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(14);
        isPaused = false;
    }
    public void QuitGame()
    {
        //Application.Quit();//only works in build
        System.Diagnostics.Process.GetCurrentProcess().Kill();//works in editor(warning save before)
    }
}
