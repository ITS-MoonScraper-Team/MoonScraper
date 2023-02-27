using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public Button AudioButton;
    public TMP_Text AudioText;
    public static bool AudioON;
    public static AudioClip jetpack, playerDeathSound, landing;
    static AudioSource AudioSrc;
    public AudioSource BackgroundAudio;
    public static SoundManager instance;
    void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        AudioON = true;
        playerDeathSound = Resources.Load < AudioClip > ("playerDeath");
        AudioSrc = GetComponent<AudioSource>();
        AudioSrc.enabled = true;
        AudioButton.GetComponentInChildren<TMP_Text>().text = "audio ON";
        AudioButton.GetComponent<Image>().color = Color.green;
    }

    public static void PlaySound(string clip)
    {
        switch (clip)
        {
            case "playerDeath":
            AudioSrc.PlayOneShot(playerDeathSound);
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
            AudioON=true;
            BackgroundAudio.Play();
            AudioSrc.enabled = true;
            AudioButton.GetComponentInChildren<TMP_Text>().text = "audio ON";
           AudioButton.GetComponent<Image>().color = Color.green;

        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
