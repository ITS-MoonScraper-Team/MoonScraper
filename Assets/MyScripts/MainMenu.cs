using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;
using TMPro;

public class MainMenu : MonoBehaviour
{
    public static MainMenu InstanceMenu;

    public Toggle xAxisToggle;
    public Toggle yAxisToggle;
    public TMP_Text lifeMeterTXT;
    public Slider maxLivesSlider;
    //public TMP_Text title12;
    //public TMP_Text title2;
    //public Material titleMat;
    private int livesMax;
    public int LivesMax
    { get; set; }

    private void Awake()
    {
        InstanceMenu = this;
    }
    public void PlayGame()
    {
        //get Scenes to play from builder
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1/*,parameterers */);
    }
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("QUIT!");
    }
    private void Start()
    {
        
    }

    void Update()
    {
        //font lightining angle modifica
        //Debug.Log (lifeMeterTXT.fontMaterial.shader.GetPropertyAttributes(4));
        LivesMax = (int)maxLivesSlider.value;
        if (LivesMax == 11)
        { lifeMeterTXT.text = "eternal"; }
        else
        {
            lifeMeterTXT.text = LivesMax.ToString();
        }
        Debug.Log($"VITE {LivesMax}");
    }

    public void SetXaxisValue(bool xAxisInverted)
    {
        AxisOrientation.instance.XAxisInverted = xAxisInverted;
    }
    public void SetYaxisValue(bool yAxisInverted)
    {
        AxisOrientation.instance.YAxisInverted = yAxisInverted;
    }
    public void SetToggles()
    {
        xAxisToggle.isOn = AxisOrientation.instance.XAxisInverted;
        yAxisToggle.isOn = AxisOrientation.instance.YAxisInverted;
    }
    public void SavePlayerSettingsOnBack()
    {
        AxisOrientation.instance.SavePlayerSettings();
    }
}
