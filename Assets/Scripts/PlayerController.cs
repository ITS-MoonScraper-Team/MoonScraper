using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D Rigidbody;
    CapsuleCollider2D capsuleCollider;
    SpriteRenderer spriteRenderer;
    public FixedJoystick joystick;
    public Sprite[] spriteArray;

    bool inputPressed;
    private bool spacePressedDown;
    private bool spacePressed;
    private bool spacePressedUp;
    private bool leftPressed;

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
    }

    //Cambia sprite direzione movimento
    void ChangeSprite(Sprite _newSprite)
    {
        spriteRenderer.sprite = _newSprite;
    }

    // Update is called once per frame
    void Update()
    {
        Sprite spriteActive = spriteRenderer.sprite;
        //DISTRUGGE SPRITE attiva: SpriteRenderer.Destroy(spriteActive);

        //JOYSTICK MOVEMENT
        if (JoystickControl)
        {
            if (joystick.Vertical > 0)
            {
                Rigidbody.velocity = new Vector2(Rigidbody.velocity.x, jetpackForce * joystick.Vertical);
            }

            //Check Facing
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
            //KEYBOARD LEFT MOVEMENT
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
                        Rigidbody.AddForce(Vector2.right * fromLeftJetForce, ForceMode2D.Force);
                        //Rigidbody.AddForceAtPosition(Vector2.right*fromLeftJetForce, )
                    }
                }
                else
                {
                    Debug.Log("Sono in aria.");
                    ChangeSprite(spriteArray[1]);
                    Rigidbody.AddForce(Vector2.right * fromRightJetForce, ForceMode2D.Force);
                }
            }

            //KEYBOARD RIGHT MOVEMENT
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
                        Rigidbody.AddForce(Vector2.left * fromLeftJetForce, ForceMode2D.Force);
                        //Rigidbody.AddForceAtPosition(Vector2.right*fromLeftJetForce, )
                    }
                }
                else
                {
                    Debug.Log("Sono in aria.");
                    ChangeSprite(spriteArray[0]);
                    Rigidbody.AddForce(Vector2.left * fromRightJetForce, ForceMode2D.Force);
                }
            }

            //KEYBOARD JUMP
            if (Input.GetKeyDown(KeyCode.Space))
            {
                spacePressedDown = true;
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
                spacePressedDown = false;
            }

            //KEYBOARD PROPULSION
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
                        Rigidbody.AddForce(Vector2.up * jetpackForce, ForceMode2D.Force);
                    }
                }
                else
                {
                    Debug.Log("Sono in aria.");
                    Rigidbody.AddForce(Vector2.up * jetpackForce, ForceMode2D.Force);
                }
            }
            else
            {
                spacePressed = false;
            }
        
    }
}
