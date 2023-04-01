using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    #region ||>> VARIABLES <<||

    public static SoundManager instance;
    public Button GameAudioButton;
    public TMP_Text GameAudioText;
    public static AudioSource MusicSource;
    //public static AudioClip playerDeathSound, jetpack, landing, mainMenuOST;
    public static bool AudioON;

    private int volumeToSlider;
    public int VolumeToSlider { get; set; }

    private float volumeLvl;
    public float VolumeLvl { get; set; }

   

    //AUDIO CLIPS
    [SerializeField] private AudioClip menuClip, gameClip/*, playerDeathClip,okClick, backClick, jetpackPropulsion*/;

    #endregion

    #region ||>> INIT <<||

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
        //LOAD RESOURCES DRIVER METHOD
        //playerDeathSound = Resources.Load < AudioClip > ("playerDeath");
        //mainMenuOST = Resources.Load<AudioClip>("mainMenu_OST");
        //AudioSrc.enabled = true;

        AudioON = true;

        MusicSource = GetComponent<AudioSource>();
        if (GameAudioButton != null)
        {
            GameAudioText.text = "audio ON";
            GameAudioButton.image.color = Color.green;
        }
    }
    #endregion

    #region ||>> PLAY MUSIC SOUNDS <<||

    public void PlaySound(string clip)
    {
        switch (clip)
        {
            case "mainMenu_OST":
                MusicSource.clip =menuClip;
                MusicSource.Play();
                break;
            case "inGame_OST":
                MusicSource.clip = gameClip;
                MusicSource.Play();
                break;
        }
    }
    #endregion

    #region ||>> UPDATE VOLUME SLIDER <<||

    public void UpdateVolume(float value)
    {

        //prende valore dallo slider e aggiorna la variabile che passa tra le scene

        //volume che appare sullo slider
        VolumeToSlider = (int)value;

        //volume effettivo tra 0 e 1 (float)
        VolumeLvl = value / 100f;
        MusicSource.volume = VolumeLvl;

        SFXsoundManager.instance.PlaySound("backClick");
    }
   
    #endregion

    #region ||>> AUDIO BUTTON ON/OFF <<||

    public void AudioONOFF()
    {
        if (AudioON)
        {
            //MusicSource.enabled = false;
            AudioON = false;
            MusicSource.Stop();
            //GameAudioButton.GetComponentInChildren<TMP_Text>().text = "audio OFF";
            GameAudioButton.GetComponent<Image>().color = Color.red;
        }
        else
        {
            //MusicSource.enabled = true;
            AudioON = true;
            MusicSource.Play();
            //GameAudioButton.GetComponentInChildren<TMP_Text>().text = "audio ON";
            GameAudioButton.GetComponent<Image>().color = Color.green;
        }
    }
    #endregion

    #region ||>> SAVE/LOAD VOLUME SETTINGS <<||

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
    #endregion

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
