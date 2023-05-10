using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotazioneDestra : MonoBehaviour
{
    public float rotationSpeed; 
    void Start()
    {
        
    }


    void Update()
    {
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);      
    }
}
