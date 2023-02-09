using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;
using Unity.Burst.CompilerServices;
using System;
using TMPro;
using System.Xml.Schema;

public class PlayerController : MonoBehaviour
{
    
    Rigidbody2D Rigidbody;
    CapsuleCollider2D capsuleCollider;
    SpriteRenderer spriteRenderer;
    //Slider fuelSlider;
    //PolygonCollider2D polygonCollider;

    #region ||>> OBJECT VARIABLES <<||

    //public Button HCmodeButton;
    //public Toggle HCmode;
    public Button buttonRestart;
    public GameObject Player;
    public FixedJoystick joystick;
    public Sprite[] spriteArray;
    public Camera mainCamera;
    public Slider fuelSlider;
    public TMP_Text fuelMeter;
    public GameObject Trail;
    public TMP_Text scoreText;
    //public TrailRenderer trailRenderer;
    //public GameObject Handle;
    //public GameObject playerDeathExplode;
    //public GameObject playerIdleLeft;
    //public GameObject playerIdleRight;
    #endregion

    #region ||>> INTERFACE VARIABLES <<||

    [Header("FORCE LEVELS")]
    public float jumpForce=6;
    public float jetpackForce=27;
    public float fromLeftJetForce=9;
    public float fromRightJetForce=9;
    [Header("FUEL LEVELS")]
    public float maxFuel=100f;
    public float fuelPerSecond = 45f;
    [Header("MOVEMENT MODES")]
    public bool joystickControl;
    public bool jetpackInAirOnly;
    public bool leftRightInAirOnly;
    [Header("OTHER")]
    public float deathFallingSpeed = 9.0f;
    public int secretNumber;
    //public bool HARDCORE;
    #endregion

    #region |> CONTROL VARIABLES <|

    private int platformCount=0;
    private bool inputPressed;
    private bool upPressedDown;
    private bool spacePressedDown;
    private bool spacePressed;
    private bool spacePressedUp;
    private bool leftPressed;
    private float remainingFuel;
    private float yLastCollidedPlatform = 0;
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

        remainingFuel = maxFuel;
        buttonRestart.onClick.AddListener(RestartGame);

        //if (gameObject.transform.position.y > 1)
        //{
        //    mainCamera.transform.position = new Vector3(0, 0, mainCamera.transform.position.z);
        //    gameObject.transform.position = new Vector3(0, 0, 0);
        //}
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    //public void HardcoreMode()
    //{
    //    if (HARDCORE == false)
    //        HARDCORE = true;
    //    else if (HARDCORE)
    //    { HARDCORE = false; }
    //}

    #region |||>>>PLAYER RESPAWN<<<|||

    private void SetPlayerActive()
    {
        gameObject.SetActive(true);
    }

    private void SetPlayerRespawn(float xRespawn, float yRespawn)
    {
        mainCamera.transform.position = new Vector3(xRespawn, yRespawn, mainCamera.transform.position.z);
        gameObject.transform.position = new Vector3(xRespawn, yRespawn, 0);
        remainingFuel = maxFuel;
        Debug.Log("FUEL " + remainingFuel);
        //faccio aspettare 1 secondo prima di riprendere i controlli movement
        Invoke("SetPlayerActive", 1);
    }
    #endregion

    #region |||>>>COLLISIONS CALCULATORS<<<|||

    #region|> Get colliders syntax <|

    //collision.gameObject.transform.parent.gameObject
    //prende parent come GameObject: oggettofiglio.transform.parent.gameObject
    //se invece: oggettofiglio.transform.parent --> è preso come Transform

    //prende child come GameObject: oggettoparent.transform.getchild(n).gameObject
    //se invece: oggettoparent.transform.getchild(n) --> è preso come Transform
    #endregion

