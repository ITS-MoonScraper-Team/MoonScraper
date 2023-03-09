using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;
using TMPro;

public class GameMenu : MonoBehaviour
{
    public static GameMenu instance;
    public bool GameIsPaused = false;
    public GameObject pauseMenuUI;
    public Animator pauseMenuAnimator;
    //public static GameMenu InstanceGameMenu;

    private void Awake()
    {
        instance=this;
    }

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
        //pauseMenuUI.GetComponent<Animator>().enabled = true;
        pauseMenuAnimator.enabled = true;

        Time.timeScale = 0f;
        PlayerController.joystickControl = false;
        //SoundManager.AudioSrc.volume =.5f;
        //SoundManager.BackgroundAudio.volume = .5f;
        GameIsPaused=true;  
    }
    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        PlayerController.joystickControl=true;
        GameIsPaused = false;
        //pauseMenuAnimator.enabled = false;

    }
    public void BackToMain()
    {
        //get Scenes to play from builder
        Resume();
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
