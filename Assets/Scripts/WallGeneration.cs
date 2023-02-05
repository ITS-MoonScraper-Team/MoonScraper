using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class WallGeneration : MonoBehaviour
{
    Rigidbody2D Rigidbody;
    CapsuleCollider2D capsuleCollider;
    //PolygonCollider2D polygonCollider;

    public GameObject startingStage;
    public GameObject[] stageWalls;
    public GameObject[] backGround;

    //public Camera MainCamera;
    //SpriteRenderer spriteRenderer;
    //public Sprite[] spriteArray;

    private bool firstStagePassed = false;
    private bool stageDone = true;
    public List<GameObject> listaStageGenerati;
    private List<GameObject> listaBackGround = new List<GameObject>();

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
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("VELOCITY " + Rigidbody.velocity.magnitude);

        //GENERAZIONE SECONDO STAGE
        if ((startingStage.transform.position.y - capsuleCollider.bounds.max.y) < 4.7f && firstStagePassed == false)
        {
            listaStageGenerati.Add(Instantiate(stageWalls[Random.Range(0, 2)], new Vector3(0, startingStage.transform.position.y + 10.7f, 0), Quaternion.identity));
            listaBackGround.Add(Instantiate(backGround[0], new Vector3(-2.3f, backGround[0].transform.position.y + 19f, 0), Quaternion.Euler(0, 0, 270)));
            firstStagePassed = true;
        }

        //GENERAZIONE ENDLESS STAGE
        if ((listaStageGenerati[0].transform.position.y - capsuleCollider.bounds.max.y) < 5)
        { stageDone = false; }
        if (stageDone == false)
        {
            listaStageGenerati.Insert(0, Instantiate(stageWalls[Random.Range(0, 2)], new Vector3(0, listaStageGenerati[0].transform.position.y + 10.5f /**(1+listaStageGenerati.Count)*/, 0), Quaternion.identity));
            listaBackGround.Insert(0, Instantiate(backGround[0], new Vector3(-2.3f, listaBackGround[0].transform.position.y + 19, 0), Quaternion.Euler(0, 0, 270)));
            stageDone = true;
        }
        #region Children.collider.transform.position.Y
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

        #region Instantiate Walls
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
