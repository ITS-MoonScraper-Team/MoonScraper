using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class SFXsoundManager : MonoBehaviour
{
    #region <VARIABLES>

    public static SFXsoundManager instance;

    //SFX AUDIO SOURCES
    public static AudioSource sfxSource1;
    public AudioSource sfxSourceJetpack;

    //SFX VOLUME VARIABLES
    private int SFXvolumeToSlider;
    public int SFXVolumeToSlider { get; set; }

    private float SFXvolumeLvl;
    public float SFXVolumeLvl { get; set; }

    //AUDIO CLIPS
    [SerializeField] private AudioClip playerDeathClip, okClick, backClick, jetpackPropulsion;

    //Future use
    //public static bool SFXgameAudioON;
    //public static bool SFXMenuAudioON;
    //public Button MenuSFXAudioButton;
    //public Button GameSFXAudioButton;

    #endregion

    #region <INIT>

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
        //sfxSourceJetpack = GetComponentInChildren<AudioSource>();
        sfxSource1 = GetComponent<AudioSource>();
    }
    #endregion

    #region <PLAY SFX SOUNDS>

    public void PlaySound(string clip)
    {
        switch (clip)
        {
            case "playerDeath":
                sfxSource1.PlayOneShot(playerDeathClip);
                break;
            case "okClick":
                sfxSource1.PlayOneShot(okClick);
                break;
            case "backClick":
                sfxSource1.PlayOneShot(backClick);
                break;
            case "jetPackProp":
                sfxSourceJetpack.clip = jetpackPropulsion;
                sfxSourceJetpack.Play();
                break;
        }
    }

    public void PlaySettingsSFXSound() 
    {
        PlaySound("backClick");
    }

    public void PlayOKButtonSFXSound()
    {
        PlaySound("okClick");
    }

    public void PlayDeathSound()
    {
        PlaySound("playerDeath");
    }
    public void PlayJetpackPropulsion()
    {
        PlaySound("jetPackProp");
    }
    #endregion

    #region <SFX VOLUME UPDATE>

    public void UpdateSFXVolume(float value)
    {
        //prende valore dallo slider e aggiorna la variabile che passa tra le scene

        //volume che appare sullo slider
        SFXVolumeToSlider = (int)value;
        //volume effettivo tra 0 e 1 (float)
        SFXVolumeLvl = value / 100f;
        sfxSource1.volume = SFXVolumeLvl;
        sfxSourceJetpack.volume = SFXVolumeLvl;
    }
    #endregion

    #region <SAVE/LOAD SFX VOLUME SETTINGS>

    public void SavePlayerSettings()
    {
        int SFXvolSlider = SFXVolumeToSlider;
        float SFXvolLvl = SFXVolumeLvl;
        PlayerPrefs.SetInt("SFXVolumeToSlider", SFXvolSlider);
        PlayerPrefs.SetFloat("SFXVolumeLvl", SFXvolLvl);
        PlayerPrefs.Save();
        Debug.Log("saved data");
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
    #endregion

    void Update()
    {

    }
}
