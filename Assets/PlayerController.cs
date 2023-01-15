using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    bool inputPressed;
    Rigidbody2D Rigidbody;
    CapsuleCollider2D capsuleCollider;
    public float jumpForce;
    private bool spacePressedDown;
    private bool spacePressed;
    private bool spacePressedUp;
    private bool leftPressed;
    public float jetpackForce;
    public float fromLeftJetForce;
    public float fromRightJetForce;

    public Joystick joystick;

    public Sprite[] spriteArray;
    public SpriteRenderer spriteRenderer;
    //public Sprite newSprite;
    ///spriteRenderer = GameObject.GetComponent<SpriteRenderer>();

    public bool jetpackInAirOnly;
    public bool leftRightInAirOnly;
    // Start is called before the first frame update
    void Start()
    {
        Rigidbody = GetComponent<Rigidbody2D>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        //spriteActive = GetComponent<SpriteRenderer>();
    }

    void ChangeSprite(Sprite _newSprite)
    {
        spriteRenderer.sprite = _newSprite;
    }

    // Update is called once per frame
    void Update()
    {
        SpriteRenderer spriteActive = GetComponent<SpriteRenderer>(); //DISTRUGGE SPRITE
        //LEFT
        if (Input.GetKey(KeyCode.LeftArrow)|| joystick.Horizontal !=null)
        {
            RaycastHit2D hit2 = Physics2D.Raycast(capsuleCollider.bounds.min, Vector2.down, 0.1f);
            if (hit2.collider != null)
            {
                Debug.Log("Sono a terra.");
                Debug.Log($"{hit2.collider.gameObject.name}");

                //SpriteRenderer.Destroy (spriteActive); //DISTRUGGE SPRITE

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

        //RIGHT
        if (Input.GetKey(KeyCode.RightArrow) || joystick.Horizontal != null)
        {
            RaycastHit2D hit2 = Physics2D.Raycast(capsuleCollider.bounds.min, Vector2.down, 0.1f);
            if (hit2.collider != null)
            {
                Debug.Log("Sono a terra.");
                Debug.Log($"{hit2.collider.gameObject.name}");

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

        if (Input.GetKeyDown(KeyCode.Space))
        {
            spacePressedDown=true;
            
            //JUMP

            //bool grounded = false;
            //RaycastHit2D hit = Physics2D.CapsuleCast(transform.position, capsuleCollider.size, capsuleCollider.direction, 0f, Vector2.down, 6);
            //List<RaycastHit2D> hits = new List<RaycastHit2D>();

            RaycastHit2D hit2 = Physics2D.Raycast(capsuleCollider.bounds.min, Vector2.down, 0.1f);
            if (hit2.collider != null)
            {
                Debug.Log("Sono a terra.");
                Debug.Log($"{hit2.collider.gameObject.name}");
                Rigidbody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            }
            else
            {
                Debug.Log("Sono in aria.");
            }
        }
        else
        {
            spacePressedDown=false;
        }

        if (Input.GetKey(KeyCode.Space))
        {
            //PROPULSION
            spacePressed=true;

            //Rigidbody.AddForce(Vector2.up * jetpackForce, ForceMode2D.Force);

            RaycastHit2D hit2 = Physics2D.Raycast(capsuleCollider.bounds.min, Vector2.down, 0.1f);

            if (hit2.collider != null)
            {
                Debug.Log("Sono a terra.");
                if(!jetpackInAirOnly)
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
            spacePressed=false;
        }
    }

    void FixedUpdate()
    {
        
        
    }

    
}
