using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;
using Unity.Burst.CompilerServices;
using System;
using TMPro;
using DG.Tweening;
//using System.Xml.Schema;
//using JetBrains.Annotations;
//using System.Runtime.CompilerServices;
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

    #region |||>> PUBLIC OBJECTS <<|||

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
    public Animator PlayerAnimator;
    //public Slider maxLivesSlider;
    //public TrailRenderer trailRenderer;
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
    public static bool joystickControl=true;
    public bool jetpackInAirOnly;
    public bool leftRightInAirOnly;
    public bool joystickYaxisInverted/*= true*/;
    public bool joystickXaxisInverted/*=true*/;
    [Header("OTHER")]
    public float deathFallingSpeed = 9.0f;
    public int secretNumber;
    public int springForce = 500;

    #endregion

    #region |||>>> PROGRAM VARIABLES <<<|||

    private Sprite directionLeftSprite;
    private Sprite directionRightSprite;
    private Vector3 leftFacingVector;
    private Vector3 rightFacingVector;
    private Color currentWarnColor;
    private Color currentFuelWarnColor;
    private Color fullFuelColor;
    private float remainingFuel;
    private float yLastReachedPlatform = 0;
    private float xRespawn;
    private float yRespawn;
    //private float[] xyRespawn = new float[2];
    private Vector2 posCollision;
    private float deathTimeDuration = .5f;
    private float yMinReachable=0;
    private int livesCount;
    private int livesMax;
    private int platformCount=0;
    private enum Index
    {
        DEAD = 0,
        OUT_OF_FUEL = 1,
        GONE_TOO_LOW = 2
    }
    private Index deathMessage;
    public static PlayerController instance;

    #endregion

    #region |> CONTROL VARIABLES <|

    private bool inputPressed;
    private bool upPressedDown;
    private bool spacePressedDown;
    private bool spacePressed;
    private bool spacePressedUp;
    private bool leftPressed;
    private bool joystickYaxisCondition;
    private bool joystickXaxisCondition;
    private bool livesActive;
    private bool halfFuel;
    private bool isFalling;
    private bool isAlive =true;

    #endregion

    private void Awake()
    {
        //instance = this;
        joystickXaxisInverted = AxisOrientation.instance.XAxisInverted/*!=null? AxisOrientation.instance.XAxisInverted:true*/;
        joystickYaxisInverted = AxisOrientation.instance.YAxisInverted/* != null ? AxisOrientation.instance.YAxisInverted : true*/;
    }

    void Start()
    {
        //GETTING PLAYER COMPONENTS
        spriteRenderer = GetComponent<SpriteRenderer>();
        Rigidbody = GetComponent<Rigidbody2D>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        colorCharacter = spriteRenderer.color;
        fullFuelColor = FuelCircleProgression.color;
        //polygonCollider = GetComponent<PolygonCollider2D>();
        //trailRenderer = GetComponentInChildren<TrailRenderer>();
        //rectTransform=Handle.GetComponent<RectTransform>();


        //SETTING PLAYER DATA
        remainingFuel = maxFuel;
        livesMax = MainMenu.InstanceMenu.LivesMax;
        livesCount = livesMax;  

        leftFacingVector=new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z);
        rightFacingVector = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        directionLeftSprite = spriteArray[0];
        directionRightSprite= spriteArray[1];

        if (livesMax != 11)
        { livesActive = true; }
        else { livesActive = false; }

        xRespawn= gameObject.transform.position.x;
        yRespawn= gameObject.transform.position.y;
        
        //buttonRestart.onClick.AddListener(RestartGame);

    }

    #region |||>>> GAME OVER AND RESTART <<<|||

    //RESTART GAME CON SCENE NAME

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

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    #endregion

    #region |||>>> COLLISIONS <<<|||

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //if(capsuleCollider.CompareTag("walls"))

        //float yStageCollision = collision.gameObject.transform.position.y;
        float yCollidedPlat = collision.gameObject.transform.parent.transform.position.y;
        PlatformBehaviour collidedPlat = collision.gameObject.GetComponentInParent<PlatformBehaviour>();
        posCollision = transform.position;
        Rigidbody.velocity = new Vector2(0, 0);

        //jumper collision piazzato sul prefab col fungo 
        if (collision.gameObject.layer==9)
        {
            Rigidbody.AddForce(Vector2.up*springForce*Time.deltaTime, ForceMode2D.Impulse);
        }

        //wall collision
        if (collision.gameObject.layer == 3)

        //prende posizione player alla collisione, occhio che conta anche allo start quando è fermo al ground
        {
            isAlive = false;
            deathMessage = Index.DEAD;
            Invoke(DeathLogic(), deathTimeDuration);
        }

        //platform collison
        if (collision.gameObject.layer ==8)
        {
            //collision da sotto
            if (transform.position.y < yCollidedPlat)
            {
                isAlive = false;
                deathMessage = Index.DEAD;
                Invoke(DeathLogic(), deathTimeDuration);
            }

            //atterraggio da sopra
            else
            {
                //morte too fast
                if (Mathf.Abs(collision.relativeVelocity.y) > deathFallingSpeed)
                {
                    isAlive = false;
                    deathMessage = Index.DEAD;
                    Invoke(DeathLogic(), deathTimeDuration);
                }
                //respawn se finsici benza su stessa piatt
                else if(yCollidedPlat == yLastReachedPlatform&&remainingFuel < 1)
                {
                    isAlive = false;
                    deathMessage = Index.OUT_OF_FUEL;
                    Invoke(DeathLogic(), deathTimeDuration);
                }
                //rileva se nuova piattaforma e aumenta score
                else if(collidedPlat.index==PlatformBehaviour.Index.MAI_TOCCATA)
                {
                    NewPlatformReached(collidedPlat);
                    yLastReachedPlatform = yCollidedPlat;
                    Debug.Log(collision.gameObject.transform.parent.gameObject.transform.parent.gameObject.name);
                }

                //else if (yCollidedPlat != yLastReachedPlatform)
                //{
                //    yLastReachedPlatform = yCollidedPlat;
                //}
            }
        }
        //collision ground level
        if (collision.gameObject.layer == 6)
        {
            if (Mathf.Abs(collision.relativeVelocity.y) > deathFallingSpeed)
            {
                isAlive = false;
                deathMessage = Index.DEAD;
                Invoke(DeathLogic(), deathTimeDuration);

            }
            //fine benza
            else if (remainingFuel < 1)
            {
                isAlive = false;
                deathMessage = Index.OUT_OF_FUEL;
                Invoke(DeathLogic(), deathTimeDuration);
            }
        }
    }
    #endregion

    #region |||>>> DEATH LOGIC <<<|||

    private string DeathLogic()
    {
        if (livesActive == false)
        {
            platformCount = 0;
        }
        SetPlayerInactiveLoseLife();
        string condition = DeathMessageType();
        return condition;
    }

    private void SetPlayerInactiveLoseLife()
    {
        //StartCoroutine(SetPlayerInactiveCorutine());

        //FX MORTE

        gameObject.SetActive(false);
        //if(SoundManager.AudioON)
        SoundManager.PlaySound("playerDeath");
        GameObject explosion = Instantiate(ExplosionTemplate, posCollision, Quaternion.identity);
        //explosion.Emit(60);
        Destroy(explosion, .4f);

        if (livesActive == true)
        {
            livesCount--;
            //controllo particella da codice
        }

    }

    private string DeathMessageType()
    {
        string condition;
        if (livesCount == 0)
        { condition = "GameOver"; }
        else
        {
            if (deathMessage == Index.DEAD)
            {
                deadText.gameObject.SetActive(true);
                //condition = "SetPlayerActive";
            }
            else if (deathMessage==Index.OUT_OF_FUEL)
            {
                outOfFuelText.gameObject.SetActive(true);
            }
                condition = "SetPlayerActive";
        }
        return condition;
    }
    #endregion

    #region |||>>> RESPAWN LOGIC <<<|||

    private void PlayerRespawnCalculator(PlatformBehaviour _collidedPlat)
    {
        xRespawn = _collidedPlat.side == PlatformBehaviour.Side.RIGHT ? 1.5f : -1.5f;
        yRespawn = _collidedPlat.transform.position.y + .3f;
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

    private void SetPlayerRespawn(/*float xRespawn, float yRespawn*/)
    {
        isAlive = true;
        gameObject.transform.position = new Vector3(xRespawn, yRespawn, 0);
        remainingFuel = maxFuel;
        //faccio aspettare 1 secondo prima di riprendere i controlli movement
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

    #region |||>>> NEW PLATFORM UPDATES <<<|||

    private void NewPlatformReached(PlatformBehaviour collidedPlat)
    {
        //PLAYER VARIABLES UPDATE: Fuel refill, platform score +1, minima Y raggiungibile
        remainingFuel = maxFuel;
        platformCount++;
        yMinReachable = collidedPlat.transform.position.y;
        //Calculate new respawn position
        PlayerRespawnCalculator(collidedPlat);
        //Platform status and color 
        SetNewPlatformStatus(collidedPlat);
        //setta status PASSATA a piattaforme vecchie
        SetPassedPlatformStatus();
    }

    private void SetNewPlatformStatus(PlatformBehaviour collidedPlat)
    {
        collidedPlat.index=PlatformBehaviour.Index.ULTIMA;
        collidedPlat.mainPart_SpriteRenderer.color = new Color(200f, 0f, 0f, 255f);
        collidedPlat.edgePart_SpriteRenderer.color = new Color(200f, 0f, 0f, 255f);
    }

    private void SetPassedPlatformStatus()
    {
        int iterMin = WallGeneration.Instance.ListaPlatforms.Count > 5 ? WallGeneration.Instance.ListaPlatforms.Count - 5 : 0;
        for (int i = WallGeneration.Instance.ListaPlatforms.Count - 1; i >= iterMin; i--)
        {
            if (WallGeneration.Instance.ListaPlatforms[i].index == PlatformBehaviour.Index.ULTIMA)
            {
                for (int j = iterMin; j < i; j++)
                {
                    WallGeneration.Instance.ListaPlatforms[j].index = PlatformBehaviour.Index.PASSATA;
                }
                break;
            }
        }
    }
    #endregion

    #region <<<OLD COLLISION-RESPAWN CALCULATOR>>>
    private void OLD_ReachedPlatformCalculator_OLD()
    {
        int iterMax = WallGeneration.Instance.ListaStageGenerati.Count > 4 ? 4 : WallGeneration.Instance.ListaStageGenerati.Count;
        for (int i = iterMax - 1; i >= 0; i--)

        //int iterMin = WallGeneration.Instance.ListaStageGenerati.Count > 4 ? WallGeneration.Instance.ListaStageGenerati.Count - 4 : 0;
        //for(int i = WallGeneration.Instance.ListaStageGenerati.Count-1; i>iterMin;i--)
        {
            if (WallGeneration.Instance.ListaPlatforms[i].index == PlatformBehaviour.Index.MAI_TOCCATA)
            {
                platformCount++;
                remainingFuel = maxFuel;
                WallGeneration.Instance.ListaPlatforms[i].index = PlatformBehaviour.Index.ULTIMA;
                yMinReachable = WallGeneration.Instance.ListaPlatforms[i].transform.position.y;
                WallGeneration.Instance.ListaPlatforms[i].mainPart_SpriteRenderer.color = new Color(200f, 0f, 0f, 255f);
                WallGeneration.Instance.ListaPlatforms[i].edgePart_SpriteRenderer.color = new Color(200f, 0f, 0f, 255f);
                xRespawn = (WallGeneration.Instance.ListaPlatforms[i].side == PlatformBehaviour.Side.RIGHT) ? 1.5f : -1.5f;
                yRespawn = WallGeneration.Instance.ListaPlatforms[i].transform.position.y + .3f;

                //mette status "2" = "passed" alle piattaforme vecchie DOPO L'ULTIMA RAGGIUNTA
                //for (int z=i+1; z<iterMax; z++)
                if (i < iterMax - 1)
                {
                    for (int j = i + 1; j < iterMax - 1; j++)
                    {
                        WallGeneration.Instance.ListaPlatforms[j].index = PlatformBehaviour.Index.PASSATA;
                    }
                }
                break;
            }
        }
    }
    private void OLD_CollisionRespawnCalculator_OLD()
    {
        //int iterMax = WallGeneration.Instance.ListaStageGenerati.Count > 4 ? 4 : WallGeneration.Instance.ListaStageGenerati.Count;
        int iterMin = WallGeneration.Instance.ListaStageGenerati.Count > 5 ? WallGeneration.Instance.ListaStageGenerati.Count - 5 : 0;
        for (int i = WallGeneration.Instance.ListaStageGenerati.Count - 1; i >= iterMin; i--)
        {
            if (WallGeneration.Instance.ListaPlatforms[i].index == PlatformBehaviour.Index.ULTIMA)
            {
                yRespawn = WallGeneration.Instance.ListaPlatforms[i].transform.position.y + .3f;
                xRespawn = (WallGeneration.Instance.ListaPlatforms[i].side == PlatformBehaviour.Side.RIGHT) ? 1.5f : -1.5f;
                break;
            }
            else
            {
                yRespawn = .3f;
                xRespawn = 0f;
            }
        }
    }
    #endregion

    #region |||>>> TRAIL DESTROY <<<|||

    private void DestroyTrail()
    {
        Trail.SetActive(false);
    }
    private void DestroyParticleFX()
    {
        gameObject.transform.GetChild(2).gameObject.SetActive(false);
    }
    #endregion

    #region |||>>> CAMBIA SPRITE DIREZIONE MOVIMENTO <<<|||

    private void ChangeSprite(Sprite _newSprite)
    {
        spriteRenderer.sprite = _newSprite;
    }

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

    #endregion

    #region ||>> VELOCITY/FUEL WARNING COROUTINES <<||

    //CTRL+RR cambia nome in tutto documento

    IEnumerator VelocityWarnCoroutine()
    {
        //spriteRenderer.transform.DOShakePosition(.25f);

        isFalling = true;
        while (isFalling)
        {
            
            spriteRenderer.DOColor(currentWarnColor, .05f);
            yield return new WaitForSeconds(.1f);
            spriteRenderer.DOColor(colorCharacter, .05f);
            yield return new WaitForSeconds(.1f);
        }
    }

    IEnumerator FuelWarnCoroutine()
    {
        //spriteRenderer.transform.DOShakePosition(.25f);

        halfFuel = true;
        while (halfFuel)
        {

            FuelCircleProgression.DOColor(currentFuelWarnColor, .05f);
            yield return new WaitForSeconds(.1f);
            FuelCircleProgression.DOColor(fullFuelColor, .05f);
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

    //void OnBecameInvisible()
    //{
    //    Destroy(gameObject);
    //}

    void Update()
    {
        //DEBUG
        Debug.Log($"VELOCITY {Rigidbody.velocity.magnitude} || FUEL {remainingFuel} || Lives: {livesCount}");
        //

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

        //indicatore fuel vecchio
        fuelMeter.text = $"FUEL {(int)remainingFuel}";
        fuelSlider.value = remainingFuel / maxFuel;
        //setta livello fuel del serbatoio nuovo
        FuelCircleProgression.fillAmount=remainingFuel/maxFuel;

        if(remainingFuel<1)
        { Invoke("DestroyTrail", 0.2f); }

        //FUEL WARNS
        if (remainingFuel > maxFuel/2)
        {
            if (halfFuel)
            { halfFuel = false; }
            FuelCircleProgression.color = fullFuelColor;
        }

        if (remainingFuel<=maxFuel/2&&remainingFuel>maxFuel/4)
        {
            if (!halfFuel)
            {
                currentFuelWarnColor = Color.yellow;
                StartCoroutine(FuelWarnCoroutine());
            }
        }
        if (remainingFuel < maxFuel/4)
        {
            currentFuelWarnColor = Color.red;
        }

        //FUEL CHEATS
        if (Input.GetKey(KeyCode.F))
        { remainingFuel = 100000000; Debug.Log("FUEL " + remainingFuel); }

        if(secretNumber==666)
        { remainingFuel = maxFuel; }
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

        #region |||>>> JOYSYICK AXIS ORIENTATION <<<|||

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
            { joystickXaxisCondition = true; }
            else if (joystick.Horizontal < 0)
            { joystickXaxisCondition = false; }
        }
        else
        {
            if (joystick.Horizontal > 0)
            { joystickXaxisCondition = false; }
            else if (joystick.Horizontal < 0)
            { joystickXaxisCondition = true; }
        }
        #endregion

        #region |||>>> JOYSTICK MOVEMENT <<<|||

        if (joystickControl&&remainingFuel>0)
        {
            //VERTICAL MOVEMENT
            if (joystickYaxisCondition || joystick.Horizontal != 0)
            { 
                Trail.SetActive(true);
                gameObject.transform.GetChild(2).gameObject.SetActive(true);
                
            }
            else
            {
               Invoke( "DestroyTrail", 0.3f);
               Invoke("DestroyParticleFX", 0.6f);
            }

            if (joystickYaxisCondition /*joystick.Vertical < 0*/)
            {
                Rigidbody.AddForce(Vector2.up * jetpackForce*Time.deltaTime, ForceMode2D.Impulse); 
                remainingFuel -= fuelPerSecond*Time.deltaTime;
                //Alternativa Velocity:
                //{ Rigidbody.velocity = new Vector2(Rigidbody.velocity.x, jetpackForce * Mathf.Abs(joystick.Vertical)); }

                //if (Rigidbody.velocity.y == .3f)
                //{
                //    PlayerAnimator = GetComponent<Animator>();
                //    string animationName = "playerAnimation_JUMP";
                //    PlayerAnimator.Play(animationName);
                //}

            }
            else 
            {  }

            //Check Facing
            

            //HORIZONTAL MOVEMENT
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
                    transform.localScale= leftFacingVector;
                    Rigidbody.AddForce((joystickXaxisInverted ? Vector2.left : Vector2.right) * fromRightJetForce * joystick.Horizontal * Time.deltaTime, ForceMode2D.Impulse);
                    ChangeSprite(directionLeftSprite);

                    //Rigidbody.velocity = new Vector2(fromLeftJetForce * -joystick.Horizontal, Rigidbody.velocity.y);
                    //polygonCollider.transform.eulerAngles = new Vector3(0, 180, 0);
                    //ChangeAnimationRight();
                }
                if (!joystickXaxisCondition/*posizFacing < 0*/)
                {
                    transform.localScale= rightFacingVector;
                    Rigidbody.AddForce((joystickXaxisInverted ? Vector2.right : Vector2.left) * fromLeftJetForce * -joystick.Horizontal * Time.deltaTime, ForceMode2D.Impulse);
                    ChangeSprite(directionRightSprite);
                }
            }
        }
        else
        { }
        #endregion

        #region |||>>> DEATH BELOW LAST PLAT <<<|||

        if (transform.position.y < yMinReachable - 5f && isAlive /*WallGeneration.Instance.ListaPlatforms[i].index == PlatformBehaviour.Index.ULTIMA*/)
        {
            //yMin = WallGeneration.Instance.ListaPlatforms[i].transform.position.y;
                posCollision = transform.position;
                Rigidbody.velocity = new Vector2(0, 0);
                isAlive = false;
                deathMessage = Index.DEAD;
                Invoke(DeathLogic(), deathTimeDuration/*/2*/);

        }
        else
        { }
        #endregion

        //if (transform.position.y>yLastReachedPlatform+5&& WallGeneration.Instance.ListaStageGenerati.Count > 6)
        //{
        //    for(int i=0; i>=WallGeneration.Instance.ListaStageGenerati.Count-7; i++)
        //    {
        //        StageSection twall = WallGeneration.Instance.ListaStageGenerati[i];
        //        Destroy(twall);
        //    }
        //    WallGeneration.Instance.ListaStageGenerati.RemoveRange(0, WallGeneration.Instance.ListaStageGenerati.Count-6);

        //    //foreach(StageSection stage in WallGeneration.Instance.ListaStageGenerati)
        //}

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

    #region >> syntax info: getting child/parents <<

    ///collision.gameObject.transform.parent.gameObject
    ///Prende parent come GameObject: oggettofiglio.transform.parent.gameObject
    ///se invece: oggettofiglio.transform.parent --> è preso come Transform;
    ///
    ///Prende child come GameObject: oggettoparent.transform.getchild(n).gameObject
    ///se invece: oggettoparent.transform.getchild(n) --> è preso come Transform.
    #endregion
}
