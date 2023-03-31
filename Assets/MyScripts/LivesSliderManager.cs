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

    private void Awake()
    {
        instance = this;
        textStartingColor = livesSliderTxt.color;
        sliderStartingColor = sliderFiller.color;

        //sliderFiller=livesSlider.transform.GetChild(2).gameObject.transform.GetChild(0).gameObject.GetComponent<Image>();
    }
    // Start is called before the first frame update
    void Start()
    {
        livesSlider.onValueChanged.AddListener(UpdateText);
        livesSlider.value = MainMenu.InstanceMenu.LivesMax;
    }

    // Update is called once per frame
    private void UpdateText(float val)
    {
        SFXsoundManager.instance.PlaySound("backClick");
        if(livesSlider.value == 1)
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
        MainMenu.InstanceMenu.UpdateLives(val);
    }



}
