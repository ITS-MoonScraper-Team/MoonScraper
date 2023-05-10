using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class WallShooter : MonoBehaviour
{
    public GameObject shot;

    [SerializeField] private int side;
    public int Side 
    {
        get => side; 
        set => side = value;
    }

    [SerializeField] private float startingSpawnTime=4f;
    //[SerializeField] private int spawnTimePace = 100;
    private float SpawnTime=> (startingSpawnTime - Mathf.Log10(PlayerController.TotalPlatformCount)) < 1f ? 1f : (startingSpawnTime - Mathf.Log10(PlayerController.TotalPlatformCount));

    //private int timerCounter=0;

    void Start()
    {
        InvokeRepeating("SpawnShot", 1f, SpawnTime);

        //Player p = FindObjectOfType<Player>();
        //p.OnDeath.AddListener(cancellaSpawn);
    }

    private void CancelSpawn()
    {
        CancelInvoke("SpawnShot");
    }

    public void SpawnShot()
    {
        gameObject.transform.DOPunchScale(new Vector3(0.1f,.3f,0), 0.1f, 2, .5f);

        Invoke("InstanceShot",.1f);
        if ((Vector3.Distance(gameObject.transform.position, FindObjectOfType<PlayerController>().gameObject.transform.position) < 10f))
        {
            SFXsoundManager.instance.PlaySound("shotSound");
        }
        gameObject.transform.DOPunchScale(new Vector3(0.1f, .2f, 0), 0.05f, 2, .5f);

        //thisShot.GetComponent<Shot>().FiringSide = this.side;
    }
    public void InstanceShot()
    {
        GameObject thisShot;

        if (Side == 0)
        {
            thisShot = Instantiate(shot, new Vector3(transform.position.x + 1, transform.position.y), Quaternion.Euler(0, 0, 270));
            thisShot.GetComponent<Shot>().FiringSide = 0;
            thisShot.transform.DOPunchScale(new Vector3(0.05f, .05f, 0), 0.1f, 2, .5f);

        }
        else
        {
            thisShot = Instantiate(shot, new Vector3(transform.position.x - 1, transform.position.y), Quaternion.Euler(0, 0, 90));
            thisShot.GetComponent<Shot>().FiringSide = 1;
            thisShot.transform.DOPunchScale(new Vector3(0.05f, .05f, 0), 0.1f, 2, .5f);

        }
    }
    void Update()
    {
        //timerCounter += Time.deltaTime;
    }
}
