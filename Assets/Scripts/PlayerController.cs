using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;
using Unity.Burst.CompilerServices;

public class PlayerController : MonoBehaviour
{
    
    Rigidbody2D Rigidbody;
    CapsuleCollider2D capsuleCollider;
    SpriteRenderer spriteRenderer;
    //PolygonCollider2D polygonCollider;

    #region ||>> OBJECT VARIABLES <<||
    public GameObject Player;
    public FixedJoystick joystick;
    public Sprite[] spriteArray;
    public Camera mainCamera;

    //public TrailRenderer trailRenderer;
    //public GameObject Handle;
    //public GameObject playerDeathExplode;
    //public GameObject playerIdleLeft;
    //public GameObject playerIdleRight;
    #endregion

    #region ||>> INTERFACE VARIABLES <<||
    public bool JoystickControl;
    public bool jetpackInAirOnly;
    public bool leftRightInAirOnly;
    public float jumpForce;
    public float jetpackForce;
    public float fromLeftJetForce;
    public float fromRightJetForce;
    #endregion

    #region |> CONTROL VARIABLES <|

    private bool inputPressed;
    private bool upPressedDown;
    private bool spacePressedDown;
    private bool spacePressed;
    private bool spacePressedUp;
    private bool leftPressed;
    #endregion

    void Start()
    {
        //GETTING PLAYER COMPONENTS
        spriteRenderer = GetComponent<SpriteRenderer>();
        Rigidbody = GetComponent<Rigidbody2D>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();

        //Player = GetComponent<GameObject>();
        //polygonCollider = GetComponent<PolygonCollider2D>();
        //trailRenderer = GetComponentInChildren<TrailRenderer>();
        //rectTransform=Handle.GetComponent<RectTransform>(); 
    }

    #region |||>>>PLAYER RESPAWN<<<|||

    private void SetPlayerActive()
    {
        gameObject.SetActive(true);
    }

    private void SetPlayerRespawn(float xRespawn, float yRespawn)
    {
        mainCamera.transform.position = new Vector3(xRespawn, yRespawn, mainCamera.transform.position.z);
        gameObject.transform.position = new Vector3(xRespawn, yRespawn, 0);
        Invoke("SetPlayerActive", 1);
    }
    #endregion

    #region |||>>>COLLISIONS DEATHS<<<|||

