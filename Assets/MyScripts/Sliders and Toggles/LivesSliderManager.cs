using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LivesSliderManager : MonoBehaviour
{
    #region <VARIABLES>

    public static LivesSliderManager instance;

    public Slider livesSlider;
    public TMP_Text livesSliderTxt;
    public Image sliderFiller;
    private Color textStartingColor;
    private Color sliderStartingColor;
    public int firstTimeStart = 0;

    #endregion

    #region <INIT>

    private void Awake()
    {
        LoadPlayerSettings();
        instance = this;
        textStartingColor = livesSliderTxt.color;
        sliderStartingColor = sliderFiller.color;
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
    #endregion

    #region <<LIVES SLIDER UPDATES>>

    #region <SET LIVES SLIDER>
    private void SetLivesSlider(float val)
    {
        MainMenu.InstanceMenu.UpdateLives(val);
        UpdateSliderText(val);
    }
    #endregion

    #region <PLAY SLIDER SFX>
    private void SFXLivesSLider(float val)
    {
        SFXsoundManager.instance.PlaySettingsSFXSound();
    }
    #endregion

    #region <SET SLIDER TEXT>
    private void UpdateSliderText(float val)
    {
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
    }
    #endregion

    #endregion

    #region <FIRST START CHECK SAVE>

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
