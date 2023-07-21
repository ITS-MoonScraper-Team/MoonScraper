using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;
using TMPro;
using DG.Tweening;
using TMPro.Examples;

public class MainMenu : MonoBehaviour
{
    #region <<VARIABLES>>

    public static MainMenu InstanceMenu;

    public Button playButton;
    public Animator menuAnimator;
    public Toggle xAxisToggle;
    public Toggle yAxisToggle;
    //public TMP_Text lifeMeterTXT;
    public Material titleMat;
    public bool menuAudioON;
    public static bool easyMode = false;


    public Slider livesSlider;
    public TMP_Text livesSliderTxt;
    public Image sliderFiller;
    private Color textStartingColor;
    private Color sliderStartingColor;
    public int firstTimeStart = 0;

    //public float volumeLvl;
    //public int volumeIntLvl;
    /*
    private int livesMax;
    public int LivesMax => livesMax;
    { 
        get
        { return livesMax; }
        //set
        //{ livesMax = value; }
    }
    */

    [SerializeField] private int minPlatformToSpawnShooter = 5;
    public int MinPlatformToSpawnShooter=>minPlatformToSpawnShooter;


    #endregion

    #region <<INIT>>

    private void Awake()
    {
        //MainMenu.InstanceMenu = this;
        LoadPlayerSettings();

        if (MainMenu.InstanceMenu != null)
        {
            Destroy(gameObject);
        }
        else
        {
            InstanceMenu = this;
            DontDestroyOnLoad(this);
        }
        
    }

    private void Start()
    {
        //Screen.SetResolution(1080, 1920, true);
        textStartingColor = livesSliderTxt.color;
        sliderStartingColor = sliderFiller.color;

        LivesSliderManager.instance.LoadPlayerLivesSettings();
        SoundManager.instance.PlaySound("mainMenu_OST");

        livesSlider.onValueChanged.AddListener(SetLivesSlider);
        if (firstTimeStart == 0)
        {
            livesSlider.value = 11f;
            firstTimeStart++;
        }
        else
        {
            livesSlider.value = LivesSliderManager.instance.LivesMax;
        }
        livesSlider.onValueChanged.AddListener(SFXLivesSLider);

        SavePlayerSettings();
    }

    //ACTIVATE UI COLOR-CHANGE COROUTINES
    private void OnEnable()
    {
        StartCoroutine(LightAngleVariationCoroutine());
    }
    #endregion

    #region <<GAME BUTTONS FUNCTIONS>>

