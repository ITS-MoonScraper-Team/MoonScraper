using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class AxisOrientation : MonoBehaviour
{
    public static AxisOrientation instance;
    public Toggle xAxisToggle;
    public Toggle yAxisToggle;
    private Image xImage;
    private Image yImage;

    private bool xAxisInverted=true;
    public bool XAxisInverted
    { 
        get { return xAxisInverted; } 
        set { xAxisInverted = value;  } 
    }

    private bool yAxisInverted=true;
    public bool YAxisInverted
    { 
        get { return yAxisInverted; } 
        set { yAxisInverted = value;  } 
    }

    private void Awake()
    {
        if(AxisOrientation.instance!=null)
        {
            Destroy(gameObject);
               
        }
        else
        {
            xImage= xAxisToggle.GetComponentInChildren<Image>();
            yImage = yAxisToggle.GetComponentInChildren<Image>();
            instance = this;

            ///non mette img rossa del toggle dopo aver richiamato la scena--fixed
            LoadPlayerSettings();

            DontDestroyOnLoad(this);
        }
    }
    void Start()
    {
    }

    //void Update()
    //{
    //    if (xAxisToggle != null && yAxisToggle != null)
    //    {
    //        if (!xAxisInverted)
    //            xImage.color = Color.red;
    //        else
    //        {
    //            xImage.color = Color.green;
    //        }
    //        if (!yAxisInverted)
    //            yImage.color = Color.red;
    //        else
    //        {
    //            yImage.color = Color.green;
    //        }
    //    }
    //}

    //SAVE CALLED ON BACK BUTTON IN MAIN MENU

    public void SavePlayerSettings()
    {
        int xValue = XAxisInverted ? 1 : 0;
        int yValue = YAxisInverted ? 1 : 0;
        PlayerPrefs.SetInt("xAxisInverted", xValue);
        PlayerPrefs.SetInt("yAxisInverted", yValue);
        PlayerPrefs.Save();
        Debug.Log("saved date");
    }

    public void LoadPlayerSettings()
    {
        if(PlayerPrefs.HasKey("xAxisInverted"))
        {
            int xValue = PlayerPrefs.GetInt("xAxisInverted");
            XAxisInverted = xValue==1? true: false;
        }
        if (PlayerPrefs.HasKey("yAxisInverted"))
        {
            int yValue = PlayerPrefs.GetInt("yAxisInverted");
            YAxisInverted = yValue == 1 ? true : false;

        }
        Debug.Log("loaded data");
    }
}
