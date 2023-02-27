using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxisOrientation : MonoBehaviour
{
    private bool xAxisInverted=true;
    public bool XAxisInverted
        { get { return xAxisInverted; } set { xAxisInverted = value; } }

    private bool yAxisInverted=true;
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
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
