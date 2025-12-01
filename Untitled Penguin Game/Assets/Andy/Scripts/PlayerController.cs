using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float runSpeed;
    public float jumpSpeed;

    private Rigidbody2D myRigidbody;
    private Animator myAnim;
    private BoxCollider2D myFeet;
    private bool isGround;

    //Body parts
    public GameObject leg1;
    public GameObject leg2;

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
            if (myRigidbody.velocity.x > 0.1f)
            {
                myAnim.SetBool("Right", true);
                myAnim.SetBool("Left", false);
            }
            //transform.localRotation = Quaternion.Euler(0, 0, 0);

            if (myRigidbody.velocity.x < -0.1f)
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
        myAnim.SetBool("Running", true);
    }

    void Jump()
    {
        if ((Input.GetButtonDown("Jump")
              || Input.GetKeyDown("w")
              || Input.GetKeyDown("up"))
             && isGround)
        {
            Vector2 jumpVel = new Vector2(0.0f, jumpSpeed);
            myRigidbody.velocity = Vector2.up * jumpVel;

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
            //
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

    void SwitchAnimation()
    {
        if (myRigidbody.velocity.x > 0.1f || myRigidbody.velocity.x < -0.1f)
        {
            myAnim.SetBool("Running", true);
        }
        else
        {
            myAnim.SetBool("Running", false);
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
