using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D Rigidbody;
    CapsuleCollider2D capsuleCollider;
    PolygonCollider2D polygonCollider;
    SpriteRenderer spriteRenderer;
    public FixedJoystick joystick;
    public Sprite[] spriteArray;
    public GameObject startingStage;
    public GameObject[] stageWalls;
    public GameObject[] backGround;
    //public TrailRenderer trailRenderer;

    //public GameObject Handle;

    private bool inputPressed;
    private bool upPressedDown;
    private bool spacePressedDown;
    private bool spacePressed;
    private bool spacePressedUp;
    private bool leftPressed;
    private bool firstStagePassed = false;
    private bool stageDone = true;
    private List<GameObject> listaStageGenerati = new List<GameObject>();
    private List<GameObject> listaBackGround = new List<GameObject>();

    public bool JoystickControl;
    public float jumpForce;
    public float jetpackForce;
    public float fromLeftJetForce;
    public float fromRightJetForce;
    public bool jetpackInAirOnly;
    public bool leftRightInAirOnly;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        Rigidbody = GetComponent<Rigidbody2D>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        polygonCollider = GetComponent<PolygonCollider2D>();

        //trailRenderer = GetComponentInChildren<TrailRenderer>();
        //rectTransform=Handle.GetComponent<RectTransform>(); 
    }

    // ***Cambia sprite direzione movimento***
    void ChangeSprite(Sprite _newSprite)
    {
        spriteRenderer.sprite = _newSprite;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("VELOCITY "+Rigidbody.velocity.magnitude);

        //RaycastHit2D hitz = Physics2D.Raycast(capsuleCollider.bounds.size, Vector2.one, 0.1f);
        ////if (hitz.collider != null)

        //    if (hitz.collider != null&& Rigidbody.velocity.magnitude>10)
        //{
        //    this.gameObject.SetActive(false);
        //}

        //Collider2D[] stageColliders; /*= new Collider2D[4];*/
        //stageColliders = startingStage.GetComponentsInChildren<Collider2D>();/*GetComponent<Collider2D>().bounds.max.y;*/
        ////float[] maxYs = new float[stageColliders.Length];
        //float maxY = stageColliders[0].transform.position.y;
        //string collName = stageColliders[0].name;
        //for (int i = 1; i < stageColliders.Length; i++)
        //{  /*maxYs[i] =*/
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

        if ((startingStage.transform.position.y - polygonCollider.bounds.max.y) < 4.7f && firstStagePassed == false)
        {
            listaStageGenerati.Add(Instantiate(stageWalls[Random.Range(0, 2)], new Vector3(0, startingStage.transform.position.y + 10.7f, 0), Quaternion.identity));
            listaBackGround.Add(Instantiate(backGround[0], new Vector3(-2.3f, backGround[0].transform.position.y + 19f, 0), Quaternion.Euler(0, 0, 270)));
            firstStagePassed = true;
        }

        //Collider2D[] newStageColliders; /*= new Collider2D[4];*/
        //newStageColliders = listaStageGenerati[0].GetComponentsInChildren<Collider2D>();/*GetComponent<Collider2D>().bounds.max.y;*/
        //float[] maxYs = new float[stageColliders.Length];
        //float newMaxY = newStageColliders[0].transform.position.y;
        //string newCollName = newStageColliders[0].name;
        //for (int i = 1; i < newStageColliders.Length; i++)
        //{  /*maxYs[i] =*/
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

        if ((listaStageGenerati[0].transform.position.y - polygonCollider.bounds.max.y) < 5)
        { stageDone = false; }
        if ( stageDone == false)
        {
            listaStageGenerati.Insert(0, Instantiate(stageWalls[Random.Range(0, 2)], new Vector3(0, listaStageGenerati[0].transform.position.y + 10.5f /**(1+listaStageGenerati.Count)*/, 0), Quaternion.identity));
            listaBackGround.Insert(0, Instantiate(backGround[0], new Vector3(-2.3f, listaBackGround[0].transform.position.y + 19, 0) , Quaternion.Euler(0,0,270)));
            stageDone = true;
        }

        //if (capsuleCollider.bounds.max.y > maxStartStageY - 6.0f)
        //{
        //    Debug.Log(capsuleCollider.bounds.max.y);
        //   // newStage = Instantiate(stageWalls[0]);
        //    //stageWalls[i].transform.position = wallPos + (direction * newSpacing);
        //}


        //if(transform.position.x /*transform.position.x*/ == 0 & transform.position.y == 0)
        //{
        //    trailRenderer.enabled = false;
        //}

        Sprite spriteActive = spriteRenderer.sprite;
        //DISTRUGGE SPRITE attiva: SpriteRenderer.Destroy(spriteActive);

        // ***JOYSTICK MOVEMENT***
        if (JoystickControl)
        {
            if (joystick.Vertical > 0)
            {
                Rigidbody.velocity = new Vector2(Rigidbody.velocity.x, jetpackForce * joystick.Vertical);
            }

            // **Check Facing**
            float posizFacing;

            if (joystick.Horizontal != 0)
            {
                posizFacing = joystick.Horizontal;
                if (posizFacing > 0)
                {
                    Rigidbody.velocity = new Vector2(fromLeftJetForce * joystick.Horizontal, Rigidbody.velocity.y);
                    //polygonCollider.transform.eulerAngles = new Vector3(0, 180, 0);
                    ChangeSprite(spriteArray[1]);
                }
                if (joystick.Horizontal < 0)
                {
                    Rigidbody.velocity = new Vector2(fromRightJetForce * joystick.Horizontal, Rigidbody.velocity.y);
                    //polygonCollider.transform.eulerAngles = new Vector3(0, 0, 0);
                    ChangeSprite(spriteArray[0]);
                }
            }
        }
        else
        {
            //FixedUpdate();
        }
    }

    void FixedUpdate()
    {
            // ***KEYBOARD LEFT MOVEMENT***
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                RaycastHit2D hit = Physics2D.Raycast(polygonCollider.bounds.min, Vector2.down, 0.1f);
                if (hit.collider != null)
                {
                    Debug.Log("Sono a terra.");
                    Debug.Log($"{hit.collider.gameObject.name}");

                    ChangeSprite(spriteArray[1]);

                    if (!leftRightInAirOnly)
                    {
                        ChangeSprite(spriteArray[1]);
                        //Più LENTO: Rigidbody.AddForce(Vector2.right * fromLeftJetForce, ForceMode2D.Force);
                        Rigidbody.velocity = new Vector2(fromLeftJetForce * Vector2.right.x, Rigidbody.velocity.y); //VELOCE
                        
                        //Rigidbody.AddForceAtPosition(Vector2.right*fromLeftJetForce, )
                    }
                }
                else
                {
                    Debug.Log("Sono in aria.");
                    ChangeSprite(spriteArray[1]);

                    //Più LENTO: Rigidbody.AddForce(Vector2.right * fromLeftJetForce, ForceMode2D.Force);
                    Rigidbody.velocity = new Vector2(fromLeftJetForce * Vector2.right.x, Rigidbody.velocity.y); //VELOCE

                }
            }

            // ***KEYBOARD RIGHT MOVEMENT***
            if (Input.GetKey(KeyCode.RightArrow))
            {
                RaycastHit2D hit = Physics2D.Raycast(polygonCollider.bounds.min, Vector2.down, 0.1f);
                if (hit.collider != null)
                {
                    Debug.Log("Sono a terra.");
                    Debug.Log($"{hit.collider.gameObject.name}");

                    ChangeSprite(spriteArray[0]);

                    if (!leftRightInAirOnly)
                    {
                        ChangeSprite(spriteArray[0]);

                        //Più LENTO: Rigidbody.AddForce(Vector2.left * fromRightJetForce, ForceMode2D.Force);
                        Rigidbody.velocity = new Vector2(fromRightJetForce * Vector2.left.x, Rigidbody.velocity.y); //VELOCE

                        //Rigidbody.AddForceAtPosition(Vector2.right*fromLeftJetForce, )
                    }
                }
                else
                {
                    Debug.Log("Sono in aria.");
                    ChangeSprite(spriteArray[0]);
                    //Più LENTO: Rigidbody.AddForce(Vector2.left * fromRightJetForce, ForceMode2D.Force);
                    Rigidbody.velocity = new Vector2(fromRightJetForce * Vector2.left.x, Rigidbody.velocity.y); //VELOCE
                }
            }

            // ***KEYBOARD JUMP***
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                upPressedDown = true;
                //bool grounded = false;
                //RaycastHit2D hit = Physics2D.CapsuleCast(transform.position, capsuleCollider.size, capsuleCollider.direction, 0f, Vector2.down, 6);
                //List<RaycastHit2D> hits = new List<RaycastHit2D>();

                RaycastHit2D hit = Physics2D.Raycast(polygonCollider.bounds.min, Vector2.down, 0.1f);
                if (hit.collider != null)
                {
                    Debug.Log("Sono a terra.");
                    Debug.Log($"{hit.collider.gameObject.name}");
                    Rigidbody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

                }
                else
                {
                    Debug.Log("Sono in aria.");
                }
            }
            else
            {
                upPressedDown = false;
            }

            // ***KEYBOARD PROPULSION***
            if (Input.GetKey(KeyCode.Space))
            {
                spacePressed = true;

                //Rigidbody.AddForce(Vector2.up * jetpackForce, ForceMode2D.Force);

                RaycastHit2D hit2 = Physics2D.Raycast(polygonCollider.bounds.min, Vector2.down, 0.1f);

                if (hit2.collider != null)
                {
                    Debug.Log("Sono a terra.");
                    if (!jetpackInAirOnly)
                    {
                        //Più LENTO: Rigidbody.AddForce(Vector2.up * jetpackForce, ForceMode2D.Force);
                        Rigidbody.velocity = new Vector2(Rigidbody.velocity.x, jetpackForce * Vector2.up.y); //VELOCE
                    }
                }
                else
                {
                    Debug.Log("Sono in aria.");
                    //Più LENTO: Rigidbody.AddForce(Vector2.up * jetpackForce, ForceMode2D.Force);
                    Rigidbody.velocity = new Vector2(Rigidbody.velocity.x, jetpackForce * Vector2.up.y); //VELOCE
                }
            }
            else
            {
                spacePressed = false;
            }
        
    }
}
