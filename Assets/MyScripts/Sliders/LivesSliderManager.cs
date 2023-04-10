using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LivesSliderManager : MonoBehaviour
{
    public static LivesSliderManager instance;
    public Slider livesSlider;
    public TMP_Text livesSliderTxt;
    public Image sliderFiller;
    private Color textStartingColor;
    private Color sliderStartingColor;

    public int firstTimeStart = 0;

    private void Awake()
    {
        LoadPlayerSettings();
        instance = this;
        textStartingColor = livesSliderTxt.color;
        sliderStartingColor = sliderFiller.color;

        //sliderFiller=livesSlider.transform.GetChild(2).gameObject.transform.GetChild(0).gameObject.GetComponent<Image>();
    }

    void Start()
    {
        livesSlider.onValueChanged.AddListener(SetLivesSlider);
        if (firstTimeStart == 0)
        {
            livesSlider.value = 11f;
            firstTimeStart++;
        }
        else
        {
            livesSlider.value = MainMenu.InstanceMenu.LivesMax;
        }
        livesSlider.onValueChanged.AddListener(SFXLivesSLider);

        SavePlayerSettings();
    }
    private void SetLivesSlider(float val)
    {
        MainMenu.InstanceMenu.UpdateLives(val);
        UpdateSliderText(val);

    }
    private void SFXLivesSLider(float val)
    {
        SFXsoundManager.instance.PlaySettingsSFXSound();

    }

    private void UpdateSliderText(float val)
    {
        //SFXsoundManager.instance.PlaySettingsSFXSound();

        if (livesSlider.value == 1)
        {
            livesSliderTxt.text = "HC";
            //livesSliderTxt.fontSize = 60;
            livesSliderTxt.color = Color.red;
            
            sliderFiller.color = Color.red;
        }
        else if(livesSlider.value == 11)
        {
            livesSliderTxt.text = "ETERNAL";
            livesSliderTxt.color = textStartingColor;
            sliderFiller.color = sliderStartingColor;
        }
        else
        {
            livesSliderTxt.text = livesSlider.value.ToString();
            livesSliderTxt.color= Color.yellow;
            sliderFiller.color = sliderStartingColor;
        }
        //MainMenu.InstanceMenu.UpdateLives(val);
    }

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

}
