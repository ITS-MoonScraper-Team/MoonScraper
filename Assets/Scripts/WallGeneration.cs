using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class WallGeneration : MonoBehaviour
{
    Rigidbody2D Rigidbody;
    CapsuleCollider2D capsuleCollider;
    //SpriteRenderer spriteRenderer;

    public StageSection startingStage;
    public GameObject backGroundStart;
    public GameObject[] stageWalls;
    public GameObject[] backGround;
    public PlatformBehaviour startingPlatform;
    //public Camera MainCamera;
    //public Sprite[] spriteArray;

    private bool firstStagePassed = false;
    private bool stageDone = true;
    private List<GameObject> listaBackGround = new List<GameObject>();

    private List<StageSection> listaStageGenerati = new List<StageSection>();
    public List<StageSection> ListaStageGenerati
    {
        get { return listaStageGenerati; }
        set { listaStageGenerati = value; }
    }

    private List<PlatformBehaviour> listaPlatforms = new List<PlatformBehaviour>();
    public List<PlatformBehaviour> ListaPlatforms
    {
        get { return listaPlatforms; }
        set { listaPlatforms = value; }
    }

    public static WallGeneration Instance;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        Rigidbody = GetComponent<Rigidbody2D>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        ListaStageGenerati = new List<StageSection>();
        ListaStageGenerati.Add(startingStage);
        listaBackGround.Add(backGroundStart);
        startingPlatform.index = PlatformBehaviour.Index.MAI_TOCCATA;
        ListaPlatforms = new List<PlatformBehaviour>();
        ListaPlatforms.Add(startingPlatform);
        //startingPlatform.absoluteNumber = 1;

        //var platformsList = new List<(GameObject platform, int[] platformStatusIndex)>();
        //var georgeEmail = platformsList[1].platformStatusIndex[0];
        //var people = new List<Person>();
    }

    void Update()
    {
       
        //GENERAZIONE SECONDO STAGE
        ///SISTEMO LOGICA STARTINGSTAGE - BOOL STAGE PASSED
        if ((ListaStageGenerati[0].transform.position.y - capsuleCollider.bounds.max.y) < 4f && firstStagePassed == false)
        {
            GameObject newStage = Instantiate(stageWalls[Random.Range(0, 12)], new Vector3(0, ListaStageGenerati[0].transform.position.y + 11f, 0), Quaternion.identity);
            StageSection newStageSection = newStage.GetComponent<StageSection>();
            ListaStageGenerati.Add(newStageSection);
            listaBackGround.Add( Instantiate(backGround[0], new Vector3(-2.3f, backGroundStart.transform.position.y + 19f, 1), Quaternion.Euler(0, 0, 270)));
            ListaPlatforms.Add(newStageSection.platform);

            firstStagePassed = true;
        }

        //GENERAZIONE ENDLESS STAGE
        ///SISTEMO LOGICA BOOL STAGEDONE
        if ((ListaStageGenerati[ListaStageGenerati.Count-1].transform.position.y - capsuleCollider.bounds.max.y) < 4f && firstStagePassed==true)
        { stageDone = false; }
        if (stageDone == false)
        {
            GameObject newStage = Instantiate(stageWalls[Random.Range(0, 12)], new Vector3(0, ListaStageGenerati[ListaStageGenerati.Count-1].transform.position.y + 11f, 0), Quaternion.identity);
            StageSection newStageSection = newStage.GetComponent<StageSection>();
            ListaStageGenerati.Add( newStageSection);
            listaBackGround.Add(Instantiate(backGround[0], new Vector3(-2.3f, listaBackGround[listaBackGround.Count-1].transform.position.y + 19f, 1), Quaternion.Euler(0, 0, 270)));
            ListaPlatforms.Add(newStageSection.platform);
            stageDone = true;
      
        }

        #region destroyExcessTest
        //if (listaStageGenerati.Count > 5)
        //{
        //    int i;
        //    for (i = 5; i < listaStageGenerati.Count; i++)
        //    {
        //        GameObject twalls = listaStageGenerati[i];
        //        listaStageGenerati.RemoveAt(i);

        //        Destroy(twalls.gameObject);
        //    }
        //}
            #endregion
            
    }
}
