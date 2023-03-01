using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AxisOrientation : MonoBehaviour
{
    public Toggle xAxisToggle;
    public Toggle yAxisToggle;
    public static AxisOrientation instance;

    private bool xAxisInverted;
    public bool XAxisInverted
        { get { return xAxisInverted; } set { xAxisInverted = value; } }

    private bool yAxisInverted;
    public bool YAxisInverted
        { get { return yAxisInverted; } set { yAxisInverted = value; } }

    public void InvertXAxis()
    {
        if (XAxisInverted)
            XAxisInverted = false;
        else
            XAxisInverted = true;
    }
    public void InvertYAxis()
    {
        if (YAxisInverted)
            YAxisInverted = false;
        else
            YAxisInverted = true;
    }

    private void Awake()
    {
        instance = this;
        //if (PlayerController.instance.joystickXaxisInverted != null)
        //{
        //   xAxisToggle.isOn = PlayerController.instance.joystickXaxisInverted;
        //    //XAxisInverted = PlayerController.instance.joystickXaxisInverted;

        ////    //XAxisInverted = if( PlayerController.instance.joystickXaxisInverted)

        //}
        //if(PlayerController.instance.joystickYaxisInverted!=null)
        //{
        //    yAxisToggle.isOn = PlayerController.instance.joystickYaxisInverted;
        ////    //YAxisInverted = PlayerController.instance.joystickYaxisInverted;

        ////    //YAxisInverted = PlayerController.instance.joystickYaxisInverted;

        //}
        //xAxisInverted = xAxisToggle.isOn;
        //yAxisInverted = yAxisToggle.isOn;



        if (xAxisToggle.isOn)
            XAxisInverted = true;
        else
            XAxisInverted = false;
        if (yAxisToggle.isOn)
            YAxisInverted = true;
        else
            YAxisInverted = false;         //(bool)PlayerController.instance.joystickYaxisInverted;
    }
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
