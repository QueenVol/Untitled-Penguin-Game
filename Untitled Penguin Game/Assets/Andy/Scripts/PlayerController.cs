using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float runSpeed;
    public float jumpSpeed;

    private Rigidbody2D myRigidbody;
    private Animator myAnim;
    private BoxCollider2D myFeet;
    private bool isGround;
    public bool jumped = false;

    //Body parts
    public GameObject leg1;
    public GameObject leg2;
    public GameObject wing1;
    public GameObject wing2;

    public GameObject levelMusic;
    public GameObject levelMusic2;
    public GameObject musicTrigger;
    public GameObject musicTrigger2;
    public GameObject message;

    //states
    public bool isLeg = false;
    public bool isWing = false;

    public AudioSource audio;
    public AudioClip eggCrack;

    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        myAnim = GetComponent<Animator>();
        myFeet = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        Flip();
        Running();
        Jump();
        CheckGrounded();
        SwitchAnimation();


    }

    void CheckGrounded()
    {
        isGround = myFeet.IsTouchingLayers(LayerMask.GetMask("Ground"));
    }


    void Flip()
    {
        bool playerHasXAxisSpeed = Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon;
        if (playerHasXAxisSpeed)
        {
            if (myRigidbody.velocity.x > 0f)
            {
                myAnim.SetBool("Right", true);
                myAnim.SetBool("Left", false);
            }

            if (myRigidbody.velocity.x < -0f)
            {
                myAnim.SetBool("Left", true);
                myAnim.SetBool("Right", false);
            }
            //transform.localRotation = Quaternion.Euler(0, 180, 0);
        }
    }

    void Running()
    {

        float moveDir = Input.GetAxis("Horizontal");
        Vector2 playerVel = new Vector2(moveDir * runSpeed, myRigidbody.velocity.y);

        myRigidbody.velocity = playerVel;
    }

    void Jump()
    {
        if ((Input.GetButtonDown("Jump")
              || Input.GetKeyDown("w")
              || Input.GetKeyDown("up"))
             && isWing && jumped)
        {
            Debug.Log("Second Jump");
            Vector2 jumpVel = new Vector2(0.0f, jumpSpeed);
            myRigidbody.velocity = Vector2.up * jumpVel * 1.2f;
            jumped = false;
        }
        if ((Input.GetButtonDown("Jump")
              || Input.GetKeyDown("w")
              || Input.GetKeyDown("up"))
             && isGround)
        {
            Vector2 jumpVel = new Vector2(0.0f, jumpSpeed);
            myRigidbody.velocity = Vector2.up * jumpVel;
            jumped = true;
        }
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Leg")
        {
            Leg();
            isLeg = true;
        }

        if (collision.gameObject.tag == "Wing")
        {
            Wing();
            isWing = true;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Water")
        {
            myRigidbody.gravityScale = -1;
        }
        if (collision.gameObject.tag == "Grave")
        {
            message.SetActive(true);
        }
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Water")
        {
            myRigidbody.gravityScale = 1;
        }
        if (collision.gameObject.tag == "Music")
        {
            levelMusic.SetActive(true);
            Destroy(musicTrigger);
        }
        if (collision.gameObject.tag == "Music2")
        {
            levelMusic2.SetActive(true);
            Destroy(musicTrigger2);
        }
        if (collision.gameObject.tag == "Grave")
        {
            message.SetActive(false);
        }
    }

    void Leg()
    {
        if (isLeg == false)
        {
            audio.PlayOneShot(eggCrack);
            leg1.SetActive(true);
            leg2.SetActive(true);
            jumpSpeed = 8f;
        }
    }

    void Wing()
    {

        if (isWing == false)
        {
            audio.PlayOneShot(eggCrack);
            wing1.SetActive(true);
            wing2.SetActive(true);
        }
    }

    void SwitchAnimation()
    {
        if (Input.GetKey(KeyCode.A))
        {
            myAnim.SetBool("Left", true);
        }
        else
        {
            myAnim.SetBool("Left", false);
        }
        if (Input.GetKey(KeyCode.D))
        {
            myAnim.SetBool("Right", true);
        }
        else
        {
            myAnim.SetBool("Right", false);
        }

        if (myRigidbody.velocity.y < 0.0f)
        {
            //myAnim.SetBool("Jump", false);
            //myAnim.SetBool("Fall", true);
        }
        else if (isGround)
        {
            //myAnim.SetBool("Fall", false);
            myAnim.SetBool("Idle", true);
        }
    }
}
