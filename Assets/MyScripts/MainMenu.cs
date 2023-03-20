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
    //public Slider LivesSlider;
    //private string animationName;
    public Material titleMat;
    public Shader titleShader;
    public bool menuAudioON;
    //public float volumeLvl;
    //public int volumeIntLvl;

    private bool lightAngleLooping = false;
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
        LoadPlayerSettings();
        //BackgroundAudio.playOnAwake=true;
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
        //SoundManager.instance.
        //LivesSliderManager.instance.livesSlider.onValueChanged.AddListener(UpdateMaxLives);
        SoundManager.instance.PlaySound("mainMenu_OST");
        //SoundManager.instance.PlayMainMenuMusic();
    }

    public void UpdateLives(float value)
    {
        //prende valore dallo slider e assegna alla variabile che aggiorna in game
        LivesMax = (int)value;
        //LivesMax = (int)LivesSliderManager.instance.livesSlider.value;

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
        //if (xAxisToggle == null) return;

        xAxisToggle.isOn = AxisOrientation.instance.XAxisInverted;
        yAxisToggle.isOn = AxisOrientation.instance.YAxisInverted;
    }
    public void SavePlayerSettingsOnBack()
    {
        //SaveLoadPrefs.instance.SavePlayerLivesSettings();
        //SaveLoadPrefs.instance.SavePlayerAxisSettings();
        //SaveLoadPrefs.instance.SavePlayerVolumeSettings();

        SavePlayerSettings();
        AxisOrientation.instance.SavePlayerSettings();
        SoundManager.instance.SavePlayerSettings();
    }


    public void SavePlayerSettings()
    {
        int livesMaxSet = LivesMax;
        PlayerPrefs.SetInt("LivesMax", livesMaxSet);
        PlayerPrefs.Save();
        Debug.Log("saved date");
    }
    public void LoadPlayerSettings()
    {
        if (PlayerPrefs.HasKey("LivesMax"))
        {
            int livesMaxSet = PlayerPrefs.GetInt("LivesMax");
            LivesMax = livesMaxSet;
        }
       
        Debug.Log("loaded data");
    }

    //IEnumerator LightAngleVariationCoroutine()
    //{
    //    lightAngleLooping = true;
    //    //titleMat.SetFloat("_LightAngle", Random.Range(.1f, 6f));

    //    titleMat.DOFloat(1f, "_LightAngle", 1.2f);
    //    yield return new WaitForSeconds(.2f);

    //    titleMat.DOFloat(5.2f, "_LightAngle", .6f);
    //    yield return new WaitForSeconds(.2f);

        
    //    //titleMat.SetFloat("_LightAngle", 6.2f); /*(6.25f, "_LightAngle", 1.2f)*/
    //    //yield return new WaitForSeconds(.2f);

    //    //titleMat.SetFloat("_LightAngle", 5.2f); //(5.2f, "_LightAngle", 1.2f);
    //    //yield return new WaitForSeconds(.2f);

    //    //titleMat.SetFloat("_LightAngle", 1f); //7(1f, "_LightAngle", .6f);
    //    //yield return new WaitForSeconds(.2f);

    //    //titleMat.SetFloat("_LightAngle", 0f); //(0f, "_LightAngle", 1.2f);
    //    //yield return new WaitForSeconds(.2f);

    //    lightAngleLooping = false;

    //    //yield return null;
    //}

    void Update()
    {
        //titleShader.GetComponent<Renderer>().sharedMaterial.SetFloat("_LightAngle", Random.Range(.1f, 6f ));
        //titleMat.shader = Shader.Find("TextMeshPro/Distance Field");

        //FUNZIA!!
        titleMat.SetFloat("_LightAngle", Random.Range(.1f, 6f));
        
        //float lightAngleLoop=3f;
        //float lightAngleTimer=0;
        //lightAngleTimer+= Time.deltaTime;
        if(lightAngleLooping==false)
        {
            //lightAngleLooping = true;
            //StartCoroutine(LightAngleVariationCoroutine());
            //CALL COROUTINE ANGLE VARIATION
            //nella coroutine setta lightAngleLooping false
        }
        else
        { }


        //font material shader - lightining angle modifica--> PROPRIETIES INDEX (16)
        string[] shaderz = new string[10];
        for (int i = 0; i < 10; i++)
        {
            shaderz[i]= titleMat.shader.GetPropertyName(i+10);

            //Debug.LogError($"shader prop {shaderz[i]}");
        }
        Debug.LogWarning($"LIGHT ANGLE {shaderz[6]}");


        //prende valore dallo slider e assegna alla variabile che aggiorna in game
        ///LivesMax = (int)LivesSliderManager.instance.livesSlider.value;
        //prende valore dallo slider e assegna alla variabile che aggiorna in game
        //METTO NEL SOUND MANAGER-->provato ma non aggiorna lo slder quando torna al main menu
        ///volumeLvl = VolumeSliderManager.instance.volumeSlider.value / 100f;
        ///volumeIntLvl = (int)VolumeSliderManager.instance.volumeSlider.value;
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
}
