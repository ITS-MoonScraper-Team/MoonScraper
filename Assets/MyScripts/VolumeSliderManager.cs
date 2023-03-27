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

    private void Awake()
    {
        instance = this;
        textStartingColor = volumeSliderTxt.color;
        sliderStartingColor = sliderFiller.color;
    }
    void Start()
    {
        volumeSlider.onValueChanged.AddListener(UpdateVolumeText);
        //SISTEMO PERCHE' METTE 0 ALL'INIZIO
        volumeSlider.value = SoundManager.instance.VolumeToSlider;
    }

    private void UpdateVolumeText(float val)
    {
        SoundManager.instance.PlaySound("backClick");
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
}
