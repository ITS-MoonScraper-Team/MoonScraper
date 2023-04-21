using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.Rendering;

public class WallGeneration : MonoBehaviour
{
    #region << VARIABLES >>

    public static WallGeneration Instance;

    //PLAYER COMPONENTS
    CapsuleCollider2D capsuleCollider;
    //SpriteRenderer spriteRenderer;

    //GAME OBJECTS
    public GameObject backGroundStart;
    public StageSection startingStage;
    public PlatformBehaviour startingPlatform;
    public GameObject[] backGround;
    public GameObject[] stageWalls;
    //public Sprite[] spriteArray;

    //ACTIVE OBJECTS LISTS
    private List<GameObject> listaBackGround = new List<GameObject>();
    public List<GameObject> ListaBackGround { get; set; }

    private List<StageSection> listaStageGenerati = new List<StageSection>();
    public List<StageSection> ListaStageGenerati { get; set; }

    private List<PlatformBehaviour> listaPlatforms = new List<PlatformBehaviour>();
    public List<PlatformBehaviour> ListaPlatforms { get; set; }

    //CONTROL VARIABLES
    private bool firstStagePassed = false;
    private bool stageDone = false;

    //Future Use - Pool variables
    public List<StageSection> activeStageList;
    public List<StageSection> stagePool;
    #endregion

    #region <<INIT>>

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        //Get player 2D capsule collider
        capsuleCollider = GetComponent<CapsuleCollider2D>();

        //set stage list
        ListaStageGenerati = new List<StageSection>();
        ListaStageGenerati.Add(startingStage);

        //set platform list
        ListaPlatforms = new List<PlatformBehaviour>();
        ListaPlatforms.Add(startingPlatform);
        startingPlatform.index = PlatformBehaviour.Index.MAI_TOCCATA;
        //startingPlatform.absoluteNumber = 1;

        //set background list
        listaBackGround.Add(backGroundStart);
    }
    #endregion

    void Update() 
    {
        ///<summary>
        ///-genera e assegna 5/10 (scegli tu numero max) stage (o platform) random in poolList.
        ///-ne prendi uno alla volta, lo passi alla lista ListaStageGenerati e li metti in game.
        ///-quando rimangono meno di 5/10 (oppure a 0),
        ///ne genera uno random (oppure se a 0 nuovi 5 / 10) dalle 12 varianti di prefab.
        ///
        ///-se ci sono più di 5/10 in gioco elimina ultimo
        ///-opure assegni random stage + plat quando viene istanziato alla ListaStage
        /// </summary>

        #region <<< GENERA STAGE >>>

        //GENERAZIONE ENDLESS STAGE
        //Se il player si avvicina più di 4 unità al lowerbound dell'ultimo stage creato ne istanzia uno nuovo
        if ((ListaStageGenerati[ListaStageGenerati.Count-1].transform.position.y - capsuleCollider.bounds.max.y) < 4f && stageDone==false)
        {
            stageDone = true;

            InstanceNewObjects();
        }
        #endregion

        #region <<< RIMUOVE STAGE IN ECCESSO >>>

        if (ListaStageGenerati.Count>5)
        {
            StageSection stageToPool = ListaStageGenerati[0];
            PutBackInPool(stageToPool);
        }
        #endregion

    }

    #region <<< INSTANCE FUNCTIONS >>>

    public void InstanceNewObjects()
    {
        //stageDone = true;

        InstanceStageSectionAndPlatform();

        if (listaBackGround[listaBackGround.Count - 1].transform.position.y - capsuleCollider.bounds.max.y < 20f)
            InstanceBackground();

        stageDone = false;
    }

    public void InstanceStageSectionAndPlatform()
    {
        GameObject newStage = Instantiate(stageWalls[Random.Range(0, 12)],
                new Vector3(0, ListaStageGenerati[ListaStageGenerati.Count - 1].transform.position.y + 11f, 0), Quaternion.identity);
        StageSection newStageSection = newStage.GetComponent<StageSection>();
        ListaStageGenerati.Add(newStageSection);
        ListaPlatforms.Add(newStageSection.platform);
    }

    public void InstanceBackground()
    {
        listaBackGround.Add(Instantiate(backGround[0],
                new Vector3(-2.3f, listaBackGround[listaBackGround.Count - 1].transform.position.y + 19f, 1), Quaternion.Euler(0, 0, 270)));
    }
    #endregion

    #region <<< REMOVE FUNCTIONS (BACK TO POOL) >>>

    public void PutBackInPool(StageSection poolObject)
    {
        //rimuove background da sotto se abbastanza distante da player
        if (poolObject.transform.position.y - listaBackGround[0].transform.position.y > 15f)
            RemoveBackground(listaBackGround[0]);

        //rimuove stageSection
        if (ListaStageGenerati.Contains(poolObject))
        {
            RemoveStageSection(poolObject);
        }
    }

    public void RemoveStageSection(StageSection poolObject)
    {
        //stagePool.Add(poolObject);
        ListaStageGenerati.Remove(poolObject);
        //poolObject.gameObject.SetActive(false);
        Destroy(poolObject.gameObject);
    }

    public void RemoveBackground(GameObject backgroundObject)
    {
        if (listaBackGround.Contains(backgroundObject))
        {
            listaBackGround.Remove(backgroundObject);
            Destroy(backgroundObject.gameObject);
        }
    }
    #endregion

    #region <Future Use: Eventuali POOL/PLACE PLATFORM FUNCTIONS>
    //UNA CHIAMA L'ALTRA
    public StageSection GetFromPool()
    {
        //se ho oggetti nella pool
        if (stagePool.Count > 0)
        {
            //mi salvo l'oggetto da prendere
            StageSection poolStageChosen = stagePool[0];

            //lo rimuovo dalla pool
            stagePool.RemoveAt(0);

            //lo aggiungo alla lista degli oggetti attivi
            activeStageList.Add(poolStageChosen);

            //ne restituisco l'istanza al giocatore
            return poolStageChosen;
        }
        else
        {
            //la pool è vuota
            //TO DO creare un nuovo oggetto, da restituire al giocartore
            return null;
        }
    }
    public void PlacePlatform()
    {
        StageSection stage = GetFromPool();
        stage.gameObject.SetActive(true);

        //posiziona la piattaf a destra dell'ultima in active platform
        //platform.transform.localScale = new Vector3(UnityEngine.Random.Range(10, 30), 1, 1);
        //platform.transform.position = lastPlatformPos
        //+ Vector3.right * UnityEngine.Random.Range(5, 10)
        //+ Vector3.up * UnityEngine.Random.Range(-3, 3)
        //+ Vector3.right * platform.transform.localScale.x / 2;
    }
    #endregion

}