    private float[] CollisionStageCalculator(float yCollision)
    {
        float yRespawn = 0f;
        float xRespawn = 0f;
        float[] xyRespawn = new float[2];
        //if (HARDCORE == false)
        //{
            //int collidedPlatStatus=0;
            Rigidbody.velocity = new Vector2(0, 0);
            int iterMax = WallGeneration.Instance.ListaStageGenerati.Count > 4 ? 4 : WallGeneration.Instance.ListaStageGenerati.Count;
            for (int i = 0; i < iterMax; i++)
            {
                //if (yCollision < WallGeneration.Instance.ListaStageGenerati[i].transform.position.y && WallGeneration.Instance.ListaStageGenerati[i].name.Contains("FIRST"))
                //{
                ////    yRespawn = 0f;
                ////    xRespawn = 0f;
                ////    SetPlayerRespawn(xRespawn, yRespawn);

                //}
                //if (yCollision > WallGeneration.Instance.ListaPlatforms[i].PlatformGenerated.transform.position.y)
                //{
                if (WallGeneration.Instance.ListaPlatforms[i].PlatformStatusIndex == 1)
                {
                    yRespawn = WallGeneration.Instance.ListaPlatforms[i].PlatformGenerated.transform.position.y +.5f;
                    xRespawn = WallGeneration.Instance.ListaStageGenerati[i].name.Contains("RIGHT") ? 1.5f : -1.5f;

                    //collidedPlatStatus = WallGeneration.Instance.ListaPlatforms[i].PlatformStatusIndex;
                    //collidedPlat = WallGeneration.Instance.ListaStageGenerati[i].transform.GetChild(0).gameObject;
                    break;
                }

                //}
                //else if(WallGeneration.Instance.ListaStageGenerati[i].name.Contains("FIRST"))
                //{
                //    break;
                //}
            }
            //SetPlayerRespawn(xRespawn, yRespawn);
        //}
        //else if (HARDCORE==true)
        //{
        //    RestartGame();
        //}
            xyRespawn[0] = xRespawn;
            xyRespawn[1] = yRespawn;

            //xyRespawn[2]=collidedPlatStatus;
            return xyRespawn;
    }

    private float[] CollisionPlatformCalculator(float yCollision)
    {
        float yRespawn = 0f;
        float xRespawn = 0f;
        float[] xyRespawn = new float[2];
         
        //int collidedPlatStatus = 0;
        //int platformListIndex = 0;
        Rigidbody.velocity = new Vector2(0, 0);
        int iterMax = WallGeneration.Instance.ListaStageGenerati.Count > 4 ? 4 : WallGeneration.Instance.ListaStageGenerati.Count;
        //if (yCollision != yCollisionLastPlatform)
        //{
            for (int i = iterMax - 1; i >= 0; i--)
            {

                if (WallGeneration.Instance.ListaPlatforms[i].PlatformStatusIndex == 0)
                {
                    platformCount++;
                    remainingFuel = maxFuel;
                    WallGeneration.Instance.ListaPlatforms[i].PlatformStatusIndex = 1;
                    xRespawn = WallGeneration.Instance.ListaStageGenerati[i].name.Contains("RIGHT") ? 1.5f : -1.5f;
                    yRespawn = WallGeneration.Instance.ListaPlatforms[i].PlatformGenerated.transform.position.y +.5f;
                    
                    //for (int z=i+1; z<iterMax; z++)
                    if (i < iterMax - 1)
                    {
                        for (int j = i + 1; j < iterMax - 1; j++)
                        {
                            WallGeneration.Instance.ListaPlatforms[j].PlatformStatusIndex = 2;
                        }
                    }
                    break;
                }


                //    if (yCollision > WallGeneration.Instance.ListaStageGenerati[i].transform.position.y)
                //{
                //    //if
                //    yRespawn = WallGeneration.Instance.ListaStageGenerati[i].transform.position.y + 1f;
                //    xRespawn = WallGeneration.Instance.ListaStageGenerati[i].name.Contains("RIGHT") ? 1.5f : -1.5f;
                //    collidedPlatStatus = WallGeneration.Instance.ListaPlatforms[i].PlatformStatusIndex;
                //    platformListIndex = i;
                //    //collidedPlat = WallGeneration.Instance.ListaStageGenerati[i].transform.GetChild(0).gameObject;
                //    break;
                //}
                //else if (WallGeneration.Instance.ListaStageGenerati[i].name.Contains("FIRST"))
                //{
                //    break;
                //}
            }
            //yLastCollidedPlatform=yCollision;
        //}
        xyRespawn[0] = xRespawn;
        xyRespawn[1] = yRespawn;
        
        return xyRespawn;
        //SetPlayerRespawn(xRespawn, yRespawn);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //if(capsuleCollider.CompareTag("walls"))
        //{Destroy(gameObject);}


        //SISTEMO COLLISION; USO STATUS PIATTAFORMA E NON POSIZIONE DEL COLLIDER
        float[] xyRespawn = new float[2];
        float[] collidedPlatformInfo = new float[2];
        float yCollision = collision.gameObject.transform.position.y;
        float yCollisionPlat = collision.gameObject.transform.parent.transform.position.y;

        //wall collision
        if (collision.gameObject.layer == 3)
        {
            //Instantiate(playerDeathExplode, transform.position, transform.rotation);
            //Animation.player
            gameObject.SetActive(false);
            xyRespawn = CollisionStageCalculator(yCollision);
            SetPlayerRespawn(xyRespawn[0], xyRespawn[1]);
        }

        //platform collison
        if (collision.gameObject.layer ==8)
        {
            //sistemo y rilevata per le piattaforme!!!
            //collision da sotto
            if (transform.position.y < yCollisionPlat)
            {
                gameObject.SetActive(false);
                xyRespawn = CollisionStageCalculator(yCollision);
                SetPlayerRespawn(xyRespawn[0], xyRespawn[1]);
            }

            //atterraggio da sopra
            else if (transform.position.y >= yCollisionPlat)
            {
                //morte too fast
                if (Mathf.Abs(collision.relativeVelocity.y) > deathFallingSpeed)
                {
                    gameObject.SetActive(false);
                    xyRespawn = CollisionStageCalculator(yCollision);
                    SetPlayerRespawn(xyRespawn[0], xyRespawn[1]);
                }
                //rileva se nuova piattaforma
                else if (yCollisionPlat != yLastCollidedPlatform)
                {
                    xyRespawn = CollisionPlatformCalculator(yCollisionPlat);
                    yLastCollidedPlatform = yCollisionPlat;
                    Debug.Log( collision.gameObject.transform.parent.gameObject.transform.parent.gameObject.name);

                    #region Rapid fuel recharge test
                    //FUNZIONA PER TEST RAPIDO
                    //collidedPlatformStatus = (int)xyRespawn[2];
                    //if(collidedPlatformStatus==0)
                    //{
                    //    remainingFuel=maxFuel;
                    //    collidedPlatformStatus=1;
                    //}
                    //FINE TEST
                    #endregion


                }
            }
        }
        //collision ground level
        if (collision.gameObject.layer == 6)
        {
            if (Mathf.Abs(collision.relativeVelocity.y) > deathFallingSpeed)
            {
                gameObject.SetActive(false);
                xyRespawn = CollisionStageCalculator(yCollision);
                SetPlayerRespawn(xyRespawn[0], xyRespawn[1]);
            }
        }
    }
    #endregion

