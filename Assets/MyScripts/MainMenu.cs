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
    public static MainMenu InstanceMenu;

    public Animator menuAnimator;
    public Toggle xAxisToggle;
    public Toggle yAxisToggle;
    public TMP_Text lifeMeterTXT;
    public Material titleMat;
    public bool menuAudioON;
    //public Slider LivesSlider;
    //public float volumeLvl;
    //public int volumeIntLvl;

    //private string animationName;
    //private bool lightAngleLooping = false;

    private int livesMax;
    public int LivesMax
    { get; set; }

    private void Awake()
    {
        if (MainMenu.InstanceMenu != null)
        {
            Destroy(gameObject);
        }
        else
        {
            InstanceMenu = this;
            DontDestroyOnLoad(this);
        }
        LoadPlayerLivesSettings();
    }

    private void Start()
    {
        SoundManager.instance.PlaySound("mainMenu_OST");
        StartCoroutine(LightAngleVariationCoroutine());
    }

    #region <<<GAME BUTTON FUNCTIONS>>>

    public void PlayGame()
    {
        //animationName = "PlayCircleAniamtion";
        SFXsoundManager.instance.PlaySound("okClick");
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
    #endregion

    #region <<<BUOTTON SFX PLAY FUNCTIONS>>>

    public void PlayOptionsButtonSound()
    {
        SFXsoundManager.instance.PlaySound("okClick");
    }
    public void PlayBackButtonSound()
    {
        SFXsoundManager.instance.PlaySound("backClick");
    }
    #endregion

    /// <fix>
    ///suona anche allo start..BOH!!
    /// </fix>
    #region <<<AXIS TOGGLES>>> 

    public void SetXaxisValue(bool xAxisInverted)
    {
        ///suona anche allo start..BOH!!
        //PlayBackButtonSound();
        AxisOrientation.instance.XAxisInverted = xAxisInverted;

    }
    public void SetYaxisValue(bool yAxisInverted)
    {
        //PlayBackButtonSound();
        AxisOrientation.instance.YAxisInverted = yAxisInverted;
    }
    public void SetToggles()
    {
        //if (xAxisToggle == null) return;

        xAxisToggle.isOn = AxisOrientation.instance.XAxisInverted;
        yAxisToggle.isOn = AxisOrientation.instance.YAxisInverted;
    }
    #endregion

    /// <fix>
    /// save generale non funziona
    /// </fix>
    #region <<<SAVE ALL>>>

    public void SavePlayerSettingsOnBack()
    {
        //SaveLoadPrefs.instance.SavePlayerLivesSettings();
        //SaveLoadPrefs.instance.SavePlayerAxisSettings();
        //SaveLoadPrefs.instance.SavePlayerVolumeSettings();

        SavePlayerLivesSettings();
        AxisOrientation.instance.SavePlayerSettings();
        SoundManager.instance.SavePlayerSettings();
    }
    #endregion

    #region <<<UPDATE-SAVE-LOAD LIVES>>>

    public void UpdateLives(float value)
    {
        //prende valore dallo slider e aggiorna la variabile che passa le vite in game
        LivesMax = (int)value;
    }

    public void SavePlayerLivesSettings()
    {
        int livesMaxSet = LivesMax;
        PlayerPrefs.SetInt("LivesMax", livesMaxSet);
        PlayerPrefs.Save();
        Debug.Log("saved date");
    }

    public void LoadPlayerLivesSettings()
    {
        if (PlayerPrefs.HasKey("LivesMax"))
        {
            int livesMaxSet = PlayerPrefs.GetInt("LivesMax");
            LivesMax = livesMaxSet;
        }
       
        Debug.Log("loaded data");
    }
    #endregion

    #region <<Title Effects Coroutine>>
    IEnumerator LightAngleVariationCoroutine()
    {
        while (true)
        {
            //lightAngleLooping = true;
            //titleMat.SetFloat("_LightAngle", Random.Range(.1f, 6f));

            titleMat.DOFloat(1f, "_LightAngle", 1.2f);
            yield return new WaitForSeconds(.2f);

            titleMat.DOFloat(5.2f, "_LightAngle", .6f);
            yield return new WaitForSeconds(.2f);
        }

        //lightAngleLooping = false;
    }
    #endregion

    void Update()
    {
        #region <DEBUG>
        //FUNZIA!!
        //titleMat.SetFloat("_LightAngle", Random.Range(.1f, 6f));

        //font material shader - lightining angle modifica--> PROPRIETIES INDEX (16)
        string[] shaderz = new string[10];
        for (int i = 0; i < 10; i++)
        {
            shaderz[i]= titleMat.shader.GetPropertyName(i+10);

            //Debug.LogError($"shader prop {shaderz[i]}");
        }
        Debug.LogWarning($"LIGHT ANGLE {shaderz[6]}");

        //prende valore dallo slider e assegna alla variabile che aggiorna in game
        /// LivesMax = (int)LivesSliderManager.instance.livesSlider.value;
        /// volumeLvl = VolumeSliderManager.instance.volumeSlider.value / 100f;
        /// volumeIntLvl = (int)VolumeSliderManager.instance.volumeSlider.value;
        #endregion
    }
}
