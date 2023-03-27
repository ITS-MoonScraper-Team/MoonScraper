using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public Button GameAudioButton;
    //public Button MenuAudioButton;
    public TMP_Text GameAudioText;
    public static AudioSource MusicSource;
    //public static AudioClip playerDeathSound, jetpack, landing, mainMenuOST;
    //public static AudioSource AudioSrc;

    public static bool AudioON;
    //public static bool MenuAudioON;
    private int volumeToSlider;
    public int VolumeToSlider { get; set; }

    private float volumeLvl;
    public float VolumeLvl { get; set; }

    public static SoundManager instance;

    //AUDIO CLIPS
    [SerializeField] private AudioClip menuClip, gameClip, playerDeathClip,okClick, backClick;

    void Awake()
    {
        if (SoundManager.instance != null)
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
        AudioON = true;
        //LOAD RESOURCES DRIVER METHOD
        //playerDeathSound = Resources.Load < AudioClip > ("playerDeath");
        //mainMenuOST = Resources.Load<AudioClip>("mainMenu_OST");
        //AudioSrc.enabled = true;

        MusicSource = GetComponent<AudioSource>();
        if (GameAudioButton != null)
        {
            GameAudioText.text = "audio ON";
            GameAudioButton.image.color = Color.green;
        }
    }

    public void PlaySound(string clip)
    {
        switch (clip)
        {
            case "playerDeath":
                MusicSource.PlayOneShot(playerDeathClip);
                break;
            case "mainMenu_OST":
                MusicSource.clip =menuClip;
                MusicSource.Play();
                break;
            case "inGame_OST":
                MusicSource.clip = gameClip;
                MusicSource.Play();
                break;
            case "okClick":
                MusicSource.PlayOneShot(okClick);
                break;
            case "backClick":
                MusicSource.PlayOneShot(backClick);
                break;
                //case "inGame_OST":
                //    MusicSource.PlayOneShot(playerDeathSound);
                //    break;
                //case "playerDeath":
                //    MusicSource.PlayOneShot(playerDeathSound);
                //    break;
        }
    }

    public void UpdateVolume(float value) {

        //prende valore dallo slider e aggiorna la variabile che passa tra le scene

        //volume che appare sullo slider
        VolumeToSlider = (int)value;

        //volume effettivo tra 0 e 1 (float)
        VolumeLvl = value / 100f;
        MusicSource.volume = VolumeLvl;
    }
    public void PlayOkClick()
    {
        MusicSource.PlayOneShot(okClick);
    }

    public void AudioONOFF()
    {
        if (AudioON)
        {
            AudioON = false;
            MusicSource.Stop();
            //AudioSrc.enabled = false;
            //AudioText.text = "audio OFF";
            //GameAudioText.text = "audio OFF";
            GameAudioButton.GetComponentInChildren<TMP_Text>().text = "audio OFF";
            GameAudioButton.GetComponent<Image>().color = Color.red;
        }
        else
        {
            AudioON = true;
            MusicSource.Play();
            //AudioSrc.enabled = true;
            //GameAudioText.text = "audio ON";
            GameAudioButton.GetComponentInChildren<TMP_Text>().text = "audio ON";
            GameAudioButton.GetComponent<Image>().color = Color.green;
        }
    }

    public void SavePlayerSettings()
    {
        int volSlider = VolumeToSlider;
        float volLvl = VolumeLvl;
        PlayerPrefs.SetInt("volumeToSlider", volSlider);
        PlayerPrefs.SetFloat("volumeLvl", volLvl);
        PlayerPrefs.Save();
        Debug.Log("saved date");
    }
    public void LoadPlayerSettings()
    {
        if (PlayerPrefs.HasKey("volumeToSlider"))
        {
            int volSlider = PlayerPrefs.GetInt("volumeToSlider");
            VolumeToSlider = volSlider;
        }
        if (PlayerPrefs.HasKey("volumeLvl"))
        {
            float volLvl = PlayerPrefs.GetFloat("volumeLvl");
            VolumeLvl = volLvl;

        }
        Debug.Log("loaded data");
    }

    void Update()
    {
        
    }

    //public void MenuAudioONOFF()
    //{
    //    if (MenuAudioON)
    //    {
    //        MenuAudioON = false;
    //        //AudioText.text = "audio OFF";
    //        BackgroundAudio.Stop();
    //        AudioSrc.enabled = false;
    //        MenuAudioButton.GetComponentInChildren<TMP_Text>().text = "audio OFF";
    //        MenuAudioButton.GetComponent<Image>().color = Color.red;
    //    }
    //    else
    //    {
    //        MenuAudioON = true;
    //        BackgroundAudio.Play();
    //        AudioSrc.enabled = true;
    //        MenuAudioButton.GetComponentInChildren<TMP_Text>().text = "audio ON";
    //        MenuAudioButton.GetComponent<Image>().color = Color.green;

    //    }
    //}
}
