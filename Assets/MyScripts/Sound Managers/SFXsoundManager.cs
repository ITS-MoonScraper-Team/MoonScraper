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
    public AudioSource sfxSourceShooter;
    public AudioSource sfxSourceRefill;

    //SFX VOLUME VARIABLES
    private int sFXVolumeToSlider;
    public int SFXVolumeToSlider { get { return sFXVolumeToSlider; }/* set; */}

    private float sFXVolumeLvl;
    public float SFXVolumeLvl { get { return sFXVolumeLvl; } /*set; */}

    //AUDIO CLIPS
    [SerializeField] private AudioClip playerDeathClip, okClick, backClick, jetpackPropulsion, refillSound;
    [SerializeField] private List<AudioClip> shootSound /*= new List<AudioClip>()*/; 


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
        sfxSourceShooter.volume = sfxSource1.volume * 0.5f ;

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
            case "shotSound":
                sfxSourceShooter.clip = shootSound[ Random.Range(0, 5)];
                sfxSourceShooter.Play();
                break;
            case "refillSound":
                sfxSourceRefill.clip = refillSound;
                sfxSourceRefill.Play();
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
    public void PlayFuelRefillSound()
    {
        PlaySound("refillSound");
    }
    #endregion

    #region <SFX VOLUME UPDATE>

    public void UpdateSFXVolume(float value)
    {
        //prende valore dallo slider e aggiorna la variabile che passa tra le scene

        //volume che appare sullo slider
        sFXVolumeToSlider = (int)value;
        //volume effettivo tra 0 e 1 (float)
        sFXVolumeLvl = value / 100f;
        sfxSource1.volume = SFXVolumeLvl;
        sfxSourceJetpack.volume = SFXVolumeLvl;
        sfxSourceShooter.volume = SFXVolumeLvl * 0.5f;
        sfxSourceRefill.volume = SFXVolumeLvl;

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
            sFXVolumeToSlider = SFXvolSlider;
        }
        if (PlayerPrefs.HasKey("SFXVolumeLvl"))
        {
            float SFXvolLvl = PlayerPrefs.GetFloat("SFXvolumeLvl");
            sFXVolumeLvl = SFXvolLvl;
        }
        Debug.Log("loaded data");
    }
    #endregion

    void Update()
    {

    }
}
