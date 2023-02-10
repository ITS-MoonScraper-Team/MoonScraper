using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LivesSliderManager : MonoBehaviour
{
    private Slider livesSlider;
    private TMP_Text livesTxt;
    private Color textStartingColor;
    private Color sliderStartingColor;
    private Image sliderFiller;

    private void Awake()
    {
        livesSlider = GetComponentInParent<Slider>();
        livesTxt = GetComponent<TMP_Text>();
        textStartingColor = livesTxt.color;
        sliderFiller=livesSlider.transform.GetChild(2).gameObject.transform.GetChild(0).gameObject.GetComponent<Image>();
        sliderStartingColor = sliderFiller.color;
    }
    // Start is called before the first frame update
    void Start()
    {
        UpdateText(livesSlider.value);
        livesSlider.onValueChanged.AddListener(UpdateText);
    }

    // Update is called once per frame
    void UpdateText(float val)
    {
        if(livesSlider.value == 1)
        {
            livesTxt.text = "HC";
            livesTxt.color = Color.red;
            sliderFiller.color = Color.red;
        }
        else if(livesSlider.value == 11)
        {
            livesTxt.text = "ETERNAL";
            livesTxt.color = textStartingColor;
            sliderFiller.color = sliderStartingColor;
        }
        else
        {
            livesTxt.text = livesSlider.value.ToString();
            livesTxt.color= Color.yellow;
            sliderFiller.color = sliderStartingColor;
        }
    }
}
