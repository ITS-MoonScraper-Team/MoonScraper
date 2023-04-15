using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveLoadPrefs
{
    public static SaveLoadPrefs instance;

    private void Awake()
    {
        instance = this;
    }

    public void SaveAllPlayerSettings()
    {
        SavePlayerLivesSettings();
        SavePlayerAxisSettings();
        SavePlayerVolumeSettings();
    }

    //SAVE PLAYER SETTINGS
    public void SavePlayerLivesSettings()
    {
        int livesMaxSet = MainMenu.InstanceMenu.LivesMax;
        PlayerPrefs.SetInt("LivesMax", livesMaxSet);
        PlayerPrefs.Save();
        Debug.Log("saved date");
    }
    public void LoadPlayerLivesSettings()
    {
        if (PlayerPrefs.HasKey("LivesMax"))
        {
            int livesMaxSet = PlayerPrefs.GetInt("LivesMax");
            MainMenu.InstanceMenu.LivesMax = livesMaxSet;
        }

        Debug.Log("loaded data");
    }

    //SAVE AXIS SETTINGS
    public void SavePlayerAxisSettings()
    {
        int xValue = AxisOrientation.instance. XAxisInverted ? 1 : 0;
        int yValue = AxisOrientation.instance.YAxisInverted ? 1 : 0;
        PlayerPrefs.SetInt("xAxisInverted", xValue);
        PlayerPrefs.SetInt("yAxisInverted", yValue);
        PlayerPrefs.Save();
        Debug.Log("saved date");
    }
    public void LoadPlayerAxisSettings()
    {
        if (PlayerPrefs.HasKey("xAxisInverted"))
        {
            int xValue = PlayerPrefs.GetInt("xAxisInverted");
            AxisOrientation.instance.XAxisInverted = xValue == 1 ? true : false;
        }
        if (PlayerPrefs.HasKey("yAxisInverted"))
        {
            int yValue = PlayerPrefs.GetInt("yAxisInverted");
            AxisOrientation.instance.YAxisInverted = yValue == 1 ? true : false;

        }
        Debug.Log("loaded data");
    }

    //SET VOLUME SETTINGS
    public void SavePlayerVolumeSettings()
    {
        int volSlider = SoundManager.instance.VolumeToSlider;
        float volLvl = SoundManager.instance.VolumeLvl;
        PlayerPrefs.SetInt("volumeToSlider", volSlider);
        PlayerPrefs.SetFloat("volumeLvl", volLvl);
        PlayerPrefs.Save();
        Debug.Log("saved date");
    }
    public void LoadPlayerVolumeSettings()
    {
        if (PlayerPrefs.HasKey("volumeToSlider"))
        {
            int volSlider = PlayerPrefs.GetInt("volumeToSlider");
            SoundManager.instance.VolumeToSlider = volSlider;
        }
        if (PlayerPrefs.HasKey("volumeLvl"))
        {
            float volLvl = PlayerPrefs.GetFloat("volumeLvl");
            SoundManager.instance.VolumeLvl = volLvl;

        }
        Debug.Log("loaded data");
    }


    void Start()
    {
    }
    void Update()
    {
    }
}