    private void CollisionStageCalculator(float yCollision)
    {
        float yRespawn;
        float xRespawn;
        gameObject.SetActive(false);
        Rigidbody.velocity = new Vector2(0, 0);
        int iterMax = WallGeneration.Instance.listaStageGenerati.Count > 3 ? 3 : WallGeneration.Instance.listaStageGenerati.Count;
        for (int i = 0; i < iterMax; i++)
        {

            if (yCollision < WallGeneration.Instance.listaStageGenerati[i].transform.position.y && WallGeneration.Instance.listaStageGenerati[i].name.Contains("FIRST"))
            {
                yRespawn = 0f;
                xRespawn = 0f;
                SetPlayerRespawn(xRespawn, yRespawn);
                break;
            }
            else if (yCollision > WallGeneration.Instance.listaStageGenerati[i].transform.position.y)
            {
                yRespawn = WallGeneration.Instance.listaStageGenerati[i].transform.position.y + 1f;
                xRespawn = WallGeneration.Instance.listaStageGenerati[i].name.Contains("RIGHT") ? 1.5f : -1.5f;
                SetPlayerRespawn(xRespawn, yRespawn);
                break;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //if(capsuleCollider.CompareTag("walls"))
        //{Destroy(gameObject);}
        
        float yCollision;
        yCollision = collision.transform.position.y;

        //wall collision
        if (collision.gameObject.layer == 3)
        {
            //Instantiate(playerDeathExplode, transform.position, transform.rotation);
            //Animation.player
            CollisionStageCalculator(yCollision);
        }

        //platform collison
        if (collision.gameObject.layer ==8)
        {
            //sistemo y rilevata per le piattaforme!!!
            //collision da sotto
            if (transform.position.y < collision.transform.position.y)
            {
                CollisionStageCalculator(yCollision);
            }

            //atterraggio da sopra
            else if (transform.position.y >= collision.transform.position.y)
            {
                if (Mathf.Abs(collision.relativeVelocity.y) > 10.0f)
                {
                    CollisionStageCalculator(yCollision);
                }
            }
        }
    }
    #endregion

    #region |||>>> CAMBIA SPRITE DIREZIONE MOVIMENTO <<<|||
    private void ChangeSprite(Sprite _newSprite)
    {
        spriteRenderer.sprite = _newSprite;
    }
    #endregion

    //void OnBecameInvisible()
    //{
    //    Destroy(gameObject);
    //}

    #region changeAnim test
    //void ChangeAnimationRight()
    //{
    //    GetComponentInChildren<GameObject>().SetActive(false);
    //    //Destroy(playerIdleLeft);
    //    Instantiate(playerIdleRight, transform);
    //}
    //void ChangeAnimationLeft()
    //{
    //    Destroy(playerIdleRight);
    //    Instantiate(playerIdleLeft, transform);
    //}
    #endregion

    void Update()
    {
        Debug.Log("VELOCITY " + Rigidbody.velocity.magnitude);

        #region |||>>> JOYSTICK MOVEMENT <<<|||

        if (JoystickControl)
        {
            if (joystick.Vertical > 0)
            { Rigidbody.velocity = new Vector2(Rigidbody.velocity.x, jetpackForce * joystick.Vertical); }

            //Check Facing
            float posizFacing;

            if (joystick.Horizontal != 0)
            {
                posizFacing = joystick.Horizontal;
                if (posizFacing > 0)
                {
                    Rigidbody.velocity = new Vector2(fromLeftJetForce * joystick.Horizontal, Rigidbody.velocity.y);
                    //polygonCollider.transform.eulerAngles = new Vector3(0, 180, 0);
                    ChangeSprite(spriteArray[1]);
                    //ChangeAnimationRight();
                }
                if (posizFacing < 0)
                {
                    Rigidbody.velocity = new Vector2(fromRightJetForce * joystick.Horizontal, Rigidbody.velocity.y);
                    //polygonCollider.transform.eulerAngles = new Vector3(0, 0, 0);
                    ChangeSprite(spriteArray[0]);
                    //ChangeAnimationLeft();
                }
            }
        }
        else
        { }
        #endregion

        //Morte se scende di troppi stage(sistemare)
        if(WallGeneration.Instance.listaStageGenerati.Count > 3)
        {
            float yRespawn;
            float xRespawn;
            if (transform.position.y < WallGeneration.Instance.listaStageGenerati[3].transform.position.y-5.5f)
            {
                gameObject.SetActive(false);
                Rigidbody.velocity = new Vector2(0, 0);
                //gameObject.transform.position = new Vector3(xRespawn, yRespawn, 0);
                yRespawn = WallGeneration.Instance.listaStageGenerati[3].name.Contains("FIRST") ? 0f : WallGeneration.Instance.listaStageGenerati[3].transform.position.y + 1f;
                xRespawn = WallGeneration.Instance.listaStageGenerati[3].name.Contains("FIRST") ? 0f : WallGeneration.Instance.listaStageGenerati[3].name.Contains("RIGHT") ? 1.5f : -1.5f;
                SetPlayerRespawn(xRespawn, yRespawn);
            }
        }

        #region reycast test
        //if (Physics.Raycast(transform.position, Vector3.down, out hit, 100, 1 << 7))
        //{
        //    hit.collider.GetComponent<WallGeneration>().method();
        //}

        //RaycastHit2D hit = Physics2D.Raycast(capsuleCollider.bounds.min, Vector2.down, 0.1f);
        //if (hit.collider != null)
        //{
        //    Debug.Log("Sono a terra.");
        //}

        //RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector3.down, 0.1f, 6);
        //if (hit)
        //{
        //    hit.collider.IsTouchingLayers(3);
        //}

        //RaycastHit2D hitz = Physics2D.Raycast(capsuleCollider.bounds.size, Vector2.one, 0.1f);
        ////if (hitz.collider != null)

        //    if (hitz.collider != null&& Rigidbody.velocity.magnitude>10)
        //{
        //    this.gameObject.SetActive(false);
        //}
        #endregion

        #region trail test
        //if(transform.position.x /*transform.position.x*/ == 0 & transform.position.y == 0)
        //{
        //    trailRenderer.enabled = false;
        //}

        //DISTRUGGE SPRITE attiva:
        //Sprite spriteActive = spriteRenderer.sprite;
        //SpriteRenderer.Destroy(spriteActive);
        #endregion

    }

    void FixedUpdate()
    {
        #region |||>>> KEYBOARD MOVEMENT <<<|||
        
        // ||>> KEYBOARD LEFT <<||
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            RaycastHit2D hit = Physics2D.Raycast(capsuleCollider.bounds.min, Vector2.down, 0.1f);
            if (hit.collider != null)
            {
                Debug.Log("Sono a terra.");
                Debug.Log($"{hit.collider.gameObject.name}");

                ChangeSprite(spriteArray[1]);
                //ChangeAnimationRight();

                if (!leftRightInAirOnly)
                {
                    ChangeSprite(spriteArray[1]);
                    //ChangeAnimationRight();

                    //Più LENTO: Rigidbody.AddForce(Vector2.right * fromLeftJetForce, ForceMode2D.Force);
                    Rigidbody.velocity = new Vector2(fromLeftJetForce * Vector2.right.x, Rigidbody.velocity.y); //VELOCE

                    //Rigidbody.AddForceAtPosition(Vector2.right*fromLeftJetForce, )
                }
            }
            else
            {
                Debug.Log("Sono in aria.");
                ChangeSprite(spriteArray[1]);
                //ChangeAnimationRight();

                //Più LENTO: Rigidbody.AddForce(Vector2.right * fromLeftJetForce, ForceMode2D.Force);
                Rigidbody.velocity = new Vector2(fromLeftJetForce * Vector2.right.x, Rigidbody.velocity.y); //VELOCE
            }
        }

        // ||>>KEYBOARD RIGHT<<||
        if (Input.GetKey(KeyCode.RightArrow))
        {
            RaycastHit2D hit = Physics2D.Raycast(capsuleCollider.bounds.min, Vector2.down, 0.1f);
            if (hit.collider != null)
            {
                Debug.Log("Sono a terra.");
                Debug.Log($"{hit.collider.gameObject.name}");

                ChangeSprite(spriteArray[0]);
                //ChangeAnimationLeft();

                if (!leftRightInAirOnly)
                {
                    ChangeSprite(spriteArray[0]);
                    //ChangeAnimationLeft();

                    //Più LENTO: Rigidbody.AddForce(Vector2.left * fromRightJetForce, ForceMode2D.Force);
                    Rigidbody.velocity = new Vector2(fromRightJetForce * Vector2.left.x, Rigidbody.velocity.y); //VELOCE

                    //Rigidbody.AddForceAtPosition(Vector2.right*fromLeftJetForce, )
                }
            }
            else
            {
                Debug.Log("Sono in aria.");
                ChangeSprite(spriteArray[0]);
                //ChangeAnimationLeft();
                //Più LENTO: Rigidbody.AddForce(Vector2.left * fromRightJetForce, ForceMode2D.Force);
                Rigidbody.velocity = new Vector2(fromRightJetForce * Vector2.left.x, Rigidbody.velocity.y); //VELOCE
            }
        }

        // ||>>KEYBOARD JUMP<<||
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
            { Debug.Log("Sono in aria."); }
        }
        else
        { upPressedDown = false; }

        // ||>>KEYBOARD PROPULSION<<||
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
        { spacePressed = false; }
        #endregion
    }
}
