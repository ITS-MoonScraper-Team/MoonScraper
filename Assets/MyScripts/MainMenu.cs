using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;
using TMPro;
using DG.Tweening;

public class MainMenu : MonoBehaviour
{
    public static MainMenu InstanceMenu;

    public Animator menuAnimator;
    public Toggle xAxisToggle;
    public Toggle yAxisToggle;
    public TMP_Text lifeMeterTXT;
    //public Slider LivesSlider;
    //private string animationName;
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
        //animationName = "PlayCircleAniamtion";
        menuAnimator.enabled=true;
        Invoke("LoadScene", .5f);
    }
    private void LoadScene()
    {
        //get Scenes to play from builder
        menuAnimator.enabled = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1/*,parameterers */);
    }
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("QUIT!");
    }
    private void Start()
    {
        //LivesSliderManager.instance.livesSlider.onValueChanged.AddListener(UpdateMaxLives);
    }

    //private void UpdateMaxLives(float val)
    //{
    //    LivesMax = (int)maxLivesSlider.value;
    //    //aggiorna text dell'indicatore in game
    //    if (LivesMax == 11)
    //    { lifeMeterTXT.text = "eternal"; }
    //    else
    //    {
    //        lifeMeterTXT.text = LivesMax.ToString();
    //    }
    //    Debug.Log($"VITE {LivesMax}");
    //}

    void Update()
    {
        //font lightining angle modifica
        //Debug.Log (lifeMeterTXT.fontMaterial.shader.GetPropertyAttributes(4));

        //prende valore dallo slider e assegna alla variabile che aggiorna in game
        LivesMax = (int)LivesSliderManager.instance.livesSlider.value;
        //aggiorna text dell'indicatore in game
        if (LivesMax == 11)
        { lifeMeterTXT.text = "eternal"; }
        else if(LivesMax==1)
        { lifeMeterTXT.text = "HARDCORE"; }
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
