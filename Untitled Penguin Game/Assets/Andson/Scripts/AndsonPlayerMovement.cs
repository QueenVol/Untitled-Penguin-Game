using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AndsonPlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotationSpeed = 10f;
    public float jumpForce = 5f;

    public Transform cameraTransform;
    private Rigidbody rb;
    private Vector3 moveDirection;


    public GameObject staticSprite;
    public GameObject movingSprite;
    public GameObject movingEffect;


    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true; // 防止物理系统把角色撞倒
    }

    void Update()
    {
        // --- Ground Check ---
        isGrounded = Physics.Raycast(transform.position, Vector3.down, 1.1f);

        // --- Input ---
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        // camera-based move direction
        Vector3 camForward = cameraTransform.forward;
        Vector3 camRight = cameraTransform.right;

        camForward.y = 0;
        camRight.y = 0;
        camForward.Normalize();
        camRight.Normalize();

        moveDirection = (camForward * vertical + camRight * horizontal).normalized;

        // --- Rotate towards movement direction ---
        if (moveDirection.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        // --- Jump ---
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }

        if (moveDirection.magnitude != 0)
        {
            staticSprite.SetActive(false);
            movingSprite.SetActive(true);
            movingEffect.SetActive(true);
        }
        else
        {
            staticSprite.SetActive(true);
            movingSprite.SetActive(false);
            movingEffect.SetActive(false);

        }
    }

    void FixedUpdate()
    {
        // --- Move using Rigidbody ---
        Vector3 targetVelocity = moveDirection * moveSpeed;
        Vector3 velocity = new Vector3(targetVelocity.x, rb.velocity.y, targetVelocity.z);
        rb.velocity = velocity;
    }
}
