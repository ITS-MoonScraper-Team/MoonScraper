using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;
using TMPro;

public class GameMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject pauseMenuUI;
    //public static GameMenu InstanceGameMenu;
    public void CheckPause()
    {
        if (GameIsPaused)
        { Resume(); }
        else
        {
            Pause();
        }
    }
    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused=true;  
    }
    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }
    public void BackToMain()
    {
        //get Scenes to play from builder
        SceneManager.LoadScene(0/*,parameterers */);
    }



    //private void Awake()
    //{
    //    InstanceGameMenu = this;
    //}
    // Start is called before the first frame update
    //public void QuitGame()
    //{
    //    Application.Quit();
    //    Debug.Log("QUIT!");
    //}

    //// Start is called before the first frame update
    //void Start()
    //{
        
    //}

    //// Update is called once per frame
    //void Update()
    //{
        
    //}
}
