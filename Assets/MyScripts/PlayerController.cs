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
    #region |||>>> VARIABLES <<<|||

    #region ||>> PLAYER COMPONENTS <<||

    Rigidbody2D myRigidbody;
    CapsuleCollider2D capsuleCollider;
    SpriteRenderer spriteRenderer;
    Color colorCharacter;
    #endregion

    #region ||> INTERFACE VARIABLES <<||

    [Header("FORCE LEVELS")] //BALANCE per piattaf distanti 11: jetForce=25, fromLeftRight=13
    [Tooltip("BALANCE: jetForce = 25, from Left/Right = 13")]
    public float jumpForce=6;
    public float jetpackForce=25;
    public float fromLeftJetForce=13;
    public float fromRightJetForce=13;
    [Header("FUEL LEVELS")] //BALANCE per piattaf distanti 11: fuel=100, fuelPerSec=49
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
    [Tooltip("The secret number is,\nthe number of the beast")]
    public float deathFallingSpeed = 9.0f;
    public int mushSpringForce = 500;
    public static int secretNumber=0;
    public static bool mushroomJumper = false;

    #endregion

    #region ||> PUBLIC OBJECTS <<||

    [Header("GAME OBJECTS")]
    public GameObject Player;
    public GameObject Trail;
    public Camera mainCamera;
    public FixedJoystick joystick;
    public FloatingJoystick joyFloat;
    public Sprite[] spriteArray;
    //public Slider fuelSlider;
    public Button buttonRestart;
    //public TMP_Text fuelMeter;
    public TMP_Text scoreText;
    public TMP_Text GameOverText;
    public TMP_Text livesText;
    public TMP_Text deadText;
    public TMP_Text outOfFuelText;
    public TMP_Text hiScoreText;
    public GameObject ExplosionTemplate;
    public Image FuelCircleProgression;
    public Animator PlayerAnimator;
    //public Slider maxLivesSlider;
    //public TrailRenderer trailRenderer;
    //public GameObject playerIdleLeft;
    //public GameObject playerIdleRight;

    #endregion

    #region ||>> PLAYER VARIABLES <<||

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
    private Vector2 playerPosOnCollision;
    private float deathTimeDuration = .5f;
    private float yMinReachable=0;
    private int livesCount;
    private int livesMax;
    private int maxScoreToSave;
    private int oldMaxScore;
    private int platformCount=0;
    //private int state = 0;
    private enum Index
    {
        DEAD = 0,
        OUT_OF_FUEL = 1,
        GONE_TOO_LOW = 2
    }
    private Index deathMessage;

    #endregion

    #region ||>> CONTROL VARIABLES <<||

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
    private bool isGrounded=true;
    private bool isAlive =true;
    private bool isJumping = false;
    private bool isLanding = false;
    private bool HC = false;
    public float minJumpDelay;
    private float minJumpDelayCounter;
    public float minLandDelay;
    private float minLandDelayCounter;
    private bool isNearGround=false;


#endregion

    #endregion

    #region ||>> INIT <<||

