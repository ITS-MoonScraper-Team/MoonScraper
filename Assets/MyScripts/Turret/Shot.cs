using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shot : MonoBehaviour
{
    [SerializeField] private float speed;
    //speed per platform pace
    public int speedRatePace = 20;
    //firing side
    private int firingSide;
    public int FiringSide { get ; set ; }
    public float scaleResize = 1.2f;
    private Vector3 startingScale;

    public List<AudioClip> shotSounds = new List<AudioClip>(); 

    // Start is called before the first frame update
    void Start()
    {
        startingScale = transform.localScale;
        if ((Vector3.Distance(transform.position, FindObjectOfType<PlayerController>().gameObject.transform.position)<10f))
        {
            GetComponent<AudioSource>().clip = shotSounds[Random.Range(0, 4)];
            GetComponent<AudioSource>().Play();
        }
        //speed = (4 + PlayerController.TotalPlatformCount / speedRatePace);
        speed = 4 + Mathf.Log10(PlayerController.TotalPlatformCount);
        StartCoroutine(ShotScalingCoroutine());
        //if(PlayerController.TotalPlatformCount>)
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += (FiringSide==0?Vector3.right:Vector3.left) * Time.deltaTime * speed;
        
        //if (firingSide==0)
        //    transform.position += Vector3.right * Time.deltaTime * speed;
        //else
        //    transform.position += Vector3.left * Time.deltaTime * speed;


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
        Invoke("DestroyObj", .1f);
    }
    private void DestroyObj()
    {
        Destroy(gameObject);

    }
    IEnumerator ShotScalingCoroutine()
    {
        while (true)
        {
            transform.DOPunchScale(startingScale * scaleResize, .4f, 4, 1f);
            //transform.DOScale(startingScale * scaleResize, .2f);
            //yield return new WaitForSeconds(.2f);
            //transform.DOScale(startingScale, .2f);
            //yield return new WaitForSeconds(.2f);
        }
    }
}
