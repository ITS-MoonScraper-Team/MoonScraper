using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        GameObject thisShot;

        if (side == 0)
        {
            thisShot = Instantiate(shot, transform.position, Quaternion.Euler(0, 0, 270));
            thisShot.GetComponent<Shot>().FiringSide = 0;
        }
        else
        {
            thisShot = Instantiate(shot, transform.position, Quaternion.Euler(0, 0, 90));
            thisShot.GetComponent<Shot>().FiringSide = 1;
        }
        //thisShot.GetComponent<Shot>().FiringSide = this.side;
        
        //}
    }
    void Update()
    {
        //timerCounter += Time.deltaTime;
    }
}
