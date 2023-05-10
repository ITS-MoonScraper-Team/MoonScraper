using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shot : MonoBehaviour
{
    [SerializeField] private float speed=4f;
    public float Speed=> speed + Mathf.Log10(PlayerController.TotalPlatformCount);
    
    [SerializeField] private float scaleResize = 1.2f;
    private Vector3 startingScale;

    //firing side
    private int firingSide;
    public int FiringSide 
    { 
        get =>firingSide;
        set =>firingSide= value; 
    }

    void Start()
    {
        Invoke("DestroyShot", 10f);

        startingScale = transform.localScale;
        StartCoroutine(ShotPulsatingCoroutine());
    }

    void Update()
    {
        transform.position += (FiringSide==0?Vector3.right:Vector3.left) * Time.deltaTime * Speed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        //Destroy(collision.gameObject);
        //Destroy(gameObject);
    }
    private void OnBecameInvisible()
    {
        Invoke("DestroyShot", .1f);
    }
    
    private void DestroyShot()
    {
        Destroy(gameObject);
    }
    IEnumerator ShotPulsatingCoroutine()
    {
        while (true)
        {
            transform.DOPunchScale(startingScale * scaleResize, .4f, 4, 1f);
            yield return new WaitForSeconds(.4f);
        }
    }
}
