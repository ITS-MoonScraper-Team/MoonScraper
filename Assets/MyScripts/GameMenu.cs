using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;
using TMPro;

public class GameMenu : MonoBehaviour
{
    public Slider ingameVolumeSlider;
    public TMP_Text ingameVolumeSliderText;
    public Image sliderFiller;
    public Slider ingameSFXVolumeSlider;
    public TMP_Text ingameSFXVolumeSliderText;
    public Image SFXsliderFiller;
    public GameObject pauseMenuUI;
    public Animator pauseMenuAnimator;
    public bool GameIsPaused = false;

    public static GameMenu instance;

    private void Awake()
    {
        instance=this;
    }

    #region PAUSE-RESUME FUNCTIONS

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
        //pauseMenuUI.SetActive(true);
        //pauseMenuUI.GetComponent<Animator>().enabled = true;
        //pauseMenuAnimator.enabled = true;

        Time.timeScale = 0f;
        PlayerController.joystickControl = false;
        //SoundManager.AudioSrc.volume =.5f;
        //SoundManager.BackgroundAudio.volume = .5f;
        GameIsPaused=true;  
    }
    public void Resume()
    {
        //pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        PlayerController.joystickControl=true;
        GameIsPaused = false;
        //pauseMenuAnimator.enabled = false;

    }
    #endregion

    #region BACK TO MAIN

    public void BackToMain()
    {
        //get Scenes to play from builder
        Resume();
        SceneManager.LoadScene(0/*,parameterers */);
    }
    #endregion

    //public void QuitGame()
    //{
    //    Application.Quit();
    //    Debug.Log("QUIT!");
    //}

    void Start()
    {
        SoundManager.instance.PlaySound("inGame_OST");

        if (ingameVolumeSlider == null) 
            return;

        ingameVolumeSlider.onValueChanged.AddListener(UpdateVolumeText);
        ingameVolumeSlider.value=SoundManager.instance.VolumeToSlider;

        ingameSFXVolumeSlider.onValueChanged.AddListener(UpdateSFXVolumeText);
        ingameSFXVolumeSlider.value = SFXsoundManager.instance.SFXVolumeToSlider;
        //UpdateVolumeText((float)SoundManager.instance.volumeToSlider);
    }


    //UPDATE VOLUME SLIDER IN GAME e SOUNDMANAGER
    private void UpdateVolumeText(float val)
    {
        if (ingameVolumeSlider == null) return;
        if (ingameVolumeSlider.value == 0)
        {
            ingameVolumeSliderText.text = "OFF";
            ingameVolumeSliderText.color = Color.white;

            sliderFiller.color = Color.white;
        }
        else if (ingameVolumeSlider.value == 100)
        {
            ingameVolumeSliderText.text = "MAX";
            ingameVolumeSliderText.color = Color.red;
            sliderFiller.color = Color.red;
        }
        else
        {
            ingameVolumeSliderText.text = ingameVolumeSlider.value.ToString();
            ingameVolumeSliderText.color = Color.yellow;
            sliderFiller.color = Color.yellow;
        }
        SoundManager.instance.UpdateVolume(val);
        //SFXsoundManager.instance.PlaySound("backClick");

    }

    private void UpdateSFXVolumeText(float val)
    {
        if (ingameSFXVolumeSlider == null) return;
        if (ingameSFXVolumeSlider.value == 0)
        {
            ingameSFXVolumeSliderText.text = "OFF";
            ingameSFXVolumeSliderText.color = Color.white;

            SFXsliderFiller.color = Color.white;
        }
        else if (ingameSFXVolumeSlider.value == 100)
        {
            ingameSFXVolumeSliderText.text = "MAX";
            ingameSFXVolumeSliderText.color = Color.red;
            SFXsliderFiller.color = Color.red;
        }
        else
        {
            ingameSFXVolumeSliderText.text = ingameSFXVolumeSlider.value.ToString();
            ingameSFXVolumeSliderText.color = Color.yellow;
            SFXsliderFiller.color = Color.yellow;
        }
        SFXsoundManager.instance.UpdateSFXVolume(val);
        //SFXsoundManager.instance.PlaySound("backClick");

    }

    void Update()
    {
        
    }
}
