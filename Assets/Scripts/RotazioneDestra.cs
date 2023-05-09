using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotazioneDestra : MonoBehaviour
{
    public float rotationSpeed;
    public Gradient goldGradient;
    private Vector3 startingScale;
    void Start()
    {
        startingScale = transform.localScale;
        StartCoroutine(UIScalingCoroutine());
        StartCoroutine(UIColorCoroutine());
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
            transform.DOScale(startingScale*.8f, .8f);
            yield return new WaitForSeconds(.8f);
            transform.DOScale(startingScale, .8f);
            yield return new WaitForSeconds(.8f);

        }
    }
    IEnumerator UIColorCoroutine()
    {
        while (true)
        {
            if (goldGradient != null && gameObject.GetComponent<SpriteRenderer>()!=null)
            {
                gameObject.GetComponent<SpriteRenderer>().DOGradientColor(goldGradient, .8f);

                yield return new WaitForSeconds(.8f);
                gameObject.GetComponent<SpriteRenderer>().DOGradientColor(goldGradient, .8f);

                yield return new WaitForSeconds(.8f);
                gameObject.GetComponent<SpriteRenderer>().DOGradientColor(goldGradient, .8f);
                yield return new WaitForSeconds(.8f);
            }
            else if(gameObject.GetComponent<SpriteRenderer>() != null)
            {
                gameObject.GetComponent<SpriteRenderer>().DOColor(Color.blue, .8f);

                yield return new WaitForSeconds(.8f);
                gameObject.GetComponent<SpriteRenderer>().DOColor(Color.red, .8f);

                yield return new WaitForSeconds(.8f);
                gameObject.GetComponent<SpriteRenderer>().DOColor(Color.yellow, .8f);
                yield return new WaitForSeconds(.8f);
            }


        }
    }
}
