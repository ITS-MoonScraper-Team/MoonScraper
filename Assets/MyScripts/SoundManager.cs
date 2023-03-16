using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    //public Slider volumeSlider;
    public Button GameAudioButton;
    public Button MenuAudioButton;
    public TMP_Text GameAudioText;
    public static AudioSource MusicSource;
    //public AudioSource BackMenuAudio;
    public static AudioClip playerDeathSound, jetpack, landing, mainMenuOST;
    public static AudioSource AudioSrc;

    public static bool AudioON;
    public static bool MenuAudioON;
    public int volumeToSlider;
    public float volumeLvl;

    public static SoundManager instance;

    [SerializeField] private AudioClip menuClip, gameClip;

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
        //BackgroundAudio.playOnAwake=true;
    }

    // Start is called before the first frame update
    void Start()
    {
        AudioON = true;
        playerDeathSound = Resources.Load < AudioClip > ("playerDeath");
        //mainMenuOST = Resources.Load<AudioClip>("mainMenu_OST");
        MusicSource = GetComponent<AudioSource>();
        //AudioSrc.enabled = true;
        GameAudioText.text = "audio ON";
        GameAudioButton.image.color = Color.green;

        //AudioButton.GetComponentInChildren<TMP_Text>().text = "audio ON";
        //AudioButton.GetComponent<Image>().color = Color.green;
        //if (SceneManager.GetActiveScene().buildIndex == 0)

    }

    public static void PlaySound(string clip)
    {
        switch (clip)
        {
            case "playerDeath":
                MusicSource.PlayOneShot(playerDeathSound);
            break;
            case "mainMenu_OST":
                MusicSource.Play();
                break;
        }
    }
    public void AudioONOFF()
    {
        if (AudioON)
        {
            AudioON = false;
            //AudioText.text = "audio OFF";
            MusicSource.Stop();
            AudioSrc.enabled = false;
            //GameAudioText.text = "audio OFF";
            GameAudioButton.GetComponentInChildren<TMP_Text>().text = "audio OFF";
            GameAudioButton.GetComponent<Image>().color = Color.red;
        }
        else
        {
            AudioON = true;
            MusicSource.Play();
            AudioSrc.enabled = true;
            //GameAudioText.text = "audio ON";
            GameAudioButton.GetComponentInChildren<TMP_Text>().text = "audio ON";
            GameAudioButton.GetComponent<Image>().color = Color.green;
        }
    }

    public void UpdateVolume(float value) {

        ///prende valore dallo slider e assegna alla variabile che aggiorna in game

        //variabile che memorizza volume che appare sullo slider tra le scene (intero ma float)
        volumeToSlider = (int)value;

        volumeLvl = value / 100f;
        MusicSource.volume = volumeLvl;
    }

    public void PlayGameMusic() {
        MusicSource.clip = gameClip;
        MusicSource.Play();
        //
    }

    public void PlayMainMenuMusic() {
        MusicSource.clip = menuClip;
        MusicSource.Play();
    
    }

    public void SavePlayerSettings()
    {
        int volSlider = volumeToSlider;
        float volLvl = volumeLvl;
        PlayerPrefs.SetInt("volumeToSlider", volSlider);
        PlayerPrefs.SetFloat("volumeLvl", volLvl);
        PlayerPrefs.Save();
        Debug.Log("saved date");
    }
    public void LoadPlayerSettings()
    {
        if (PlayerPrefs.HasKey("volumeLvl"))
        {
            int volSlider = PlayerPrefs.GetInt("volumeToSlider");
            volumeToSlider = volSlider;
        }
        if (PlayerPrefs.HasKey("yAxisInverted"))
        {
            float volLvl = PlayerPrefs.GetFloat("volumeLvl");
            volumeLvl = volLvl;

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
