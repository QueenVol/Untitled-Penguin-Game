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

    public float explosionForce = 15f;   // 冲击力度
    public float upwardForce = 5f;       // 向上抬升，让玩家“跳起来”更像炸飞
    private HashSet<Collider> usedExplosionTriggers = new HashSet<Collider>();


    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true; // 防止物理系统把角色撞倒
    }

    void Update()
    {
        if (!StartScreenTexts.isPaused)
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

            if (rb.velocity.magnitude!=0)
            {
                staticSprite.SetActive(false);
                movingSprite.SetActive(true);
                movingEffect.SetActive(true);
            }
            else if (rb.velocity.magnitude<=0)
            {
                staticSprite.SetActive(true);
                movingSprite.SetActive(false);
                movingEffect.SetActive(false);

            }
        }

    }

    void FixedUpdate()
    {
        // --- Move using Rigidbody ---
        Vector3 targetVelocity = moveDirection * moveSpeed;
        Vector3 velocity = new Vector3(targetVelocity.x, rb.velocity.y, targetVelocity.z);
        rb.velocity = velocity;
    }

    private void OnTriggerEnter(Collider other)
    {
        // 如果你不想对某些东西触发，可以检查 Tag
        // if (!other.CompareTag("ExplosionObject")) return;

        if (!other.CompareTag("Fire")) return;
        // 计算玩家相对于物体的方向 = 玩家位置 - 物体位置
        if (usedExplosionTriggers.Contains(other))
        {
            return;
        }

        // 第一次触发：先记录下来，防止以后再次生效
        usedExplosionTriggers.Add(other);

        // 计算“玩家相对于物体”的方向（物体 -> 玩家）
        Vector3 direction = (transform.position - other.transform.position).normalized;

        // 最终力：水平推开 + 向上抬起
        Vector3 finalForce = direction * explosionForce + Vector3.up * upwardForce;

        // 可选：清掉当前速度，防止跟现有速度叠加出奇怪结果
        rb.velocity = Vector3.zero;

        // 施加冲击力
        rb.AddForce(finalForce, ForceMode.Impulse);

        // 可选：如果这个物体以后完全没用了，可以直接关掉它的 Collider
         other.enabled = false;
    }
}
