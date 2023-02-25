using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public Button AudioButton;
    public TMP_Text AudioText;
    public static bool AudioON = true;
    public static AudioClip jetpack, playerDeathSound, landing;
    static AudioSource AudioSrc;
    // Start is called before the first frame update
    void Start()
    {
        playerDeathSound = Resources.Load < AudioClip > ("playerDeath");
        AudioSrc = GetComponent<AudioSource>();
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
        { AudioON = false;
          //AudioText.text = "audio OFF";
          AudioButton.GetComponentInChildren<TMP_Text>().text = "audio OFF";
            AudioButton.GetComponent<Image>().color = Color.red;
        }
        else
        { AudioON=true;
          AudioButton.GetComponentInChildren<TMP_Text>().text = "audio ON";
           AudioButton.GetComponent<Image>().color = Color.green;

        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
