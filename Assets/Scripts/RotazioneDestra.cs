using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RotazioneDestra : MonoBehaviour
{
    public float rotationSpeed;
    public Gradient goldGradient;
    public float scaleResize=.8f;
    public float playScaleResize = 1.1f;
    private Vector3 startingScale;

    private void Awake()
    {
        startingScale = transform.localScale;

       // transform.localScale = Vector3.zero;
    }
    void Start()
    {
        //startingScale = transform.localScale;
        transform.localScale = Vector3.zero;


        //startingScale = transform.localScale;
        //StartCoroutine(UIScalingCoroutine());
        //if (goldGradient != null && gameObject.GetComponent<SpriteRenderer>() != null)
        //    StartCoroutine(UIGradientColorCoroutine());
        //else if (gameObject.GetComponent<SpriteRenderer>() != null)
        //    StartCoroutine(UIColorCoroutine());
        //if (gameObject.name == "PLAY BUTTON")
        //    StartCoroutine(UIPlayButtonPulsation());
    }
    private void OnEnable()
    {
        if(gameObject.name != "PLAY BUTTON")
            StartCoroutine(UIScalingCoroutine());
        else 
            StartCoroutine(UIPlayButtonPulsation());

        if (goldGradient != null && gameObject.GetComponent<SpriteRenderer>() != null)
            StartCoroutine(UIGradientColorCoroutine());
        else if (gameObject.GetComponent<SpriteRenderer>() != null)
            StartCoroutine(UIColorCoroutine());
        //if (gameObject.name == "PLAY BUTTON")
        //    StartCoroutine(UIPlayButtonPulsation());
    }

    void Update()
    {
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
        //transform.DOScale(.8f, .5f);
    }
    IEnumerator UIScalingCoroutine()
    {
        while (true)
        {
            transform.DOScale(startingScale*scaleResize, .8f);
            yield return new WaitForSeconds(.8f);
            transform.DOScale(startingScale, .8f);
            yield return new WaitForSeconds(.8f);

        }
    }
    IEnumerator UIColorCoroutine()
    {
        while (true)
        {
            gameObject.GetComponent<SpriteRenderer>().DOColor(Color.blue, .8f);

            yield return new WaitForSeconds(.8f);
            gameObject.GetComponent<SpriteRenderer>().DOColor(Color.red, .8f);

            yield return new WaitForSeconds(.8f);
            gameObject.GetComponent<SpriteRenderer>().DOColor(Color.yellow, .8f);
            yield return new WaitForSeconds(.8f);
        }
    }
    IEnumerator UIGradientColorCoroutine()
    {
        while (true)
        {
            gameObject.GetComponent<SpriteRenderer>().DOGradientColor(goldGradient, 3f);

            yield return new WaitForSeconds(3f);
            gameObject.GetComponent<SpriteRenderer>().DOGradientColor(goldGradient, 3f);

            yield return new WaitForSeconds(3f);
            gameObject.GetComponent<SpriteRenderer>().DOGradientColor(goldGradient, 3f);
            yield return new WaitForSeconds(3f);
        }
    }
    IEnumerator UIPlayButtonPulsation()
    {
        while (true)
        {
            //transform.DOScale(startingScale * scaleResize, .75f);
            //GetComponent<Image>().DOColor(new Color(0.85f, 0.7f, 0.7f), .75f);
            //yield return new WaitForSeconds(.2f);
            //transform.DOScale(startingScale, .45f);
            //GetComponent<Image>().DOColor(Color.white, .45f);
            //yield return new WaitForSeconds(.2f);
            //transform.DOScale(startingScale * scaleResize, .45f);
            //GetComponent<Image>().DOColor(new Color(0.85f, 0.7f, 0.7f), .45f);
            //yield return new WaitForSeconds(.2f);
            //transform.DOScale(startingScale, 1.15f);
            //GetComponent<Image>().DOColor(Color.white, 1.15f);
            //yield return new WaitForSeconds(.8f);



            transform.DOScale(startingScale * playScaleResize, .2f);
            GetComponent<Image>().DOColor(Color.white, .2f);
            yield return new WaitForSeconds(.2f);
            transform.DOScale(startingScale, .3f);
            GetComponent<Image>().DOColor(new Color(0.85f, 0.7f, 0.7f), .3f);
            yield return new WaitForSeconds(.3f);
            transform.DOScale(startingScale * playScaleResize, .4f);
            GetComponent<Image>().DOColor(Color.white, .4f);
            yield return new WaitForSeconds(.4f);
            transform.DOScale(startingScale, 1f);
            GetComponent<Image>().DOColor(Color.white, 1f);
            yield return new WaitForSeconds(1f);

        }
    }
    

}
