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

    //public GameObject playerDeathExplode;
    //public GameObject playerIdleLeft;
    //public GameObject playerIdleRight;

    public FixedJoystick joystick;
    public Sprite[] spriteArray;

    //public TrailRenderer trailRenderer;
    //public GameObject Handle;

    public bool JoystickControl;
    public float jumpForce;
    public float jetpackForce;
    public float fromLeftJetForce;
    public float fromRightJetForce;
    public bool jetpackInAirOnly;
    public bool leftRightInAirOnly;

    #region State variables:
    private bool inputPressed;
    private bool upPressedDown;
    private bool spacePressedDown;
    private bool spacePressed;
    private bool spacePressedUp;
    private bool leftPressed;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        Rigidbody = GetComponent<Rigidbody2D>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        //polygonCollider = GetComponent<PolygonCollider2D>();

        //trailRenderer = GetComponentInChildren<TrailRenderer>();
        //rectTransform=Handle.GetComponent<RectTransform>(); 
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //if(capsuleCollider.CompareTag("walls"))
        //{
        //    Destroy(gameObject);
        //}
        if (collision.gameObject.layer == 3)
        {
            //Instantiate(playerDeathExplode, transform.position, transform.rotation);
            //Animation.pl
            Destroy(gameObject, 0.1f);
        }
        if (collision.gameObject.layer == 8)
        {
            if (transform.position.y < collision.transform.position.y)
                Destroy(gameObject, 0.1f);

            else if (transform.position.y >= collision.transform.position.y)
            {
                if (Mathf.Abs(collision.relativeVelocity.y) > 10.0f)
                    Destroy(gameObject, 0.1f);
            }
        }
    }

    //void OnBecameInvisible()
    //{
    //    Destroy(gameObject);
    //}

    // |>>> CAMBIA SPRITE DIREZIONE MOVIMENTO <<<|
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

    // Update is called once per frame
    void Update()
    {
        Debug.Log("VELOCITY " + Rigidbody.velocity.magnitude);


        #region |>>> JOYSTICK MOVEMENT <<<|

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
        //muore se scende troppo
        if(WallGeneration.Instance.listaStageGenerati.Count > 3)
        {
            if (transform.position.y < WallGeneration.Instance.listaStageGenerati[3].transform.position.y)
            {
                Destroy(gameObject);
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

        

        #region |>>> KEYBOARD MOVEMENT <<<|

        // |>> KEYBOARD LEFT <<|
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

        // |>>KEYBOARD RIGHT<<|
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

        // |>>KEYBOARD JUMP<<|
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

        // |>>KEYBOARD PROPULSION<<|
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
