using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AxisOrientation : MonoBehaviour
{
    public static AxisOrientation instance;
    public Toggle xAxisToggle;
    public Toggle yAxisToggle;

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
            instance = this;
            DontDestroyOnLoad(this);
        }

        LoadPlayerSettings();

    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (xAxisToggle != null && yAxisToggle != null)
        {
            if (!xAxisInverted)
                xAxisToggle.GetComponentInChildren<Image>().color = Color.red;
            else
            {
                xAxisToggle.GetComponentInChildren<Image>().color = Color.green;
            }
            if (!yAxisInverted)
                yAxisToggle.GetComponentInChildren<Image>().color = Color.red;
            else
            {
                yAxisToggle.GetComponentInChildren<Image>().color = Color.green;
            }
        }
    }

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
