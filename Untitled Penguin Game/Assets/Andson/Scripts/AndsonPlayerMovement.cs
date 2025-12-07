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

    public Transform startPlace;

    public bool isGrounded;
    public bool isBlownUp;
    private bool wasGroundedLastFrame;

    public GameObject tutorTalk;
    public GameObject endTalk;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true; // 防止物理系统把角色撞倒
        wasGroundedLastFrame = true;   // 一开始默认在地面上

    }

    void Update()
    {
        if (!StartScreenTexts.isPaused)
        {
            // --- Ground Check ---
            isGrounded = Physics.Raycast(transform.position, Vector3.down, 1.1f);

            if (isBlownUp && !wasGroundedLastFrame && isGrounded)
            {
                isBlownUp = false;
            }

            // 记住这帧的结果，下一帧用来对比
            wasGroundedLastFrame = isGrounded;

            // --- Input ---
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");

            Vector3 camForward = cameraTransform.forward;
            Vector3 camRight = cameraTransform.right;

            camForward.y = 0;
            camRight.y = 0;
            camForward.Normalize();
            camRight.Normalize();

            moveDirection = (camForward * vertical + camRight * horizontal).normalized;

            if (moveDirection.magnitude > 0.1f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }

            if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
            {
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            }

            // ★ 展示爆炸中的特效
            if (!isGrounded && isBlownUp)
            {
                staticSprite.SetActive(false);
                movingSprite.SetActive(true);
                movingEffect.SetActive(true);
            }
            else if (isGrounded)  // 落地统一恢复
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
        if (other.CompareTag("Void"))
        {
            this.transform.position = startPlace.position;
        }

        if (other.CompareTag("Tutor"))
        {
            tutorTalk.SetActive(true);
        }
        if (other.CompareTag("End"))
        {
            endTalk.SetActive(true);
        }

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

        isBlownUp = true;

        // 施加冲击力
        rb.AddForce(finalForce, ForceMode.Impulse);

        // 可选：如果这个物体以后完全没用了，可以直接关掉它的 Collider
         other.enabled = false;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Tutor"))
        {
            tutorTalk.SetActive(false);
        }
        if (other.CompareTag("End"))
        {
            endTalk.SetActive(false);
        }
    }
}
