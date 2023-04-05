using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class VolumeSliderManager : MonoBehaviour
{
    #region <VARIABLES>

    public static VolumeSliderManager instance;
    
    public Slider volumeSlider;
    public TMP_Text volumeSliderTxt;
    public Image sliderFiller;

    public Slider SFXvolumeSlider;
    public TMP_Text SFXvolumeSliderTxt;
    public Image SFXsliderFiller;

    private int firstTimeStart = 0;
    #endregion

    #region <INIT>

    private void Awake()
    {
        Debug.LogWarning($"preload {firstTimeStart}");
        LoadPlayerSettings();
        Debug.LogWarning($"post {firstTimeStart}");

        instance = this;
    }
    void Start()
    {
        ///TO FIX: RIPRODUCE RUMORE BACKCLICK ALLO START
        ///FIXED(?): METTE 0 LA PRIMA VOLTA ALL'AVVIO
        volumeSlider.onValueChanged.AddListener(MusicVolumeSliderUpdate);
        SFXvolumeSlider.onValueChanged.AddListener(SFXVolumeSliderUpdate);

        if (firstTimeStart == 0)
        {
            volumeSlider.value = 100f;
            SFXvolumeSlider.value = 100f;
            firstTimeStart++;
        }
        else
        {
            volumeSlider.value = SoundManager.instance.VolumeToSlider;
            SFXvolumeSlider.value = SFXsoundManager.instance.SFXVolumeToSlider;
        }

        SavePlayerSettings();
    }
    #endregion

    #region <VOLUME SLIDERS UPDATES>

    private void MusicVolumeSliderUpdate(float val)
    {
        SoundManager.instance.UpdateVolume(val);
        UpdateSliderText(val, volumeSlider, sliderFiller);
    }
    private void SFXVolumeSliderUpdate(float val)
    {
        SFXsoundManager.instance.UpdateSFXVolume(val);
        UpdateSliderText(val, SFXvolumeSlider, SFXsliderFiller);
    }

    private void UpdateSliderText(float val, Slider slide, Image fill)
    {
        if (slide.value == 0)
        {
            slide.GetComponentInChildren<TMP_Text>().text = "OFF";
            //volumeSliderTxt.fontSize = 60;
            slide.GetComponentInChildren<TMP_Text>().color = Color.white;
            fill.color = Color.white;
        }
        else if (slide.value == 100)
        {
            slide.GetComponentInChildren<TMP_Text>().text = "MAX";
            slide.GetComponentInChildren<TMP_Text>().color = Color.red;
            fill.color = Color.red;
        }
        else
        {
            slide.GetComponentInChildren<TMP_Text>().text = slide.value.ToString();
            slide.GetComponentInChildren<TMP_Text>().color = Color.yellow;
            fill.color = Color.yellow;
        }
    }
    #endregion

    #region <FIRST START CHECK>

    public void SavePlayerSettings()
    {
        int firstTime = firstTimeStart;
        
        PlayerPrefs.SetInt("firstTime", firstTime);
        PlayerPrefs.Save();
        Debug.Log("saved date");
    }

    public void LoadPlayerSettings()
    {
        if (PlayerPrefs.HasKey("firstTime"))
        {
            int firstTime = PlayerPrefs.GetInt("firstTime");
            firstTimeStart = firstTime;
        }
        Debug.Log("loaded data");
    }
    #endregion

    private void OnDestroy()
    {
    }

    //private void UpdateSFXVolumeText(float val)
    //{
    //    if (SFXvolumeSlider.value == 0)
    //    {
    //        SFXvolumeSliderTxt.text = "OFF";
    //        //volumeSliderTxt.fontSize = 60;
    //        SFXvolumeSliderTxt.color = Color.white;
    //        SFXsliderFiller.color = Color.white;
    //    }
    //    else if (SFXvolumeSlider.value == 100)
    //    {
    //        SFXvolumeSliderTxt.text = "MAX";
    //        SFXvolumeSliderTxt.color = Color.red;
    //        SFXsliderFiller.color = Color.red;
    //    }
    //    else
    //    {
    //        SFXvolumeSliderTxt.text = SFXvolumeSlider.value.ToString();
    //        SFXvolumeSliderTxt.color = Color.yellow;
    //        SFXsliderFiller.color = Color.yellow;
    //    }
    //}
}
