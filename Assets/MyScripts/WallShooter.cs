using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class WallShooter : MonoBehaviour
{
    public GameObject shot;

    private int side;
    public int Side { get; set; }

    private float spawnTime=3f;
    //platOnSpawnTimePace
    public int SpawnTimePace = 10;
    //private int timerCounter=0;
    //public static ShotSpawner instance;

    //private void Awake()
    //{
    //    instance = this;
    //}
    void Start()
    {
        spawnTime= (3f - PlayerController.TotalPlatformCount / SpawnTimePace) < 1f ? 1f : (3f - PlayerController.TotalPlatformCount / SpawnTimePace);
        //invoca a ripetizione funzione: nome funz, tempo prima di avvio, cadenza
        InvokeRepeating("SpawnShot", 1.5f, spawnTime);
        //Player p = FindObjectOfType<Player>();
        //p.OnDeath.AddListener(cancellaSpawn);
    }

    private void cancellaSpawn()
    {
        CancelInvoke("SpawnShot");
    }

    public void SpawnShot()
    {
        //if (Player. > 0)
        //{
        gameObject.transform.DOPunchScale(new Vector3(0.1f,.3f,0), 0.1f, 2, .5f);
        //gameObject.transform.DOPunchScale()

        Invoke("InstanceShot",.2f);

        //thisShot.GetComponent<Shot>().FiringSide = this.side;

        //}
    }
    public void InstanceShot()
    {
        GameObject thisShot;

        if (Side == 0)
        {
            thisShot = Instantiate(shot, new Vector3(transform.position.x + 1, transform.position.y), Quaternion.Euler(0, 0, 270));
            thisShot.GetComponent<Shot>().FiringSide = 0;
        }
        else
        {
            thisShot = Instantiate(shot, new Vector3(transform.position.x - 1, transform.position.y), Quaternion.Euler(0, 0, 90));
            thisShot.GetComponent<Shot>().FiringSide = 1;
        }
    }
    void Update()
    {
        //timerCounter += Time.deltaTime;
    }
}
