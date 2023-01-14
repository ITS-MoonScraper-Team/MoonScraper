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
    public float jetpackForce;

    public bool jetpackInAirOnly;
    // Start is called before the first frame update
    void Start()
    {
        Rigidbody = GetComponent<Rigidbody2D>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            spacePressedDown=true;
            
            bool grounded = false;
            RaycastHit2D hit = Physics2D.CapsuleCast(transform.position, capsuleCollider.size, capsuleCollider.direction, 0f, Vector2.down, 6);
            List<RaycastHit2D> hits = new List<RaycastHit2D>();

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
            //JUMP
        }
        else
        {
            spacePressedDown=false;
        }
        if (Input.GetKey(KeyCode.Space))
        {
            //PROPULSION
            spacePressed=true;

           // Rigidbody.AddForce(Vector2.up * jetpackForce, ForceMode2D.Force);

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
