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

    private bool isAttacking = false;
    public float attackDuration = 0.3f;
    private float attackTimer = 0f;

    public AudioSource audioSource;
    public AudioClip attack;

    public float vibrationDuration = 0.05f;
    public float lowFreq = 0.1f;
    public float highFreq = 0.3f;

    // ---- NEW: Attack Rumble Timings ----
    public float rumbleDelay1 = 0.05f;
    public float rumbleDelay2 = 0.20f;

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
        Attack();

        if (isAttacking)
        {
            attackTimer -= Time.deltaTime;
            if (attackTimer <= 0f)
            {
                isAttacking = false;
            }
        }

    }

    void CheckGrounded()
    {
        isGround = myFeet.IsTouchingLayers(LayerMask.GetMask("Ground"));
    }

    void Attack()
    {
        if ((Input.GetMouseButtonDown(0))
            && !isAttacking)
        {
            myAnim.SetBool("Attack", true);

            isAttacking = true;
            attackTimer = attackDuration;
            audioSource.PlayOneShot(attack, 1f);

        }
        else
        {
            myAnim.SetBool("Attack", false);
        }
    }


    void Flip()
    {
        bool playerHasXAxisSpeed = Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon;
        if (playerHasXAxisSpeed)
        {
            if (myRigidbody.velocity.x > 0.1f)
                transform.localRotation = Quaternion.Euler(0, 0, 0);

            if (myRigidbody.velocity.x < -0.1f)
                transform.localRotation = Quaternion.Euler(0, 180, 0);
        }
    }

    void Running()
    {
        if (isAttacking)
        {
            myRigidbody.velocity = new Vector2(0, myRigidbody.velocity.y);
            return;
        }

        float moveDir = Input.GetAxis("Horizontal");
        Vector2 playerVel = new Vector2(moveDir * runSpeed, myRigidbody.velocity.y);

        myRigidbody.velocity = playerVel;
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

    void SwitchAnimation()
    {
        if (myRigidbody.velocity.y < 0.0f)
        {
            // Falling animation
        }
        else if (isGround)
        {
            // Idle animation
        }
    }
}
