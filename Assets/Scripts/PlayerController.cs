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
    SpriteRenderer spriteRenderer;
    //TrailRenderer trailRenderer;
    public FixedJoystick joystick;
    public Sprite[] spriteArray;
    public GameObject[] stageWalls;

    //public class GeneratedWalls
    //{
    //    //public GameObject wall;
    //}

    //public GameObject Handle;
    //RectTransform rectTransform;

    bool inputPressed;
    private bool upPressedDown;
    private bool spacePressedDown;
    private bool spacePressed;
    private bool spacePressedUp;
    private bool leftPressed;
    private bool firstStagePassed = false;
    private List<GameObject> listaStageGenerati = new List<GameObject>();
    private bool stageDone = true;

    public bool JoystickControl;
    public float jumpForce;
    public float jetpackForce;
    public float fromLeftJetForce;
    public float fromRightJetForce;
    public bool jetpackInAirOnly;
    public bool leftRightInAirOnly;

    public GameObject startingStage;

    //private int rng;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        Rigidbody = GetComponent<Rigidbody2D>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();

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

        
        //float maxY;
        //{

            Collider2D[] stageColliders; /*= new Collider2D[4];*/
            stageColliders = startingStage.GetComponentsInChildren<Collider2D>();/*GetComponent<Collider2D>().bounds.max.y;*/
            //float[] maxYs = new float[stageColliders.Length];
            float maxY = stageColliders[0].transform.position.y;
            string collName = stageColliders[0].name;
            for (int i = 1; i < stageColliders.Length; i++)
            {  /*maxYs[i] =*/
                Debug.Log(stageColliders[i].transform.position.y);
                if (stageColliders[i].transform.position.y >= maxY)
                {
                    maxY = stageColliders[i].transform.position.y;
                    collName = stageColliders[i].name;
                }
                else
                {

                }
                Debug.Log(stageColliders[i].name + " = " + maxY);
            }
            Debug.Log("MAX Y" + collName + maxY);
            Debug.Log(capsuleCollider.bounds.max.y);

            //listaStageGenerati.Add(startingStage);
            //if ((maxY - capsuleCollider.bounds.max.y) < 5)
            //    firstStagePassed = false;

            if (firstStagePassed == false)
            {
                listaStageGenerati.Add(Instantiate(stageWalls[Random.Range(0, 2)], new Vector3(0, startingStage.transform.position.y + 10.7f, 0), Quaternion.identity));
                firstStagePassed = true;
            }
        //}

        //else
        //{

        //    Collider2D[] newStageColliders; /*= new Collider2D[4];*/
        //    newStageColliders = listaStageGenerati[0].GetComponentsInChildren<Collider2D>();/*GetComponent<Collider2D>().bounds.max.y;*/
        ////float[] maxYs = new float[stageColliders.Length];
        //    float newMaxY = newStageColliders[0].transform.position.y;
        //    string newCollName = newStageColliders[0].name;
        //    for (int i = 1; i < newStageColliders.Length; i++)
        //    {  /*maxYs[i] =*/
        //        Debug.Log(newStageColliders[i].transform.position.y);
        //        if (newStageColliders[i].transform.position.y >= newMaxY)
        //        {
        //            newMaxY = newStageColliders[i].transform.position.y;
        //            newCollName = newStageColliders[i].name;
        //        }
        //        else
        //        {

        //        }
        //        Debug.Log(newStageColliders[i].name + " = " + newMaxY);
        //    }

        //    if ((newMaxY - capsuleCollider.bounds.max.y) < 5 && stageDone==false)
        //    {
        //        listaStageGenerati.Insert(0, Instantiate(stageWalls[Random.Range(0, 2)], new Vector3(0, listaStageGenerati[0].transform.position.y + 10.5f /**(1+listaStageGenerati.Count)*/, 0), Quaternion.identity));
        //        stageDone = true;
        //    }

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
        //    else
        //    {

        //    }
        //    Debug.Log(newStageColliders[i].name + " = " + newMaxY);
        //}

        if ((listaStageGenerati[0].transform.position.y - capsuleCollider.bounds.max.y) < 5)
        { stageDone = false; }
        if ( stageDone == false)
        {
            listaStageGenerati.Insert(0, Instantiate(stageWalls[Random.Range(0, 2)], new Vector3(0, listaStageGenerati[0].transform.position.y + 10.5f /**(1+listaStageGenerati.Count)*/, 0), Quaternion.identity));
            stageDone = true;
        }
        //if (listaStageGenerati[0].transform.position.y==)
        else
        {

        }





        //}

        //if (())

        //if (capsuleCollider.bounds.max.y > maxStartStageY - 6.0f)
        //{
        //    Debug.Log(capsuleCollider.bounds.max.y);

        //   // newStage = Instantiate(stageWalls[0]);
        //    //balls[i].transform.position = ballPos + (direction * newSpacing);


        //}

        Sprite spriteActive = spriteRenderer.sprite;
        //DISTRUGGE SPRITE attiva: SpriteRenderer.Destroy(spriteActive);

        //if(rectTransform.position.x /*transform.position.x*/ == 0 & rectTransform.position.y == 0)
        //{
        //    trailRenderer.enabled = false;
        //}


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
                    ChangeSprite(spriteArray[1]);
                }
                if (joystick.Horizontal < 0)
                {
                    Rigidbody.velocity = new Vector2(fromRightJetForce * joystick.Horizontal, Rigidbody.velocity.y);
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
                RaycastHit2D hit = Physics2D.Raycast(capsuleCollider.bounds.min, Vector2.down, 0.1f);
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
                RaycastHit2D hit = Physics2D.Raycast(capsuleCollider.bounds.min, Vector2.down, 0.1f);
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

                RaycastHit2D hit = Physics2D.Raycast(capsuleCollider.bounds.min, Vector2.down, 0.1f);
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

                RaycastHit2D hit2 = Physics2D.Raycast(capsuleCollider.bounds.min, Vector2.down, 0.1f);

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
