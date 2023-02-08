using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class WallGeneration : MonoBehaviour
{
    private bool firstStagePassed = false;
    private bool stageDone = true;

    Rigidbody2D Rigidbody;
    CapsuleCollider2D capsuleCollider;

    public GameObject startingStage;
    public GameObject backGroundStart;
    public GameObject[] stageWalls;
    public GameObject[] backGround;

    //private int[] platformStatusIndexRange = new int[] { 0, 1, 2 };
    
    public class Platforms 
    {

        private GameObject platformGenerated;
        public GameObject PlatformGenerated { get; set; }

        private int platformStatusIndex;
        public int PlatformStatusIndex { get; set; }

        public Platforms (GameObject _platformGenerated, int _platformStatusIndex)
        {
            PlatformGenerated = _platformGenerated;
            
            PlatformStatusIndex = platformStatusIndex;
        }
    }


    //public Camera MainCamera;
    //SpriteRenderer spriteRenderer;
    //public Sprite[] spriteArray;

    private List<GameObject> listaBackGround = new List<GameObject>();

    private List<GameObject> listaStageGenerati = new List<GameObject>();
    public List<GameObject> ListaStageGenerati
    {
        get { return listaStageGenerati; }
        set { listaStageGenerati = value; }
    }

    private List<Platforms> listaPlatforms = new List<Platforms>();
    public List<Platforms> ListaPlatforms
    {
        get { return listaPlatforms; }
        set { listaPlatforms = value; }
    }
  

    public static WallGeneration Instance;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        Rigidbody = GetComponent<Rigidbody2D>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        listaStageGenerati.Add(startingStage);
        listaBackGround.Add(backGroundStart);
        //Platforms firstPlat = new Platforms ( startingStage.transform.GetChild(0).gameObject , platformStatusIndexRange[0]);

        ListaPlatforms.Add(new Platforms(startingStage.transform.GetChild(0).gameObject, 0/*platformStatusIndexRange[0]*/));
            //[0].Platform=startingStage.transform.GetChild(0).gameObject;

        //var platformsList = new List<(GameObject platform, int[] platformStatusIndex)>();

        //var georgeEmail = platformsList[1].platformStatusIndex[0];
        //var people = new List<Person>();

        //prende child come GameObject: oggettoparent.transform.getchild(n).gameObject
        //se invece: oggettoparent.transform.getchild(n) --> è preso come Transform

    }

    // Update is called once per frame
    void Update()
    {

        //GENERAZIONE SECONDO STAGE
        if ((startingStage.transform.position.y - capsuleCollider.bounds.max.y) < 4.5f && firstStagePassed == false)
        {
            ListaStageGenerati.Insert(0, Instantiate(stageWalls[Random.Range(0, 2)], new Vector3(0, startingStage.transform.position.y + 10.7f, 0), Quaternion.identity));
            listaBackGround.Insert(0, Instantiate(backGround[0], new Vector3(-2.3f, backGroundStart.transform.position.y + 19f, 0), Quaternion.Euler(0, 0, 270)));
            //listaBackGround.Insert(0, Instantiate(backGround[0], new Vector3(-2.3f, backGround[0].transform.position.y + 19f, 0), Quaternion.Euler(0, 0, 270)));
            //Platforms secondPlat = new Platforms(ListaStageGenerati[0].transform.GetChild(0).gameObject, platformStatusIndexRange[0]);
            ListaPlatforms.Insert(0, new Platforms(ListaStageGenerati[0].transform.GetChild(0).gameObject, 0));
            firstStagePassed = true;
        }

        //GENERAZIONE ENDLESS STAGE
        if ((ListaStageGenerati[0].transform.position.y - capsuleCollider.bounds.max.y) < 5)
        { stageDone = false; }
        if (stageDone == false)
        {
            listaStageGenerati.Insert (0, Instantiate(stageWalls[Random.Range(0, 2)], new Vector3(0, listaStageGenerati[0].transform.position.y + 10.5f /**(1+listaStageGenerati.Count)*/, 0), Quaternion.identity));
            listaBackGround.Insert(0, Instantiate(backGround[0], new Vector3(-2.3f, listaBackGround[0].transform.position.y + 19, 0), Quaternion.Euler(0, 0, 270)));

            //Platforms nextPlat = new Platforms(ListaStageGenerati[0].transform.GetChild(0).gameObject, platformStatusIndexRange[0]);
            ListaPlatforms.Insert(0, new Platforms(ListaStageGenerati[0].transform.GetChild(0).gameObject, 0));

            stageDone = true;
        }

        #region destroyExcessTest
        //if (listaStageGenerati.Count > 50)
        //{ listaStageGenerati.RemoveRange(50, listaStageGenerati.Count); }

        //if (listaStageGenerati.Count > 3)
        //{
        //    for (int i = 3; i < listaStageGenerati.Count + 1; i++)
        //    { Destroy(listaStageGenerati[i].gameObject); }
        //}
        #endregion

        #region Children.collider test
        //Collider2D[] stageColliders;
        //stageColliders = startingStage.GetComponentsInChildren<Collider2D>();/*GetComponent<Collider2D>().bounds.max.y;*/
        //float maxY = stageColliders[0].transform.position.y;
        //string collName = stageColliders[0].name;
        //for (int i = 1; i < stageColliders.Length; i++)
        //{  
        //    Debug.Log(stageColliders[i].transform.position.y);
        //    if (stageColliders[i].transform.position.y >= maxY)
        //    {
        //        maxY = stageColliders[i].transform.position.y;
        //        collName = stageColliders[i].name;
        //    }
        //    else
        //    {
        //    }
        //    Debug.Log(stageColliders[i].name + " = " + maxY);
        //}
        //Debug.Log("MAX Y" + collName + maxY);
        //Debug.Log(capsuleCollider.bounds.max.y);

        //listaStageGenerati.Add(startingStage);
        //if ((maxY - capsuleCollider.bounds.max.y) < 5)
        //    firstStagePassed = false;

        //Collider2D[] newStageColliders; /*= new Collider2D[4];*/
        //newStageColliders = listaStageGenerati[0].GetComponentsInChildren<Collider2D>();/*GetComponent<Collider2D>().bounds.max.y;*/
        //float newMaxY = newStageColliders[0].transform.position.y;
        //string newCollName = newStageColliders[0].name;
        //for (int i = 1; i < newStageColliders.Length; i++)
        //{  
        //    Debug.Log(newStageColliders[i].transform.position.y);
        //    if (newStageColliders[i].transform.position.y >= newMaxY)
        //    {
        //        newMaxY = newStageColliders[i].transform.position.y;
        //        newCollName = newStageColliders[i].name;
        //    }
        //    Debug.Log(newStageColliders[i].name + " = " + newMaxY);
        //}
        //    if ((newMaxY - capsuleCollider.bounds.max.y) < 5 && stageDone==false)
        //    {
        //        listaStageGenerati.Insert(0, Instantiate(stageWalls[Random.Range(0, 2)], new Vector3(0, listaStageGenerati[0].transform.position.y + 10.5f /**(1+listaStageGenerati.Count)*/, 0), Quaternion.identity));
        //        stageDone = true;
        //    }
        #endregion

        #region Instantiate Walls test
        //if (capsuleCollider.bounds.max.y > maxStartStageY - 6.0f)
        //{
        //    Debug.Log(capsuleCollider.bounds.max.y);
        //    //newStage = Instantiate(stageWalls[0]);
        //    //stageWalls[i].transform.position = wallPos + (direction * newSpacing);
        //}

        //if (gameObject.transform.position.y-MainCamera.transform.position.y<3)
        //{
        //    GameObject gameObj = new GameObject() ;
        //    gameObj.AddComponent<WallGeneration>();
        //    gameObj.AddComponent<Transform>();
        //    gameObj.AddComponent<SpriteRenderer>();
        //    gameObj.AddComponent<Collider2D>();
        //    wallPos = gameObj.AddComponent<Transform>()=
        //    gameObj.transform.position.y = (gameObject.transform.position.y + 5.2);
        //}
        #endregion
    }
}
