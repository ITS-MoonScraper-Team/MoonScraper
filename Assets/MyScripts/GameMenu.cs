using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;
using TMPro;
using DG.Tweening;
using UnityEngine.Events;

public class GameMenu : MonoBehaviour
{
    #region <VARIABLES>

    public static GameMenu instance;

    //INGAME MUSIC VOLUME SLIDER
    public Slider ingameVolumeSlider;
    public TMP_Text ingameVolumeSliderText;
    public Image sliderFiller;

    //INGAME SFX VOLUME SLIDER
    public Slider ingameSFXVolumeSlider;
    public TMP_Text ingameSFXVolumeSliderText;
    public Image SFXsliderFiller;

    //INGAME BUTTONS
    public Button backToGameButton;
    public Button GameMusicButton;

    //CONTROL VARIABLES
    public bool GameIsPaused = false;
    public bool MusicAudioON;
    //public static bool AudioON2;

    //public GameObject pauseMenuUI;
    //public Animator pauseMenuAnimator;
    #endregion

    #region <INIT>

    private void Awake()
    {
        if (SoundManager.instance)
            MusicAudioON = SoundManager.instance.inGameMusicAudioON;
        //else
        //    MusicAudioON = true;

        instance=this;
    }

    void Start()
    {
        if(SoundManager.MusicSource)
        SoundManager.MusicSource.Stop();

        if (MusicAudioON)
        {
            SoundManager.instance.PlaySound("inGame_OST");
            GameMusicButton.image.color = Color.green;
        }
        else
            GameMusicButton.image.color = Color.red;

        if (ingameVolumeSlider == null) 
            return;

        ///FIXED: METTE 0 LA PRIMA VOLTA ALL'AVVIO
        ///FIXED: RIPRODUCE RUMORE BACKCLICK ALLO START

        //Aggiunge listener solo per volume update, così non riproduce SFX all'avvio
        ingameVolumeSlider.onValueChanged.AddListener(MusicVolumeSliderUpdate);
        ingameSFXVolumeSlider.onValueChanged.AddListener(SFXVolumeSliderUpdate);

        //Update inziale
        ingameSFXVolumeSlider.value = SFXsoundManager.instance.SFXVolumeToSlider;
        ingameVolumeSlider.value = SoundManager.instance.VolumeToSlider;

        //Aggiunge listener anche per gli SFX all'update
        ingameVolumeSlider.onValueChanged.AddListener(MusicVolumeSliderSFX);
        ingameSFXVolumeSlider.onValueChanged.AddListener(SFXVolumeSliderSFX);
    }
    #endregion

    #region <<BUTTON INTERACTIONS>>

    #region <PAUSE-RESUME FUNCTIONS>

    public void CheckPause()
    {
        SFXsoundManager.instance.PlaySound("okClick");
        SoundManager.instance.SavePlayerSettings();
        SFXsoundManager.instance.SavePlayerSettings();
        if (GameIsPaused)
        {
            ///TO FIX
            //backToGameButton.gameObject.transform.DOScale(Vector3.one * 8f, .15f).SetEase(Ease.InBounce);
            //Invoke("Resume", 0.2f);
            
            Resume();
        }
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
        GameIsPaused=true;  
        //SoundManager.AudioSrc.volume =.5f;
    }
    public void Resume()
    {
        //pauseMenuUI.SetActive(false);
        //pauseMenuAnimator.enabled = false;
        Time.timeScale = 1f;
        PlayerController.joystickControl=true;
        GameIsPaused = false;
        //backToGameButton.gameObject.transform.DOScale(Vector3.one * 8f, .15f).SetEase(Ease.InBounce);

    }
    #endregion

    #region <BACK TO MAIN>

    public void BackToMain()
    {
        SFXsoundManager.instance.PlayOKButtonSFXSound();
        SFXsoundManager.instance.SavePlayerSettings();
        SoundManager.instance.SavePlayerSettings();
        //LivesSliderManager.instance.SavePlayerLivesSettings();
        Resume();
        //get Scenes to play from builder
        SceneManager.LoadScene(0/*,parameterers */);
    }
    #endregion

    #region <AUDIO ON/OFF>

    public void AudioONOFF()
    {
        if (MusicAudioON)
        {
            //MusicSource.enabled = false;
            MusicAudioON = false;
            SoundManager.instance.inGameMusicAudioON = false;
            SoundManager.MusicSource.Stop();
            GameMusicButton.image.color = Color.red;
        }
        else
        {
            //MusicSource.enabled = true;
            MusicAudioON = true;
            SoundManager.instance.inGameMusicAudioON = true;
            SoundManager.instance.PlaySound("inGame_OST");
            GameMusicButton.image.color = Color.green;


        }
    }
    #endregion

    #endregion

    #region <VOLUME SLIDERS UPDATES>

    //SET VOLUME SLIDERS
    private void MusicVolumeSliderUpdate(float val)
    {
        SoundManager.instance.UpdateVolume(val);
        UpdateSliderText(val, ingameVolumeSlider, sliderFiller);
    }
    private void SFXVolumeSliderUpdate(float val)
    {
        SFXsoundManager.instance.UpdateSFXVolume(val);
        UpdateSliderText(val, ingameSFXVolumeSlider, SFXsliderFiller);
    }

    //PLAY SLIDER SFX
    private void MusicVolumeSliderSFX(float val) 
    {
        SFXsoundManager.instance.PlaySettingsSFXSound();
    }
    private void SFXVolumeSliderSFX(float val)
    {
        SFXsoundManager.instance.PlaySettingsSFXSound();
    }

    //SET SLIDER TEXT
    private void UpdateSliderText(float val, Slider slide, Image fill)
    {
        if (slide == null) return;

        if (slide.value == 0)
        {
            slide.GetComponentInChildren<TMP_Text>().text = "OFF";
            //volumeSliderTxt.fontSize = 60;
            slide.GetComponentInChildren<TMP_Text>().color = Color.white;
            fill.color = Color.white;
        }
        else if (slide.value == 100)
        {
            slide.GetComponentInChildren<TMP_Text>().text = "MAX";
            slide.GetComponentInChildren<TMP_Text>().color = Color.red;
            fill.color = Color.red;
        }
        else
        {
            slide.GetComponentInChildren<TMP_Text>().text = slide.value.ToString();
            slide.GetComponentInChildren<TMP_Text>().color = Color.yellow;
            fill.color = Color.yellow;
        }
    }
    #endregion

    void Update()
    { }
}