    #region |||>>> TRAIL MANAGEMENT <<<|||

    private void DestroyTrail()
    {
        Trail.SetActive(false);
    }

    #region trail test
        //if(transform.position.x /*transform.position.x*/ == 0 & transform.position.y == 0)
        //{
        //    trailRenderer.enabled = false;
        //}

        //DISTRUGGE SPRITE attiva:
        //Sprite spriteActive = spriteRenderer.sprite;
        //SpriteRenderer.Destroy(spriteActive);
        #endregion
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
        //HCmodeButton.onClick.AddListener(HardcoreMode);
        //HCmode.onValueChanged.AddListener(HardcoreMode);

        #region |||>>> FUEL CONTROL <<<|||

        Debug.Log($"VELOCITY {Rigidbody.velocity.magnitude} || FUEL {remainingFuel}");
        remainingFuel = remainingFuel < 0 ? 0 : remainingFuel;

        fuelMeter.text = $"FUEL {(int)remainingFuel}";
        scoreText.text = $"SCORE {platformCount}";
        fuelSlider.value = remainingFuel / maxFuel;

        if(Input.GetKey(KeyCode.F))
        { remainingFuel = maxFuel; Debug.Log("FUEL " + remainingFuel); }

        if(secretNumber==666)
        { remainingFuel = maxFuel; }
        #endregion

        //if (isOnGround)
        //{
        //    rb.drag = drag;
        //}

        #region |||>>> JOYSTICK MOVEMENT <<<|||

        if (joystickControl&&remainingFuel>0)
        {
            if (joystick.Vertical < 0 || joystick.Horizontal != 0)
            { Trail.SetActive(true); }
            else
            {
                Invoke( "DestroyTrail", 0.4f);
            }

            if (joystick.Vertical < 0)
            { 
                //Alternativa Velocity:
                //{ Rigidbody.velocity = new Vector2(Rigidbody.velocity.x, jetpackForce * Mathf.Abs(joystick.Vertical)); }
                Rigidbody.AddForce(Vector2.up * jetpackForce*Time.deltaTime, ForceMode2D.Impulse); 
                remainingFuel -= fuelPerSecond*Time.deltaTime;
            }
            else 
            {  }
            //Check Facing
            float posizFacing;

            if (joystick.Horizontal != 0)
            {
                posizFacing = joystick.Horizontal;
                if (posizFacing > 0)
                {
                    Rigidbody.AddForce(Vector2.left* fromLeftJetForce * joystick.Horizontal * Time.deltaTime, ForceMode2D.Impulse);

                    //Rigidbody.velocity = new Vector2(fromLeftJetForce * -joystick.Horizontal, Rigidbody.velocity.y);
                    //polygonCollider.transform.eulerAngles = new Vector3(0, 180, 0);
                    ChangeSprite(spriteArray[0]);
                    //ChangeAnimationRight();
                }
                if (posizFacing < 0)
                {
                    Rigidbody.AddForce(Vector2.right*fromRightJetForce * -joystick.Horizontal * Time.deltaTime, ForceMode2D.Impulse);

                    //Rigidbody.velocity = new Vector2(fromRightJetForce * -joystick.Horizontal, Rigidbody.velocity.y);
                    //polygonCollider.transform.eulerAngles = new Vector3(0, 0, 0);
                    ChangeSprite(spriteArray[1]);
                    //ChangeAnimationLeft();
                }
            }
        }
        else
        { }
        #endregion

