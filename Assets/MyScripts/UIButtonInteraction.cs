using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIButtonInteraction : MonoBehaviour
{
    //On


    void Start()
    {
        
    }

    /// <summary>
    /// NON FUNZIONA
    /// </summary>
    void Update()
    {
        //Handle screen touches.
        if (Input.touchCount > 0)
        {

            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                //multiply the size .
                transform.DOScale(Vector3.one * 8f, .15f).SetEase(Ease.InBounce);
                //transform.localScale = new Vector3(0.75f, 0.75f, 0.75f);
            }

            if (touch.phase == TouchPhase.Ended)
            {
                // Restore the regular size.
                transform.DOScale(Vector3.one * 3.66f, .15f).SetEase(Ease.InBounce);
                //transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

            }
        }
    }
}
