using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class SFXsoundManager : MonoBehaviour
{

    #region |> VARIABLES <|

    public static SFXsoundManager instance;
    public Button GameSFXAudioButton;
    //public Button MenuAudioButton;
    public TMP_Text GameSFXAudioText;
    public static AudioSource MusicSource2;
    public static AudioSource sfxSource;
    public static bool AudioON;
    //public static bool MenuAudioON;

    private int SFXvolumeToSlider;
    public int SFXVolumeToSlider { get; set; }

    private float SFXvolumeLvl;
    public float SFXVolumeLvl { get; set; }

    //AUDIO CLIPS
    [SerializeField] private AudioClip playerDeathClip, okClick, backClick, jetpackPropulsion;

    #endregion

    #region ||>> INIT <<||

    void Awake()
    {
        if (SFXsoundManager.instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        LoadPlayerSettings();
    }

    void Start()
    {
        sfxSource = GetComponentInChildren<AudioSource>();
        MusicSource2 = GetComponent<AudioSource>();

        
    }
    #endregion

    /// <summary>
    /// PROBLEMA:
    /// STOPPA AUDIO DELL'ESPLOSIONE QUANDO STOPPA JETPACK
    /// </summary>
    #region ||>> PLAY SFX SOUNDS <<||

    public void PlaySound(string clip)
    {
        switch (clip)
        {
            case "playerDeath":
                MusicSource2.PlayOneShot(playerDeathClip);
                break;
            case "okClick":
                MusicSource2.PlayOneShot(okClick);
                break;
            case "backClick":
                MusicSource2.PlayOneShot(backClick);
                break;
            case "jetPackProp":
                sfxSource.clip = jetpackPropulsion;
                sfxSource.Play();
                break;
        }
    }
    #endregion

    public void UpdateSFXVolume(float value)
    {

        //prende valore dallo slider e aggiorna la variabile che passa tra le scene

        //volume che appare sullo slider
        SFXVolumeToSlider = (int)value;

        //volume effettivo tra 0 e 1 (float)
        SFXVolumeLvl = value / 100f;
        MusicSource2.volume = SFXVolumeLvl;

        PlaySound("backClick");
    }

    public void SavePlayerSettings()
    {
        int SFXvolSlider = SFXVolumeToSlider;
        float SFXvolLvl = SFXVolumeLvl;
        PlayerPrefs.SetInt("SFXVolumeToSlider", SFXvolSlider);
        PlayerPrefs.SetFloat("SFXVolumeLvl", SFXvolLvl);
        PlayerPrefs.Save();
        Debug.Log("saved date");
    }
    public void LoadPlayerSettings()
    {
        if (PlayerPrefs.HasKey("SFXVolumeToSlider"))
        {
            int SFXvolSlider = PlayerPrefs.GetInt("SFXVolumeToSlider");
            SFXVolumeToSlider = SFXvolSlider;
        }
        if (PlayerPrefs.HasKey("SFXVolumeLvl"))
        {
            float SFXvolLvl = PlayerPrefs.GetFloat("SFXvolumeLvl");
            SFXVolumeLvl = SFXvolLvl;
        }
        Debug.Log("loaded data");
    }

    void Update()
    {

    }
    
    #region |>Future Use<|
    public void UpdateVolume(float value)
    {

        ////prende valore dallo slider e aggiorna la variabile che passa tra le scene

        ////volume che appare sullo slider
        //VolumeSFXToSlider = (int)value;

        ////volume effettivo tra 0 e 1 (float)
        //VolumeSFXLvl = value / 100f;
        //MusicSource.volume = VolumeSFXLvl;
    }
    public void AudioONOFF()
    {
        //if (AudioON)
        //{
        //    AudioON = false;
        //    MusicSource.Stop();
        //    //AudioSrc.enabled = false;
        //    //AudioText.text = "audio OFF";
        //    //GameAudioText.text = "audio OFF";
        //    GameSFXAudioButton.GetComponentInChildren<TMP_Text>().text = "audio OFF";
        //    GameSFXAudioButton.GetComponent<Image>().color = Color.red;
        //}
        //else
        //{
        //    AudioON = true;
        //    MusicSource.Play();
        //    //AudioSrc.enabled = true;
        //    //GameAudioText.text = "audio ON";
        //    GameSFXAudioButton.GetComponentInChildren<TMP_Text>().text = "audio ON";
        //    GameSFXAudioButton.GetComponent<Image>().color = Color.green;
        //}
    }
   
    #endregion

}
