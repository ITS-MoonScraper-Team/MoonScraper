using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayButtonAnimations : MonoBehaviour
{
    [SerializeField] private float playScaleResize = 1.1f;
    private Vector3 startingScale;

    private void Awake()
    {
        startingScale = transform.localScale;
    }
    void Start()
    {
        transform.localScale = Vector3.zero;
    }
    private void OnEnable()
    {
        StartCoroutine(UIPlayButtonPulsation());
    }

    void Update()
    {
    }
   
    IEnumerator UIPlayButtonPulsation()
    {
        while (true)
        {
            //PLAY BUTTON HEART PULSATION
            transform.DOScale(startingScale * playScaleResize, .15f);
            GetComponent<Image>().DOColor(Color.white, .15f);
            yield return new WaitForSeconds(.15f);
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