        #region |||>>> MUORE SE SCENDE SOTTO PIATTAFORMA RAGGIUNTA <<<|||

        if (WallGeneration.Instance.ListaStageGenerati.Count > 3)
        {
            float yMin;
            float xRespawn;
            float yRespawn;
            int i;
            for(i=0; i<WallGeneration.Instance.ListaStageGenerati.Count; i++)
            {
                if (WallGeneration.Instance.ListaPlatforms[i].PlatformStatusIndex == 1)
                {
                    yMin = WallGeneration.Instance.ListaPlatforms[i].PlatformGenerated.transform.position.y;
                    if (transform.position.y < yMin - 5f)
                    {
                        gameObject.SetActive(false);
                        yRespawn = yMin + 1f;
                        xRespawn = WallGeneration.Instance.ListaStageGenerati[i].name.Contains("RIGHT") ? 1.5f : -1.5f;
                        Rigidbody.velocity = new Vector2(0, 0);
                        SetPlayerRespawn(xRespawn, yRespawn);

                        //gameObject.transform.position = new Vector3(xRespawn, yRespawn, 0);
                        //yRespawn = WallGeneration.Instance.ListaStageGenerati[3].name.Contains("FIRST") ? 0f : WallGeneration.Instance.ListaStageGenerati[3].transform.position.y + 1f;
                        //xRespawn = WallGeneration.Instance.ListaStageGenerati[3].name.Contains("FIRST") ? 0f : WallGeneration.Instance.ListaStageGenerati[3].name.Contains("RIGHT") ? 1.5f : -1.5f;
                        //SetPlayerRespawn(xRespawn, yRespawn);
                    }
                    break;
                }
                else
                {
                }
            }
        }
        #endregion

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

                    Rigidbody.AddForce(Vector2.right * fromLeftJetForce * Time.deltaTime, ForceMode2D.Impulse);

                    //Rigidbody.velocity = new Vector2(fromLeftJetForce * Vector2.right.x, Rigidbody.velocity.y); //VELOCE

                    //Rigidbody.AddForceAtPosition(Vector2.right*fromLeftJetForce, )
                }
            }
            else
            {
                Debug.Log("Sono in aria.");
                ChangeSprite(spriteArray[1]);
                //ChangeAnimationRight();

                Rigidbody.AddForce(Vector2.right * fromLeftJetForce * Time.deltaTime, ForceMode2D.Impulse);

                //Rigidbody.velocity = new Vector2(fromLeftJetForce * Vector2.right.x, Rigidbody.velocity.y); //VELOCE
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

                    Rigidbody.AddForce(Vector2.left * fromRightJetForce * Time.deltaTime, ForceMode2D.Impulse);

                    //Rigidbody.velocity = new Vector2(fromRightJetForce * Vector2.left.x, Rigidbody.velocity.y); //VELOCE

                    //Rigidbody.AddForceAtPosition(Vector2.right*fromLeftJetForce, )
                }
            }
            else
            {
                Debug.Log("Sono in aria.");
                ChangeSprite(spriteArray[0]);
                //ChangeAnimationLeft();

                Rigidbody.AddForce(Vector2.left * fromRightJetForce * Time.deltaTime, ForceMode2D.Impulse);

                //Rigidbody.velocity = new Vector2(fromRightJetForce * Vector2.left.x, Rigidbody.velocity.y); //VELOCE
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
            if (remainingFuel > 0)
            {
                if (hit2.collider != null)
                {
                    Debug.Log("Sono a terra.");
                    if (!jetpackInAirOnly)
                    {
                        Rigidbody.AddForce(Vector2.up * jetpackForce * Time.deltaTime, ForceMode2D.Impulse);
                        remainingFuel -= fuelPerSecond * Time.deltaTime;

                        //Rigidbody.velocity = new Vector2(Rigidbody.velocity.x, jetpackForce * Vector2.up.y); //VELOCE
                    }
                }
                else
                {
                    Debug.Log("Sono in aria.");

                    Rigidbody.AddForce(Vector2.up * jetpackForce * Time.deltaTime, ForceMode2D.Impulse);
                    remainingFuel -= fuelPerSecond * Time.deltaTime;

                    //Rigidbody.velocity = new Vector2(Rigidbody.velocity.x, jetpackForce * Vector2.up.y); //VELOCE
                }
            }
            else
            {
            }
        }
        else
        { spacePressed = false; }
        #endregion
    }
}
