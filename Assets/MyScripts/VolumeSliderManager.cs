using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class VolumeSliderManager : MonoBehaviour
{
    public Slider volumeSlider;
    public TMP_Text volumeSliderTxt;
    private Color textStartingColor;
    private Color sliderStartingColor;
    public Image sliderFiller;
    public static VolumeSliderManager instance;

    private void Awake()
    {
        instance = this;
        textStartingColor = volumeSliderTxt.color;
        sliderStartingColor = sliderFiller.color;
    }
    void Start()
    {
        UpdateVolumeText(volumeSlider.value);
        volumeSlider.onValueChanged.AddListener(UpdateVolumeText);
    }

    private void UpdateVolumeText(float val)
    {

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
    }
}
