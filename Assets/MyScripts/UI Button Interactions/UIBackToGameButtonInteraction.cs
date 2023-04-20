using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBackToGameButtonInteraction : MonoBehaviour
{
    private Touch touch;

    void Start()
    { }

    /// <summary>
    /// NON FUNZIONA
    /// </summary>
    void Update()
    {
        //Handle screen touches.
        if (Input.touchCount > 0)
        {
            StartButtonAnimationCoroutine();
        }
    }
    public void StartButtonAnimationCoroutine()
    {
        StartCoroutine(TouchResizeCoroutine());
        //Invoke("CallPauseCheck",.1f);
    }
    public void CallPauseCheck()
    {
        GameMenu.instance.CheckPause();
    }
    IEnumerator TouchResizeCoroutine()
    {

        touch = Input.GetTouch(0);
        Time.timeScale = 0.1f;
        if (touch.phase == TouchPhase.Began)
        {

            //multiply the size .
            transform.DOScale(Vector3.one * 6f, .005f).SetEase(Ease.InBounce);
            //transform.localScale = new Vector3(0.75f, 0.75f, 0.75f);
        }

        if (touch.phase == TouchPhase.Ended)
        {
            // Restore the regular size.
            transform.DOScale(Vector3.one * 3.66f, .01f).SetEase(Ease.InBounce);
            //transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            
        }
        yield return null;
    }
    private void OnDisable()
    {
            transform.DOScale(Vector3.one * 3.66f, .05f).SetEase(Ease.InBounce);
    }
}
