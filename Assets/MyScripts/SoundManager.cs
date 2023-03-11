using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    //public Slider volumeSlider;
    public Button AudioButton;
    public TMP_Text AudioText;
    public static bool AudioON;
    public static bool MenuAudioON;
    public static AudioClip jetpack, playerDeathSound, landing, mainMenuOST;
    public static AudioSource AudioSrc;
    public AudioSource BackgroundAudio;
    //public AudioSource BackMenuAudio;
    public Button MenuAudioButton;
    public int volumeIntLvl;
    public float volumeLvl;
    public static SoundManager instance;
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
        //BackgroundAudio.playOnAwake=true;
    }

    // Start is called before the first frame update
    void Start()
    {
        AudioON = true;
        playerDeathSound = Resources.Load < AudioClip > ("playerDeath");
        mainMenuOST = Resources.Load<AudioClip>("mainMenu_OST");
        AudioSrc = GetComponent<AudioSource>();
        AudioSrc.enabled = true;
        AudioText.text = "audio ON";
        //AudioButton.GetComponentInChildren<TMP_Text>().text = "audio ON";
        //AudioButton.GetComponent<Image>().color = Color.green;
        AudioButton.image.color = Color.green;

        //if (SceneManager.GetActiveScene().buildIndex == 0)

    }

    public static void PlaySound(string clip)
    {
        switch (clip)
        {
            case "playerDeath":
            AudioSrc.PlayOneShot(playerDeathSound);
            break;
            case "mainMenu_OST":
            AudioSrc.Play();
                break;
                
        }
    }
    public void AudioONOFF()
    {
        if (AudioON)
        {
            AudioON = false;
            //AudioText.text = "audio OFF";
            BackgroundAudio.Stop();
            AudioSrc.enabled = false;
            AudioButton.GetComponentInChildren<TMP_Text>().text = "audio OFF";
            AudioButton.GetComponent<Image>().color = Color.red;
        }
        else
        {
            AudioON = true;
            BackgroundAudio.Play();
            AudioSrc.enabled = true;
            AudioButton.GetComponentInChildren<TMP_Text>().text = "audio ON";
            AudioButton.GetComponent<Image>().color = Color.green;

        }
        
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
    // Update is called once per frame
    void Update()
    {
        //prende valore dallo slider e assegna alla variabile che aggiorna in game

        volumeLvl= VolumeSliderManager.instance.volumeSlider.value / 100;
        volumeIntLvl = (int)VolumeSliderManager.instance.volumeSlider.value;
        BackgroundAudio.volume = volumeLvl;
        //BackgroundAudio.volume = GameMenu.instance.ingameVolumeSlider.value/100;
        //volumeLvl = (int)GameMenu.instance.ingameVolumeSlider.value;



        //aggiorna text dell'indicatore in game


        //if (BackgroundAudio.volume == 1)
        //{ volumeLvltext.text = "max"; }
        //else
        //{
        //    volumeLvltext.text = volumeLvl.ToString();
        //}
        //Debug.Log($"VITE {volumeLvl}");
    }
}
