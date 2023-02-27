using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyOnLoad : MonoBehaviour
{
    //public static DontDestroyOnLoad instance;
    //private List<GameObject> SoundManagerInstance;
    //void Awake()
    //{
    //    instance = this;
    //    instance.SoundManagerInstance.Add(gameObject);
    //    //GameObject[] objs = GameObject.FindGameObjectsWithTag("soundManager");

    //    //if (objs.Length > 1)
    //    //{
    //    //    Destroy(this.gameObject);
    //    //}
    //    if (SoundManagerInstance.Count > 1)
    //    {
    //        for (int i = 0; i < SoundManagerInstance.Count; i++)
    //        {
    //            Destroy(gameObject);
    //        }
    //    }
    //    else
    //    { }
    //    DontDestroyOnLoad(gameObject);
    //}

    void Start()
    {

        //for (int i = 0; i < Object.FindObjectsOfType<DontDestroyOnLoad>().Length; i++)
        //{
        //    if (Object.FindObjectsOfType<DontDestroyOnLoad>()[i] != this)
        //        Destroy(gameObject);
        //}
        //DontDestroyOnLoad(gameObject);   

    }

    void Update()
    {
        
    }
}