    public void PlayGame()
    {
        //animationName = "PlayCircleAniamtion";
        SFXsoundManager.instance.PlayOKButtonSFXSound();
        menuAnimator.enabled=true;
        Invoke("LoadScene", .5f);
    }
    private void LoadScene()
    {
        //get scenes to play from build index
        menuAnimator.enabled = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1/*,parameterers */);
    }
    public void OpenOptions()
    {
        SFXsoundManager.instance.PlayOKButtonSFXSound();
        SetToggles();
    }
    public void BackFromOptions()
    {
        SFXsoundManager.instance.PlayOKButtonSFXSound();
        SavePlayerSettingsOnBackFromOptions();

    }
    public void QuitGame()
    {
        SFXsoundManager.instance.PlayOKButtonSFXSound();
        Application.Quit();
        Debug.Log("QUIT!");
    }
    #endregion

    #region <<AXIS TOGGLES>> 

    //SETTA VALORE DEI TOGGLES ALLO SCRIPT ASSI QUANDO CI INTERAGISCI
    public void SetXaxisValue(bool xAxisInverted)
    {
        SFXsoundManager.instance?.PlaySettingsSFXSound();

        if(AxisOrientation.instance!=null)
        AxisOrientation.instance.XAxisInverted = xAxisInverted;

        if (xAxisInverted)
            xAxisToggle.image.color = Color.green;
        else
            xAxisToggle.image.color = Color.red;
    }
    public void SetYaxisValue(bool yAxisInverted)
    {
        SFXsoundManager.instance?.PlaySettingsSFXSound();

        if (AxisOrientation.instance != null)
            AxisOrientation.instance.YAxisInverted = yAxisInverted;

        if (yAxisInverted)
            yAxisToggle.image.color = Color.green;
        else
            yAxisToggle.image.color = Color.red;
    }

    //SETTA VALORE DELLO SCRIPT ASSI AI TOGGLES QUANDO APRI OPZIONI
    public void SetToggles()
    {
        //if (xAxisToggle == null) return;
        xAxisToggle.isOn = AxisOrientation.instance.XAxisInverted;
        yAxisToggle.isOn = AxisOrientation.instance.YAxisInverted;
    }
    #endregion

    #region <<LIVES SLIDER UPDATES>>

    #region <SET LIVES SLIDER>
    private void SetLivesSlider(float val)
    {
        LivesSliderManager.instance.UpdateLives(val);
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
        else if (livesSlider.value == 11)
        {
            livesSliderTxt.text = "ETERNAL";
            livesSliderTxt.color = textStartingColor;
            sliderFiller.color = sliderStartingColor;
        }
        else
        {
            livesSliderTxt.text = livesSlider.value.ToString();
            livesSliderTxt.color = Color.yellow;
            sliderFiller.color = sliderStartingColor;
        }
    }
    #endregion

    #endregion

    #region <<UI EFFECTS COROUTINE>>

    IEnumerator LightAngleVariationCoroutine()
    {
        while (true)
        {
            //titleMat.SetFloat("_LightAngle", Random.Range(.1f, 6f));

            titleMat.DOFloat(1f, "_LightAngle", 1.2f);
            yield return new WaitForSeconds(.2f);

            titleMat.DOFloat(5.2f, "_LightAngle", .6f);
            yield return new WaitForSeconds(.2f);
        }
    }

    IEnumerator ColorChangeCoroutine()
    {
        //PLAY BUTTON HEART PULSATION
        while (true)
        {
            playButton.image.DOColor(new Color(0.85f, 0.7f, 0.7f), .75f);
            yield return new WaitForSeconds(.2f);
            playButton.image.DOColor(Color.white, .45f);
            yield return new WaitForSeconds(.2f);
            playButton.image.DOColor(new Color(0.85f, 0.7f, 0.7f), .45f);
            yield return new WaitForSeconds(.2f);
            playButton.image.DOColor(Color.white, 1.15f);
            yield return new WaitForSeconds(.8f);
        }
    }
    #endregion

    #region <<<SAVE ALL>>>

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

    ///TO FIX: save generale non funziona
    public void SavePlayerSettingsOnBackFromOptions()
    {
        //SaveLoadPrefs.instance.SavePlayerLivesSettings();
        //SaveLoadPrefs.instance.SavePlayerAxisSettings();
        //SaveLoadPrefs.instance.SavePlayerVolumeSettings();

        LivesSliderManager.instance.SavePlayerLivesSettings();
        AxisOrientation.instance.SavePlayerSettings();
        SoundManager.instance.SavePlayerSettings();
        SFXsoundManager.instance.SavePlayerSettings();
        
        SavePlayerSettings();
    }
    #endregion

    void Update()
    {
        #region <DEBUG>

        //FUNZIONA
        //titleMat.SetFloat("_LightAngle", Random.Range(.1f, 6f));

        //font material shader - modifica lightining angle --> PROPRIETIES INDEX (16)
        //string[] shaderz = new string[10];
        //for (int i = 0; i < 10; i++)
        //{
        //    shaderz[i]= titleMat.shader.GetPropertyName(i+10);

        //    //Debug.LogError($"shader prop {shaderz[i]}");
        //}
        //Debug.LogWarning($"LIGHT ANGLE {shaderz[6]}");
        #endregion
    }
}