private void Awake()
    {
        //SET AXIS ORIENTATION VARIABLE
        if (AxisOrientation.instance != null)
        {
            joystickXaxisInverted = AxisOrientation.instance.XAxisInverted;
            joystickYaxisInverted = AxisOrientation.instance.YAxisInverted;
        }
        else
        {
            joystickXaxisInverted=true;
            joystickYaxisInverted=true;
        }
        
        LoadHighScore();
    }

    void Start()
    {
        //GETTING GAME COMPONENTS
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        myRigidbody = GetComponent<Rigidbody2D>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        colorCharacter = spriteRenderer.color;
        fullFuelColor = FuelCircleProgression.color;

        //SETTING PLAYER DATA
        remainingFuel = maxFuel;
        //if (MainMenu.InstanceMenu != null)
        livesMax = MainMenu.InstanceMenu.LivesMax!=null ? MainMenu.InstanceMenu.LivesMax:11;
        livesCount = livesMax;
        
        //SPRITE DIRECTION
        leftFacingVector = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z);
        rightFacingVector = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        directionLeftSprite = spriteArray[0];
        directionRightSprite= spriteArray[1];

        //BASE RESPAWN
        xRespawn= gameObject.transform.position.x;
        yRespawn= gameObject.transform.position.y;

        //LIVES CONDITION
        if(livesMax==11)
        { 
            livesActive = false;
            livesText.text = "ETERNAL LIFE";
        }
        else if (livesMax != 11)
        { 
            livesActive = true; 
            if (livesMax == 1)
            { 
                HC = true;
                livesText.text = "HARDCORE"; 
            }
        }

        ////MUSHROOM JUMPER ò_ò
        //if (mushroomJumper)
        //{
        //    ActivateJumper();
        //}

    }
    #endregion

    #region ||>> GAME OVER MESSAGES AND RESTART <<||

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

    #region ||>> COLLISIONS <<||

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //if(capsuleCollider.CompareTag("walls"))
        //float yStageCollision = collision.gameObject.transform.position.y;

        PlatformBehaviour collidedPlat = collision.gameObject.GetComponentInParent<PlatformBehaviour>();
        float yCollidedPlat = collision.gameObject.transform.parent.transform.position.y;
        playerPosOnCollision = transform.position;
        myRigidbody.velocity = new Vector2(0, 0);

        //Collision su Jumper piazzato sul prefab col fungo
        //(TO ACTIVATE: attiva bool mushroomJumper che attiva polygon collider del fungo e collider del jumper sul prefab)
        if (collision.gameObject.layer == 9)
        {
            //fine benza
            if (remainingFuel < 1)
            {
                isAlive = false;
                deathMessage = Index.OUT_OF_FUEL;
                Invoke(DeathLogic(), deathTimeDuration);
            }
            else
            { myRigidbody.AddForce(Vector2.up * mushSpringForce * Time.deltaTime, ForceMode2D.Impulse); }
        }

        //wall collision
        if (collision.gameObject.layer == 3)
        {
            
            if (MainMenu.easyMode==false || Mathf.Abs(collision.relativeVelocity.y) > 7.5f)
            //prende posizione player alla collisione, occhio che conta anche allo start quando è fermo al ground
            {
                isAlive = false;
                deathMessage = Index.DEAD;
                Invoke(DeathLogic(), deathTimeDuration);
            }
            else
            { }
            //if (MainMenu.InstanceMenu.easyMode && Mathf.Abs(collision.relativeVelocity.y) > 7.5f)

        }
        
        //PLATFORM COLLISION
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

        #region TEST LOWER LIMIT CONDITION
        //TEST LOWER LIMIT CON COLLISION
        if (collision.gameObject.layer == 10)
        {
            playerPosOnCollision = transform.position;
            myRigidbody.velocity = new Vector2(0, 0);
            isAlive = false;
            deathMessage = Index.DEAD;
            Invoke(DeathLogic(), deathTimeDuration/*/2*/);
        }
        #endregion

    }
    #endregion

    #region ||>> DEATH LOGIC <<||

    private string DeathLogic()
    {
        //disattiva player, toglie vita ed effetti di morte
        SetPlayerInactiveLoseLife();

        //valorizza condition con messaggio death e chiama respawn in base al tipo di morte
        string condition = DeathMessageType();
        return condition;
    }

    private void SetPlayerInactiveLoseLife()
    {
        //StartCoroutine(SetPlayerInactiveCorutine());
        //isAlive = false;

        //Disattiva player
        gameObject.SetActive(false);

        //Toglie una vita
        if (livesActive == true)
        {
            livesCount--;
        }

        //azzera score se Eternal life attivo
        if (livesActive == false)
        {
            platformCount = 0;
        }

        //Sound e graphic FX MORTE
        if (SFXsoundManager.instance?.sfxSourceJetpack.isPlaying==true)
        Invoke("StopSFXplaying", 0.05f);

        if(SoundManager.instance)
        SFXsoundManager.instance.PlayDeathSound();

        GameObject explosion = Instantiate(ExplosionTemplate, playerPosOnCollision, Quaternion.identity);
        //explosion.Emit(60);
        Destroy(explosion, .4f);

    }

    private string DeathMessageType()
    {
        //messaggio morte e chiama respawn in base al tipo di morte

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

    #region ||>> RESPAWN LOGIC <<||

    //Calcola posizione piattaforma respawn
    private void PlayerRespawnCalculator(PlatformBehaviour _collidedPlat)
    {
        xRespawn = _collidedPlat.side == PlatformBehaviour.Side.RIGHT ? 1.5f : -1.5f;
        yRespawn = _collidedPlat.transform.position.y + .3f;
    }

    //disattiva messagi di morte, setta respawn e riattiva player
    private void SetPlayerActive()
    {
        if(deadText.IsActive())
        deadText.gameObject.SetActive(false);

        if(outOfFuelText.IsActive())
        outOfFuelText.gameObject.SetActive(false);

        //Setta respawn e attiva player
        SetPlayerRespawn();
        gameObject.SetActive(true);
    }

    //Imposta le coordinate di respawn 
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

    #region ||>> NEW PLATFORM UPDATES <<||

    private void NewPlatformReached(PlatformBehaviour collidedPlat)
    {
        //PLAYER VARIABLES UPDATE: Fuel refill, platform score +1, minima Y raggiungibile
        //Fuel refill
        FuelCircleProgression.DOFillAmount(maxFuel, (1 - remainingFuel / maxFuel) * 1f);
        remainingFuel = maxFuel;
        //aumenta score
        platformCount++;
        //imposta minima y raggiungibile
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
        //StageSection collidedStage= collidedPlat.gameObject.GetComponentInParent<StageSection>();
        //collidedStage.lowerCollider.enabled = true;

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

    #region ||>> KILL JETPACK EFFECTS (trail, particle, sound) <<||

    private void KillJetpackEffects()
    {
        Invoke("StopJetpackSFXplaying", .05f);
        Invoke("DestroyTrail", 0.2f);
        Invoke("DestroyParticleFX", 0.5f);
    }

    private void DestroyTrail()
    {
        Trail.SetActive(false);
    }

    private void DestroyParticleFX()
    {
        gameObject.transform.GetChild(3).gameObject.SetActive(false);
    }

    private void StopJetpackSFXplaying()
    {
        SFXsoundManager.instance?.sfxSourceJetpack.Stop();
    }
    #endregion

    #region ||>> CAMBIA SPRITE DIREZIONE MOVIMENTO <<||

    private void ChangeSprite(Sprite _newSprite)
    {
        spriteRenderer.sprite = _newSprite;
    }

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

    #region |> ACTIVATE-DEACTIVATE MUSHROOM JUMPER <|

    private void ActivateJumper()
    {
        WallGeneration.Instance.stageWalls[4].transform.GetChild(3).gameObject.GetComponent<BoxCollider2D>().enabled = true;
        WallGeneration.Instance.stageWalls[4].transform.GetChild(4).gameObject.GetComponent<PolygonCollider2D>().enabled = true;
    }
    private void DeactivateJumper()
    {
        WallGeneration.Instance.stageWalls[4].transform.GetChild(3).gameObject.GetComponent<BoxCollider2D>().enabled = false;
        WallGeneration.Instance.stageWalls[4].transform.GetChild(4).gameObject.GetComponent<PolygonCollider2D>().enabled = false;
    }
    #endregion

    #region ||>> SAVE HIGH SCORE <<||

    private void SaveHighScore()
    {
        maxScoreToSave = platformCount;
        //oldMaxScore = maxScoreToSave;
        PlayerPrefs.SetInt("maxScore", maxScoreToSave);
        PlayerPrefs.Save();
    }
    private void LoadHighScore()
    {
        if (PlayerPrefs.HasKey("maxScore"))
        {
            //VALORE DA MOSTRARE A TXT COME HI SCORE
            oldMaxScore = PlayerPrefs.GetInt("maxScore");
            hiScoreText.text = $"HI-SCORE:{oldMaxScore.ToString()}";
        }
    }
    #endregion

    #region ||||>>>> UPDATE FUNCTIONS: JOYSTICK MOVEMENT, LIFE, FUEL CONTROL <<<<||||

    void Update()
    {
        //DEBUG
        Debug.Log($"VELOCITY {myRigidbody.velocity.magnitude} || FUEL {remainingFuel} || Lives: {livesCount}");
        Debug.LogWarning($"HISCORE {oldMaxScore}");

        #region <JUMP DELAY COUNTER>
        if (minJumpDelayCounter < 0)
            minJumpDelayCounter = 0;
        else
        {
            minJumpDelayCounter-=Time.deltaTime;
        }
        #endregion

        #region <LANDING TEST ANIMATION>
        //if (minLandDelayCounter < 0)
        //    minLandDelayCounter = 0;
        //else
        //{
        //    minLandDelayCounter -= Time.deltaTime;
        //}

        //if (isNearGround && minLandDelayCounter <= 0)
        //{
        //    if (!isGrounded)
        //    {
        //        PlayerAnimator.SetTrigger("Landing");
        //        minLandDelayCounter = minLandDelay;
        //        isNearGround = false;
        //    }
        //}
        #endregion

        #region ||>> JOYSYICK AXIS ORIENTATION <<||

        //Y AXIS
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

        //X AXIS
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

        if (joystickControl)
        {
            if (remainingFuel > 0 && isAlive)
            {
                //MOVEMENT EFFECTS
                if (joystickYaxisCondition || joystick.Horizontal != 0)
                {
                    Trail.SetActive(true);
                    //activate particleFX: cambia uso di Getchild
                    gameObject.transform.GetChild(3).gameObject.SetActive(true);
                }
                else
                {
                    KillJetpackEffects();
                }

                #region ||>> VERTICAL MOVEMENT <<||

                if (joystickYaxisCondition /*joystick.Vertical < 0*/)
                {
                    //playerState=inAir;

                    //Play Jetpack sound
                    if (SFXsoundManager.instance)
                        if (SFXsoundManager.instance.sfxSourceJetpack.isPlaying == false)
                            SFXsoundManager.instance.PlayJetpackPropulsion();

                    //PLAY JUMP ANIMATION
                    if (isGrounded)
                    {
                        if (!isJumping&&minJumpDelayCounter<=0)
                        {
                            PlayerAnimator.SetTrigger("Jump");
                            //Debug.LogError("triggerjumpanimation");
                            isJumping = true;
                            minJumpDelayCounter = minJumpDelay;
                        }
                        //isGrounded = false;
                    }

                    //move body
                    myRigidbody.AddForce(Vector2.up * jetpackForce * Time.deltaTime, ForceMode2D.Impulse);
                    remainingFuel -= fuelPerSecond/2 * Time.deltaTime;

                    //Alternativa Velocity:
                    //{ myRigidbody.velocity = new Vector2(myRigidbody.velocity.x, jetpackForce * Mathf.Abs(joystick.Vertical)); }

                    //if (myRigidbody.velocity.y <.3f && nearGround)
                    //{
                    //    PlayerAnimator = GetComponent<Animator>();
                    //    string animationName = "playerAnimation_JUMP";
                    //    PlayerAnimator.Play(animationName);
                    //}
                }
                else
                { }
                #endregion

                #region ||>> HORIZONTAL MOVEMENT <<||

                if (joystick.Horizontal != 0)
                {
                    remainingFuel -= fuelPerSecond/2 * Time.deltaTime;

                    if (joystickXaxisCondition/*posizFacing > 0*/)
                    {
                        transform.localScale = leftFacingVector;
                        myRigidbody.AddForce((joystickXaxisInverted ? Vector2.left : Vector2.right) * fromRightJetForce * joystick.Horizontal * Time.deltaTime, ForceMode2D.Impulse);
                        ChangeSprite(directionLeftSprite);

                        //myRigidbody.velocity = new Vector2(fromLeftJetForce * -joystick.Horizontal, myRigidbody.velocity.y);
                        //ChangeAnimationRight();
                    }
                    if (!joystickXaxisCondition/*posizFacing < 0*/)
                    {
                        transform.localScale = rightFacingVector;
                        myRigidbody.AddForce((joystickXaxisInverted ? Vector2.right : Vector2.left) * fromLeftJetForce * -joystick.Horizontal * Time.deltaTime, ForceMode2D.Impulse);
                        ChangeSprite(directionRightSprite);
                    }
                }
                #endregion

            }
            else
            {
                KillJetpackEffects();
            }
        }
        else
        {  }

        //if (isOnGround)
        //{
        //    myRigidbody.drag = drag;
        //}
        #region old xAxis
        //if (joystickXaxisInverted)
        //{
        //    directionLeftSprite = spriteArray[0];
        //    directionRightSprite = spriteArray[1];
        //    if (/*joystickXaxisCondition*/posizFacing > 0)
        //    {
        //        myRigidbody.AddForce(Vector2.left * fromRightJetForce * joystick.Horizontal * Time.deltaTime, ForceMode2D.Impulse);
        //        //*(joyXinv?1:-1)

        //        //myRigidbody.velocity = new Vector2(fromLeftJetForce * -joystick.Horizontal, myRigidbody.velocity.y);
        //        //polygonCollider.transform.eulerAngles = new Vector3(0, 180, 0);
        //        ChangeSprite(directionLeftSprite);
        //        //ChangeAnimationRight();
        //    }
        //    if (/*!joystickXaxisCondition*/posizFacing < 0)
        //    {
        //        myRigidbody.AddForce(Vector2.right * fromLeftJetForce * -joystick.Horizontal * Time.deltaTime, ForceMode2D.Impulse);
        //        //myRigidbody.velocity = new Vector2(fromRightJetForce * -joystick.Horizontal, myRigidbody.velocity.y);
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
        //        myRigidbody.AddForce(Vector2.right * fromLeftJetForce * joystick.Horizontal * Time.deltaTime, ForceMode2D.Impulse);
        //        ChangeSprite(directionRightSprite);
        //    }
        //    if (posizFacing < 0)
        //    {
        //        myRigidbody.AddForce(Vector2.left * fromRightJetForce * -joystick.Horizontal * Time.deltaTime, ForceMode2D.Impulse);
        //        ChangeSprite(directionLeftSprite);
        //    }
        //}
        #endregion

        #endregion

        #region ||>> HIGH VELOCITY WARNINGS <<||

        if(myRigidbody.velocity.y>-7)
        {
            //spriteRenderer.DOColor(colorCharacter, .05f);
            if (isFalling)
            { isFalling = false; }
            spriteRenderer.color = colorCharacter;
        }

        if (myRigidbody.velocity.y <= -7.5f&&myRigidbody.velocity.y>-9)
        {
            //GetComponent<MeshRenderer>().material = myHitTakenMaterial;
            //Invoke("SetNormalMaterial", 0.25f);
            if (!isFalling)
            {
                currentWarnColor= Color.yellow;
                StartCoroutine(VelocityWarnCoroutine());
            }
        }
        if(myRigidbody.velocity.y<-9)
        {
            currentWarnColor = Color.red;
        }
        #endregion

        #region ||>> FUEL CONTROL <<||

        remainingFuel = remainingFuel < 0 ? 0 : remainingFuel;

        //DEATH IF FUEL EMPTY AND PLAYER NOT MOVING
        if (remainingFuel<1&& myRigidbody.velocity.magnitude==0)
        {
            isAlive = false;
            deathMessage = Index.OUT_OF_FUEL;
            Invoke(DeathLogic(), deathTimeDuration);
        }

        //Setta livello fuel del serbatoio nuovo
        FuelCircleProgression.DOFillAmount(remainingFuel / maxFuel,.05f /*(1 - remainingFuel / maxFuel)*/) ;
        //FuelCircleProgression.fillAmount=remainingFuel/maxFuel;

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

        //indicatore fuel vecchio
        //fuelMeter.text = $"FUEL {(int)remainingFuel}";
        //fuelSlider.value = remainingFuel / maxFuel;
        #endregion

        #region ||>> LIFE AND SCORE COUNT <<||

        ///FIX TEXT VITE INGAME..ORA FUNZIA(17/03)
        if (livesCount != 0)
        {
            if(livesActive ==true&& HC == false)
             livesText.text = $"LIVES {livesCount}"; 
        }
        //else
        //{ livesText.text = "LIVES 0"; }

        scoreText.text = $"SCORE {platformCount}";

        if (platformCount > oldMaxScore)
        {
            SaveHighScore();
            LoadHighScore();
            //hiScoreText.text = $"HI-SCORE: {oldMaxScore.ToString()}";
        }
        #endregion

        #region ||>> DEATH BELOW LAST PLAT <<||

        if (transform.position.y < yMinReachable - 5f && isAlive /*WallGeneration.Instance.ListaPlatforms[i].index == PlatformBehaviour.Index.ULTIMA*/)
        {
            //yMin = WallGeneration.Instance.ListaPlatforms[i].transform.position.y;
            playerPosOnCollision = transform.position;
            myRigidbody.velocity = new Vector2(0, 0);
            isAlive = false;
            deathMessage = Index.DEAD;
            Invoke(DeathLogic(), deathTimeDuration/*/2*/);
        }
        else
        { }
        #endregion

        #region |> MUSHROOM JUMPER ò_ò <|
        //MUSHROOM JUMPER ò_ò
        if (mushroomJumper)
        {
            ActivateJumper();
        }
        else
        {
            DeactivateJumper();
        }
        #endregion
    }

    #endregion


    /// <summary>
    /// TO FIX AND UPDATE: KEYBOARD MOVEMENT
    /// </summary>
    void FixedUpdate()
    {

        #region <IS_GROUNDED CONTROL>
        RaycastHit2D ground = Physics2D.Raycast(capsuleCollider.bounds.min, Vector2.down, 0.1f);
        if (ground.collider != null)
        {
            Debug.Log("Sono a terra.");
            Debug.Log($"{ground.collider.gameObject.name}");
            isGrounded = true;
            isJumping = false;
            //myRigidbody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
        else
        {
            isGrounded = false;
        }
        #endregion

        #region <LANDING CONDITION TEST>
        //if (isJumping && !isNearGround)
        //{
        //    RaycastHit2D landing = Physics2D.Raycast(capsuleCollider.bounds.min, Vector2.down, 0.4f);
        //    if (landing.collider != null)
        //    {
        //        //isLanding = true;
        //        isNearGround = true;
        //        //PlayerAnimator.SetTrigger("Landing");
        //        //minLandDelayCounter = minLandDelay;
        //    }
        //    else
        //    {
        //        isNearGround = false;
        //    }

        //}
        #endregion

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

                    myRigidbody.AddForce(Vector2.right * fromLeftJetForce * Time.deltaTime, ForceMode2D.Impulse);

                    //myRigidbody.velocity = new Vector2(fromLeftJetForce * Vector2.right.x, myRigidbody.velocity.y); //VELOCE

                    //myRigidbody.AddForceAtPosition(Vector2.right*fromLeftJetForce, )
                }
            }
            else
            {
                Debug.Log("Sono in aria.");
                ChangeSprite(spriteArray[1]);
                //ChangeAnimationRight();

                myRigidbody.AddForce(Vector2.right * fromLeftJetForce * Time.deltaTime, ForceMode2D.Impulse);

                //myRigidbody.velocity = new Vector2(fromLeftJetForce * Vector2.right.x, myRigidbody.velocity.y); //VELOCE
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

                    myRigidbody.AddForce(Vector2.left * fromRightJetForce * Time.deltaTime, ForceMode2D.Impulse);

                    //myRigidbody.velocity = new Vector2(fromRightJetForce * Vector2.left.x, myRigidbody.velocity.y); //VELOCE

                    //myRigidbody.AddForceAtPosition(Vector2.right*fromLeftJetForce, )
                }
            }
            else
            {
                Debug.Log("Sono in aria.");
                ChangeSprite(spriteArray[0]);
                //ChangeAnimationLeft();

                myRigidbody.AddForce(Vector2.left * fromRightJetForce * Time.deltaTime, ForceMode2D.Impulse);

                //myRigidbody.velocity = new Vector2(fromRightJetForce * Vector2.left.x, myRigidbody.velocity.y); //VELOCE
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
                myRigidbody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
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

            //myRigidbody.AddForce(Vector2.up * jetpackForce, ForceMode2D.Force);

            RaycastHit2D hit2 = Physics2D.Raycast(capsuleCollider.bounds.min, Vector2.down, 0.1f);
            if (remainingFuel > 0)
            {
                if (hit2.collider != null)
                {
                    Debug.Log("Sono a terra.");
                    if (!jetpackInAirOnly)
                    {
                        myRigidbody.AddForce(Vector2.up * jetpackForce * Time.deltaTime, ForceMode2D.Impulse);
                        remainingFuel -= fuelPerSecond * Time.deltaTime;

                        //myRigidbody.velocity = new Vector2(myRigidbody.velocity.x, jetpackForce * Vector2.up.y); //VELOCE
                    }
                }
                else
                {
                    Debug.Log("Sono in aria.");

                    myRigidbody.AddForce(Vector2.up * jetpackForce * Time.deltaTime, ForceMode2D.Impulse);
                    remainingFuel -= fuelPerSecond * Time.deltaTime;

                    //myRigidbody.velocity = new Vector2(myRigidbody.velocity.x, jetpackForce * Vector2.up.y); //VELOCE
                }
            }
            else
            {
            }
        }
        else
        { spacePressed = false; }
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

        //    if (hitz.collider != null&& myRigidbody.velocity.magnitude>10)
        //{
        //    this.gameObject.SetActive(false);
        //}
        #endregion
    }

    //void OnBecameInvisible()
    //{
    //    Destroy(gameObject);
    //}

    #region >> syntax info: getting child/parents <<

    ///collision.gameObject.transform.parent.gameObject
    ///Prende parent come GameObject: oggettofiglio.transform.parent.gameObject
    ///se invece: oggettofiglio.transform.parent --> è preso come Transform;
    ///
    ///Prende child come GameObject: oggettoparent.transform.getchild(n).gameObject
    ///se invece: oggettoparent.transform.getchild(n) --> è preso come Transform.
    #endregion
}
