using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    #region <VARIABLES>

    public static SoundManager instance;

    //MUSIC AUDIO SOURCES
    public static AudioSource MusicSource;

    //MUSIC VOLUME VARIABLES
    private int volumeToSlider;
    public int VolumeToSlider { get; set; }

    private float volumeLvl;
    public float VolumeLvl { get; set; }

    //INGAME MUSIC CONTROL
    public bool inGameMusicAudioON;
    //public int volSlider = 100;

    //AUDIO CLIPS
    [SerializeField] private AudioClip menuClip, gameClip;
    //public static AudioClip gameClip, menuClip;

    #endregion

    #region <INIT>

    void Awake()
    {
        if (SoundManager.instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            ///TO FIX: METTE 0 LA PRIMA VOLTA ALL'AVVIO
            ///
            //volumeToSlider = 100;
            //volumeLvl = 1f;
            inGameMusicAudioON = true;
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

        MusicSource = GetComponent<AudioSource>();
    }
    #endregion

    #region <PLAY MUSIC SOUNDS>

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

    #region <MUSIC VOLUME UPDATE>

    public void UpdateVolume(float value)
    {
        //Prende valore dallo slider e aggiorna la variabile che passa tra le scene

        //volume che appare sullo slider
        VolumeToSlider = (int)value;
        //volume effettivo tra 0 e 1 (float)
        VolumeLvl = value / 100f;
        MusicSource.volume = VolumeLvl;

        SFXsoundManager.instance.PlaySound("backClick");
    }
   
    #endregion

    #region <SAVE/LOAD VOLUME SETTINGS>

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
}
