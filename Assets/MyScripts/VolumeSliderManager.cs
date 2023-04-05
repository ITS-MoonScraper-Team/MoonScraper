using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class VolumeSliderManager : MonoBehaviour
{
    public static VolumeSliderManager instance;
    
    public Slider volumeSlider;
    public TMP_Text volumeSliderTxt;
    public Image sliderFiller;
    private Color textStartingColor;
    private Color sliderStartingColor;

    public Slider SFXvolumeSlider;
    public TMP_Text SFXvolumeSliderTxt;
    public Image SFXsliderFiller;
    private Color SFXtextStartingColor;
    private Color SFXsliderStartingColor;

    private void Awake()
    {
        instance = this;
        textStartingColor = volumeSliderTxt.color;
        sliderStartingColor = sliderFiller.color;
        SFXtextStartingColor = SFXvolumeSliderTxt.color;
        SFXsliderStartingColor = SFXsliderFiller.color;
    }
    void Start()
    {
        ///TO FIX: RIPRODUCE RUMORE BACKCLICK ALLO START
        ///TO FIX: METTE 0 LA PRIMA VOLTA ALL'AVVIO
        volumeSlider.onValueChanged.AddListener(UpdateVolumeText);
        SFXvolumeSlider.onValueChanged.AddListener(UpdateSFXVolumeText);
        volumeSlider.value = SoundManager.instance.VolumeToSlider;
        SFXvolumeSlider.value = SFXsoundManager.instance.SFXVolumeToSlider;
    }

    private void UpdateVolumeText(float val)
    {
        //SFXsoundManager.instance.PlaySound("backClick");
        if (volumeSlider.value == 0)
        {
            volumeSliderTxt.text = "OFF";
            //volumeSliderTxt.fontSize = 60;
            volumeSliderTxt.color = Color.white;

            sliderFiller.color = Color.white;
        }
        else if (volumeSlider.value == 100)
        {
            volumeSliderTxt.text = "MAX";
            volumeSliderTxt.color = Color.red;
            sliderFiller.color = Color.red;
        }
        else
        {
            volumeSliderTxt.text = volumeSlider.value.ToString();
            volumeSliderTxt.color = Color.yellow;
            sliderFiller.color = Color.yellow;
        }
        SoundManager.instance.UpdateVolume(val);
    }

    private void UpdateSFXVolumeText(float val)
    {
        //SFXsoundManager.instance.PlaySound("backClick");
        if (SFXvolumeSlider.value == 0)
        {
            SFXvolumeSliderTxt.text = "OFF";
            //volumeSliderTxt.fontSize = 60;
            SFXvolumeSliderTxt.color = Color.white;

            SFXsliderFiller.color = Color.white;
        }
        else if (SFXvolumeSlider.value == 100)
        {
            SFXvolumeSliderTxt.text = "MAX";
            SFXvolumeSliderTxt.color = Color.red;
            SFXsliderFiller.color = Color.red;
        }
        else
        {
            SFXvolumeSliderTxt.text = SFXvolumeSlider.value.ToString();
            SFXvolumeSliderTxt.color = Color.yellow;
            SFXsliderFiller.color = Color.yellow;
        }
        SFXsoundManager.instance.UpdateSFXVolume(val);
    }
}
