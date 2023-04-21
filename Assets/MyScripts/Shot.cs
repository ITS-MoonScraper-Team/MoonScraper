using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shot : MonoBehaviour
{
    [SerializeField]
    private float speed;
    //speed per platform pace
    public int speedRatePace = 20;
    //firing side
    private int firingSide;
    public int FiringSide { get ; set  ; } 

    // Start is called before the first frame update
    void Start()
    {
        speed = (4 + PlayerController.TotalPlatformCount / speedRatePace);
        //if(PlayerController.TotalPlatformCount>)
    }

    // Update is called once per frame
    void Update()
    {
        if (firingSide==0)
            transform.position += Vector3.right * Time.deltaTime * speed;
        else
            transform.position += Vector3.left * Time.deltaTime * speed;


    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        //Destroy(collision.gameObject);
        Destroy(gameObject);
    }
    private void OnBecameInvisible()
    {
        Invoke("DestroyObj", .1f);
        //Destroy(gameObject);
    }
    private void DestroyObj()
    {
        Destroy(gameObject);

    }
}
