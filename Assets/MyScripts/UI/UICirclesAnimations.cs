using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UICirclesAnimations : MonoBehaviour
{
    public float rotationSpeed;
    public Gradient goldGradient;
    public float scaleResize = .8f;
    private Vector3 startingScale;

    private void Awake()
    {
        startingScale = transform.localScale;
        transform.localScale = Vector3.zero;
    }
    void Start()
    {
    }
    private void OnEnable()
    {
        StartCoroutine(UIScalingCoroutine());

        if (goldGradient != null && gameObject.GetComponent<SpriteRenderer>() != null)
            StartCoroutine(UIGradientColorCoroutine());
        else
            StartCoroutine(UIColorCoroutine());
    }

    void Update()
    {
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
    }
    IEnumerator UIScalingCoroutine()
    {
        while (true)
        {
            transform.DOScale(startingScale * scaleResize, .8f);
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
}