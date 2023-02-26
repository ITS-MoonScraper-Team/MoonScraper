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
using JetBrains.Annotations;
using System.Runtime.CompilerServices;
using DG.Tweening;
//using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    #region |||>>> PLAYER COMPONENTS <<<|||

    Rigidbody2D Rigidbody;
    CapsuleCollider2D capsuleCollider;
    SpriteRenderer spriteRenderer;
    Color colorCharacter;
    //Slider fuelSlider;
    //PolygonCollider2D polygonCollider;
    #endregion

    #region |||>> PUBLIC GAME OBJECTS <<|||

    public GameObject Player;
    public GameObject Trail;
    public Camera mainCamera;
    public FixedJoystick joystick;
    public FloatingJoystick joyFloat;
    public Sprite[] spriteArray;
    public Slider fuelSlider;
    public Button buttonRestart;
    public TMP_Text fuelMeter;
    public TMP_Text scoreText;
    public TMP_Text GameOverText;
    public TMP_Text livesText;
    public TMP_Text deadText;
    public TMP_Text outOfFuelText;
    public GameObject ExplosionTemplate;
    public Image FuelCircleProgression;
    //public Button HCmodeButton;
    //public Slider maxLivesSlider;
    //public TrailRenderer trailRenderer;
    //public GameObject Handle;
    //public GameObject playerIdleLeft;
    //public GameObject playerIdleRight;
    #endregion

    #region |||>> INTERFACE VARIABLES <<|||

    [Header("FORCE LEVELS")] //BALANCING: jetForce=25, fromLeftRight=13
    [Tooltip("BALANCE: jetForce = 25, from Left/Right = 13")]
    public float jumpForce=6;
    public float jetpackForce=25;
    public float fromLeftJetForce=13;
    public float fromRightJetForce=13;
    [Header("FUEL LEVELS")] //BALANCING: fuel=100, fuelPerSec=49
    [Tooltip("BALANCE: fuel=100, fuelPerSec=49")]
    public float maxFuel=100f;
    public float fuelPerSecond = 49f;
    [Header("MOVEMENT MODES")]
    public bool joystickControl;
    public bool jetpackInAirOnly;
    public bool leftRightInAirOnly;
    public bool joystickYaxisInverted=true;
    public bool joystickXaxisInverted=true;
    [Header("OTHER")]
    public float deathFallingSpeed = 9.0f;
    public int secretNumber;
    //public bool HARDCORE;
    #endregion

    #region |||>>> PROGRAM VARIABLES <<<|||

    private float remainingFuel;
    private float yLastReachedPlatform = 0;
    private float xRespawn;
    private float yRespawn;
    private int deathMessageNum;
    private Vector2 posCollision;
    private float deathTimeDuration = .5f;
    //private float[] xyRespawn = new float[2];
    #endregion

    #region |> CONTROL VARIABLES <|

    private int livesCount;
    private int livesMax;
    private bool livesActive;
    private int platformCount=0;
    private bool inputPressed;
    private bool upPressedDown;
    private bool spacePressedDown;
    private bool spacePressed;
    private bool spacePressedUp;
    private bool leftPressed;
    private bool joystickYaxisCondition;
    private bool joystickXaxisCondition;
    private Sprite directionLeftSprite;
    private Sprite directionRightSprite;
    private bool isFalling;
    private Color currentWarnColor;
    private bool isAlive;
    #endregion

    void Start()
    {
        //GETTING PLAYER COMPONENTS
        spriteRenderer = GetComponent<SpriteRenderer>();
        Rigidbody = GetComponent<Rigidbody2D>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        colorCharacter = spriteRenderer.color;
        //polygonCollider = GetComponent<PolygonCollider2D>();
        //trailRenderer = GetComponentInChildren<TrailRenderer>();
        //rectTransform=Handle.GetComponent<RectTransform>();
        directionLeftSprite= spriteArray[0];
        directionRightSprite= spriteArray[1];

        //SETTING PLAYER DATA
        remainingFuel = maxFuel;
        livesMax = MainMenu.InstanceMenu.LivesMax;
        livesCount = livesMax;  

        if (livesMax != 11)
        { livesActive = true; }
        else { livesActive = false; }

        xRespawn= gameObject.transform.position.x;
        yRespawn= gameObject.transform.position.y;
        
        buttonRestart.onClick.AddListener(RestartGame);

        //if (gameObject.transform.position.y > 1)
        //{
        //    mainCamera.transform.position = new Vector3(0, 0, mainCamera.transform.position.z);
        //    gameObject.transform.position = new Vector3(0, 0, 0);
        //}
    }

    #region |||>>> GAME OVER AND RESTART <<<|||

    //RESTART GAME CON SCENE NAME
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void GameOver()
    {
        GameOverMessage();
        //livesText.text = "LIVES 0";
        Invoke("GameOverMessOff", 2f);
        Invoke("RestartGame", 2f);
    }

    private void GameOverMessage()
    { GameOverText.gameObject.SetActive(true); }
    
    private void GameOverMessOff()
    { GameOverText.gameObject.SetActive(false); }
    #endregion

    #region |||>>> DEATH AND RESPAWN LOGIC <<<|||

    private string DeathLogic()
    {
        if (livesActive == false)
        {
            platformCount = 0;
        }
        SetPlayerInactiveLoseLife();
        CollisionStageCalculator();
        //SetPlayerRespawn(/*xyRespawn[0], xyRespawn[1]*/);
        string condition = DeathMessageType();
        return condition;
    }

    private void SetPlayerInactiveLoseLife()
    {
        //StartCoroutine(SetPlayerInactiveCorutine());

        //FX MORTE

        gameObject.SetActive(false);
        GameObject explosion = Instantiate(ExplosionTemplate, posCollision, Quaternion.identity);
        //explosion.Emit(60);
        if(SoundManager.AudioON)
        SoundManager.PlaySound("playerDeath");

        Destroy(explosion, .4f);

        //apapre scritta: morto
        if (livesActive == true)
        {
            livesCount--;
            //controllo particella da codice
        }

    }
    //IEnumerator SetPlayerInactiveCorutine()
    //{
    //    //FX MORTE
    //    //apapre scritta: morto
    //    if (livesActive == true)
    //    {
    //        gameObject.SetActive(false);
    //        livesCount--;
    //        ParticleSystem ps = Instantiate(ExplosionTemplate, posCollision, Quaternion.identity);
    //        //controllo particella da codice
    //        ps.Emit(60);
    //        yield return new WaitForSecondsRealtime(.4f);//NON deleta ps
    //        Destroy(ps.gameObject);
    //    }
    //    else //GAMEOVER
    //    {
    //        gameObject.SetActive(false);
    //        ParticleSystem ps = Instantiate(ExplosionTemplate, posCollision, Quaternion.identity);
    //        //controllo particella da codice
    //        ps.Emit(60);
    //        yield return new WaitForSecondsRealtime(.4f);
    //        Destroy(ps.gameObject);
    //    }
    //}

    private void SetPlayerRespawn(/*float xRespawn, float yRespawn*/)
    {
        isAlive = true;
        //mainCamera.transform.position = new Vector3(0, yRespawn, mainCamera.transform.position.z);
        gameObject.transform.position = new Vector3(xRespawn, yRespawn, 0);
        remainingFuel = maxFuel;
        //Debug.Log("FUEL " + remainingFuel);
        //faccio aspettare 1 secondo prima di riprendere i controlli movement
    }

    private string DeathMessageType()
    {
        string condition;
        if (livesCount == 0)
        { condition = "GameOver"; }
        else
        {
            if (deathMessageNum == 0)
            {
                deadText.gameObject.SetActive(true);
                //condition = "SetPlayerActive";
            }
            else
            {
                outOfFuelText.gameObject.SetActive(true);
            }
                condition = "SetPlayerActive";
        }
        return condition;
    }

    private void SetPlayerActive()
    {
        if(deadText.IsActive())
        deadText.gameObject.SetActive(false);

        if(outOfFuelText.IsActive())
        outOfFuelText.gameObject.SetActive(false);

        //StartCoroutine(SetPlayerActiveDelay());
        SetPlayerRespawn();
        gameObject.SetActive(true);
    }

    //IEnumerator SetPlayerActiveDelay()
    //{
    //    yield return new WaitForSeconds ()
    //    SetPlayerRespawn();
    //    gameObject.SetActive(true);
    //}

    //private string RespawnCondition(int _deathMessageType)
    //{
    //    string resolution = _deathMessageType == 0 ? "GameOver" : "SetPlayerActive";
    //    //Invoke(solution, 1f);
    //    return resolution;
    //}
    #endregion

    #region |||>>> COLLISIONS CALCULATORS <<<|||

    #region >> syntax info: getting child/parents <<

    ///collision.gameObject.transform.parent.gameObject
    ///Prende parent come GameObject: oggettofiglio.transform.parent.gameObject
    ///se invece: oggettofiglio.transform.parent --> è preso come Transform;
    ///
    ///Prende child come GameObject: oggettoparent.transform.getchild(n).gameObject
    ///se invece: oggettoparent.transform.getchild(n) --> è preso come Transform.
    #endregion

    private void CollisionStageCalculator(/*vector2 _posCollision*/)
    {
        //Rigidbody.velocity = new Vector2(0, 0);
        int iterMax = WallGeneration.Instance.ListaStageGenerati.Count > 4 ? 4 : WallGeneration.Instance.ListaStageGenerati.Count;
        for (int i = 0; i < iterMax; i++)
        {
            if (WallGeneration.Instance.ListaPlatforms[i].PlatformStatusIndex == 1)
            {
                yRespawn = WallGeneration.Instance.ListaPlatforms[i].PlatformGenerated.transform.position.y +.3f;
                xRespawn = WallGeneration.Instance.ListaStageGenerati[i].name.Contains("RIGHT") ? 1.5f : -1.5f;
                break;
            }
            else
            {
                yRespawn = .3f;
                xRespawn = 0f;
            }
        }
    }

    private void CollisionPlatformCalculator(/*Transform _posCollision*/)
    {
        //float yRespawn = 0f;
        //float xRespawn = 0f;
        //float[] xyRespawn = new float[2];
        //int collidedPlatStatus = 0;
        //int platformListIndex = 0;
        //Rigidbody.velocity = new Vector2(0, 0);
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
                    WallGeneration.Instance.ListaPlatforms[i].PlatformGenerated.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().color = new Color(200f, 0f, 0f,255f);
                    WallGeneration.Instance.ListaPlatforms[i].PlatformGenerated.transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>().color = new Color(200f, 0f, 0f, 255f);
                    xRespawn = WallGeneration.Instance.ListaStageGenerati[i].name.Contains("RIGHT") ? 1.5f : -1.5f;
                    yRespawn = WallGeneration.Instance.ListaPlatforms[i].PlatformGenerated.transform.position.y +.3f;
                    
                    //mette status "2" = "passed" alle piattaforme vecchie
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
            }
            //yLastCollidedPlatform=yCollision;
        //}
        //xyRespawn[0] = xRespawn;
        //xyRespawn[1] = yRespawn;
        //return xyRespawn;
        //SetPlayerRespawn(xRespawn, yRespawn);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //if(capsuleCollider.CompareTag("walls"))
        //{Destroy(gameObject);}

        //SISTEMO COLLISION; USO STATUS PIATTAFORMA E NON POSIZIONE DEL COLLIDER
        //float[] xyRespawn = new float[2];
        posCollision = transform.position;
        float yCollision = collision.gameObject.transform.position.y;
        float yCollisionPlat = collision.gameObject.transform.parent.transform.position.y;
        Rigidbody.velocity = new Vector2(0, 0);

        //wall collision
        if (collision.gameObject.layer == 3)

        //prende posizione player alla collisione, occhio che conta anche allo start quando è fermo al ground
        {
            isAlive = false;
            deathMessageNum = 0;
            Invoke(DeathLogic(), deathTimeDuration);
        }

        //platform collison
        if (collision.gameObject.layer ==8)
        {
            //sistemo y rilevata per le piattaforme!!!

            //collision da sotto
            if (transform.position.y < yCollisionPlat)
            {
                isAlive = false;
                deathMessageNum = 0;
                //DeadMessage();
                Invoke(DeathLogic(), deathTimeDuration);
                //SetPlayerInactiveLoseLife();
                //xyRespawn = CollisionStageCalculator(yCollision);
                //SetPlayerRespawn(xyRespawn[0], xyRespawn[1]);
                //RespawnCondition();
            }

            //atterraggio da sopra
            else if (transform.position.y >= yCollisionPlat)
            {
                //morte too fast
                if (Mathf.Abs(collision.relativeVelocity.y) > deathFallingSpeed)
                {
                    isAlive = false;
                    deathMessageNum = 0;
                    Invoke(DeathLogic(), deathTimeDuration);
                }
                //respawn se finsici benza su stessa piatt
                else if(yCollisionPlat==yLastReachedPlatform&&remainingFuel < 1)
                {
                    isAlive = false;
                    deathMessageNum = 1;
                    Invoke(DeathLogic(), deathTimeDuration);
                }
                //rileva se nuova piattaforma e aumenta score
                else if (yCollisionPlat != yLastReachedPlatform)
                {
                    CollisionPlatformCalculator(/*yCollisionPlat*/);
                    yLastReachedPlatform = yCollisionPlat;
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
                isAlive = false;
                deathMessageNum = 0;
                Invoke(DeathLogic(), deathTimeDuration);

            }
            //fine benza
            else if (remainingFuel < 1)
            {
                isAlive = false;
                deathMessageNum = 1;
                Invoke(DeathLogic(), deathTimeDuration);
            }
        }
    }
    #endregion

    #region |||>>> TRAIL MANAGEMENT <<<|||

    private void DestroyTrail()
    {
        Trail.SetActive(false);
    }
    private void DestroyParticleFX()
    {
        gameObject.transform.GetChild(2).gameObject.SetActive(false);
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

    #region ||>> VELOCITY WARNING COROUTINES <<||

    //CTRL+RR cambia nome in tutto documento

    IEnumerator VelocityWarnCoroutine()
    {
        //spriteRenderer.transform.DOShakePosition(.25f);

        ///SISTEMA COROUTINE CHE STARTA PIU'VOLTE-SISTEMA COLOR CHE RIMANE RED
        ///
       
        isFalling = true;
        while (isFalling)
        {
            
            spriteRenderer.DOColor(currentWarnColor, .05f);
            yield return new WaitForSeconds(.1f);
            spriteRenderer.DOColor(colorCharacter, .05f);
            yield return new WaitForSeconds(.1f);
        }
        
    }
    //IEnumerator VelocityRedWarnCoroutine()
    //{
    //    spriteRenderer.DOColor(Color.red, .05f);
    //    yield return new WaitForSeconds(.05f);
    //    spriteRenderer.DOColor(colorCharacter, .05f);
    //}
    #endregion

    void Update()
    {
        ///metto possibilità di cambiare vite max in game menu
        //livesMax = MainMenu.InstanceMenu.LivesMax;
        //HCmodeButton.onClick.AddListener(HardcoreMode);

        
        ///DEBUG
        ///
        Debug.Log($"VELOCITY {Rigidbody.velocity.magnitude} || FUEL {remainingFuel} || Lives: {livesCount}");

        #region ||>> HIGH VELOCITY WARNINGS <<||

        if(Rigidbody.velocity.y>-7)
        {
            //spriteRenderer.DOColor(colorCharacter, .05f);
            if (isFalling)
            { isFalling = false; }
            spriteRenderer.color = colorCharacter;
        }

        if (Rigidbody.velocity.y <= -7.5f&&Rigidbody.velocity.y>-9)
        {
            //GetComponent<MeshRenderer>().material = myHitTakenMaterial;
            //Invoke("SetNormalMaterial", 0.25f);
            if (!isFalling)
            {
                currentWarnColor= Color.yellow;
                StartCoroutine(VelocityWarnCoroutine());
            }
        }
        if(Rigidbody.velocity.y<-9)
        {
            currentWarnColor = Color.red;
        }
        #endregion

        #region |||>>> FUEL CONTROL <<<|||

        remainingFuel = remainingFuel < 0 ? 0 : remainingFuel;

        fuelMeter.text = $"FUEL {(int)remainingFuel}";
        fuelSlider.value = remainingFuel / maxFuel;
        FuelCircleProgression.fillAmount=remainingFuel/maxFuel;

        if(Input.GetKey(KeyCode.F))
        { remainingFuel = maxFuel; Debug.Log("FUEL " + remainingFuel); }

        if(secretNumber==666)
        { remainingFuel = maxFuel; }

        if(remainingFuel<1)
        { Invoke("DestroyTrail", 0.2f); }
        #endregion

        #region |||>>> LIFE AND SCORE COUNT <<<|||

        if (livesCount != 0)
        {
            if (livesMax == 11)
            { livesText.text = "ETERNAL LIFE"; }
            else if (livesMax == 1)
            { livesText.text = "HARDCORE"; }
            else
            { livesText.text = $"LIVES {livesCount}"; }
        }
        else
        { livesText.text = "LIVES 0"; }

        scoreText.text = $"SCORE {platformCount}";
        #endregion

        //if (isOnGround)
        //{
        //    rb.drag = drag;
        //}

        #region |||>>> JOYSTICK MOVEMENT <<<|||

        if (joystickYaxisInverted)
        {
            if (joystick.Vertical < 0)
            { joystickYaxisCondition = true; }
            else
            { joystickYaxisCondition = false; }
        }
        else
        {
            if(joystick.Vertical > 0)
            { joystickYaxisCondition = true; }
            else
            { joystickYaxisCondition = false; }
        }

        if (joystickXaxisInverted)
        {
            if (joystick.Horizontal > 0)
            {
                joystickXaxisCondition = true;
                //directionLeftSprite = spriteArray[0];
            }
            else if (joystick.Horizontal < 0)
            {
                joystickXaxisCondition = false;
                //directionRightSprite = spriteArray[1];
            }
        }
        else
        {
            if (joystick.Horizontal < 0)
            {
                joystickXaxisCondition = true;
                //directionLeftSprite = spriteArray[1];
            }
            else if (joystick.Horizontal > 0)
            {
                joystickXaxisCondition = false;
                //directionRightSprite = spriteArray[0];
            }
        }

        if (joystickControl&&remainingFuel>0)
        {
            if (joystickYaxisCondition || joystick.Horizontal != 0)
            { Trail.SetActive(true);
                gameObject.transform.GetChild(2).gameObject.SetActive(true);
            }
            else
            {
               Invoke( "DestroyTrail", 0.3f);
               Invoke("DestroyParticleFX", .6f);
            }

            if (joystickYaxisCondition /*joystick.Vertical < 0*/)
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
            posizFacing = joystick.Horizontal;

            if (joystick.Horizontal != 0)
            {
                #region old xAxis
                //if (joystickXaxisInverted)
                //{
                //    directionLeftSprite = spriteArray[0];
                //    directionRightSprite = spriteArray[1];
                //    if (/*joystickXaxisCondition*/posizFacing > 0)
                //    {
                //        Rigidbody.AddForce(Vector2.left * fromRightJetForce * joystick.Horizontal * Time.deltaTime, ForceMode2D.Impulse);
                //        //*(joyXinv?1:-1)

                //        //Rigidbody.velocity = new Vector2(fromLeftJetForce * -joystick.Horizontal, Rigidbody.velocity.y);
                //        //polygonCollider.transform.eulerAngles = new Vector3(0, 180, 0);
                //        ChangeSprite(directionLeftSprite);
                //        //ChangeAnimationRight();
                //    }
                //    if (/*!joystickXaxisCondition*/posizFacing < 0)
                //    {
                //        Rigidbody.AddForce(Vector2.right * fromLeftJetForce * -joystick.Horizontal * Time.deltaTime, ForceMode2D.Impulse);
                //        //Rigidbody.velocity = new Vector2(fromRightJetForce * -joystick.Horizontal, Rigidbody.velocity.y);
                //        //polygonCollider.transform.eulerAngles = new Vector3(0, 0, 0);
                //        ChangeSprite(directionRightSprite);
                //        //ChangeAnimationLeft();
                //    }
                //}
                //else
                //{
                //    directionLeftSprite = spriteArray[0];
                //    directionRightSprite = spriteArray[1];
                //    if (posizFacing > 0)
                //    {
                //        Rigidbody.AddForce(Vector2.right * fromLeftJetForce * joystick.Horizontal * Time.deltaTime, ForceMode2D.Impulse);
                //        ChangeSprite(directionRightSprite);
                //    }
                //    if (posizFacing < 0)
                //    {
                //        Rigidbody.AddForce(Vector2.left * fromRightJetForce * -joystick.Horizontal * Time.deltaTime, ForceMode2D.Impulse);
                //        ChangeSprite(directionLeftSprite);
                //    }
                //}
                #endregion

                if (joystickXaxisCondition/*posizFacing > 0*/)
                {
                    Rigidbody.AddForce((joystickXaxisInverted ? Vector2.left : Vector2.right) * fromRightJetForce * joystick.Horizontal * Time.deltaTime, ForceMode2D.Impulse);
                    //Rigidbody.velocity = new Vector2(fromLeftJetForce * -joystick.Horizontal, Rigidbody.velocity.y);
                    //polygonCollider.transform.eulerAngles = new Vector3(0, 180, 0);
                    ChangeSprite(joystickXaxisInverted ? directionLeftSprite:directionRightSprite);
                    //ChangeAnimationRight();
                }
                if (!joystickXaxisCondition/*posizFacing < 0*/)
                {
                    Rigidbody.AddForce((joystickXaxisInverted ? Vector2.right : Vector2.left) * fromLeftJetForce * -joystick.Horizontal * Time.deltaTime, ForceMode2D.Impulse);
                    ChangeSprite(joystickXaxisInverted ? directionRightSprite : directionLeftSprite);//SPRITE DIREZ SBAGLIATA,METTILA NELLA CONDIZ AXIX X
                }
            }
        }
        else
        { }
        #endregion

        #region |||>>> DEATH BELOW LAST PLAT <<<|||

        float yMin;
        int i;


        //if (WallGeneration.Instance.ListaStageGenerati.Count>1 )
        //if()
        for(i=0; i<WallGeneration.Instance.ListaStageGenerati.Count; i++)
        {
            if (WallGeneration.Instance.ListaPlatforms[i].PlatformStatusIndex == 1)
            {
                yMin = WallGeneration.Instance.ListaPlatforms[i].PlatformGenerated.transform.position.y;
                if (transform.position.y < yMin - 5f&&isAlive)
                {
                    isAlive = false;
                    deathMessageNum = 0;
                    posCollision = transform.position;
                    SetPlayerInactiveLoseLife();
                    Rigidbody.velocity = new Vector2(0, 0);
                    yRespawn = yMin + .3f;
                    xRespawn = WallGeneration.Instance.ListaStageGenerati[i].name.Contains("RIGHT") ? 1.5f : -1.5f;
                    //SetPlayerRespawn();
                    Invoke(DeathMessageType(), deathTimeDuration/*/2*/);

                    //gameObject.transform.position = new Vector3(xRespawn, yRespawn, 0);
                    //yRespawn = WallGeneration.Instance.ListaStageGenerati[3].name.Contains("FIRST") ? 0f : WallGeneration.Instance.ListaStageGenerati[3].transform.position.y + 1f;
                    //xRespawn = WallGeneration.Instance.ListaStageGenerati[3].name.Contains("FIRST") ? 0f : WallGeneration.Instance.ListaStageGenerati[3].name.Contains("RIGHT") ? 1.5f : -1.5f;
                    //SetPlayerRespawn(xRespawn, yRespawn);
                }
                break;
            }
            else
            { }
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
