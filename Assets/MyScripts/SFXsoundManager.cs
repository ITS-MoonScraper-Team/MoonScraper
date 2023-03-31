using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class SFXsoundManager : MonoBehaviour
{
    public Button GameSFXAudioButton;
    //public Button MenuAudioButton;
    public TMP_Text GameSFXAudioText;
    public static AudioSource MusicSource2;
    public static AudioSource sfxSource;
    //public static AudioClip playerDeathSound, jetpack, landing, mainMenuOST;
    //public static AudioSource AudioSrc;

    public static bool AudioON;
    //public static bool MenuAudioON;
    private int volumeSFXToSlider;
    public int VolumeSFXToSlider { get; set; }

    private float volumeSFXLvl;
    public float VolumeSFXLvl { get; set; }

    public static SFXsoundManager instance;

    //AUDIO CLIPS
    [SerializeField] private AudioClip playerDeathClip, okClick, backClick, jetpackPropulsion;

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
        //AudioON = true;
        sfxSource = GetComponentInChildren<AudioSource>();
        MusicSource2 = GetComponent<AudioSource>();
        //if (GameAudioButton != null)
        //{
        //    GameAudioText.text = "audio ON";
        //    GameAudioButton.image.color = Color.green;
        //}
    }

    /// <summary>
    /// PROBLEMA:
    /// STOPPA AUDIO DELL'ESPLOSIONE QUANDO STOPPA JETPACK
    /// </summary>
    /// <param name="clip"></param>

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

    public void SavePlayerSettings()
    {
        //int volSlider = VolumeSFXToSlider;
        //float volLvl = VolumeSFXLvl;
        //PlayerPrefs.SetInt("volumeToSlider", volSlider);
        //PlayerPrefs.SetFloat("volumeLvl", volLvl);
        //PlayerPrefs.Save();
        //Debug.Log("saved date");
    }
    public void LoadPlayerSettings()
    {
        //if (PlayerPrefs.HasKey("volumeToSlider"))
        //{
        //    int volSlider = PlayerPrefs.GetInt("volumeToSlider");
        //    VolumeSFXToSlider = volSlider;
        //}
        //if (PlayerPrefs.HasKey("volumeLvl"))
        //{
        //    float volLvl = PlayerPrefs.GetFloat("volumeLvl");
        //    VolumeSFXLvl = volLvl;

        //}
        //Debug.Log("loaded data");
    }

    void Update()
    {

    }

}
