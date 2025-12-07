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

    // 一帧输入的快照
    private struct InputSnapshot
    {
        public float time;        // 输入发生的时间
        public float horizontal;  // 水平轴
        public float vertical;    // 垂直轴
        public bool jumpPressed;  // 这一帧有没有按下跳跃键（Space）
    }

    public static float inputDelay = 0f;


    private Queue<InputSnapshot> inputQueue = new Queue<InputSnapshot>();

    // 当前已经“延迟后生效”的输入值
    private float delayedHorizontal;
    private float delayedVertical;


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

            // ====== 1. 采样真实输入，压入队列（立即不生效） ======
            float realHorizontal = Input.GetAxisRaw("Horizontal");
            float realVertical = Input.GetAxisRaw("Vertical");
            bool realJumpPressed = Input.GetKeyDown(KeyCode.Space);

            EnqueueInput(realHorizontal, realVertical, realJumpPressed);

            // ====== 2. 处理队列，找出已经延迟够久的输入并执行 ======
            ProcessDelayedInputs();

            // ====== 3. 用“延迟后的输入”来计算 moveDirection 和旋转 ======

            Vector3 camForward = cameraTransform.forward;
            Vector3 camRight = cameraTransform.right;

            camForward.y = 0;
            camRight.y = 0;
            camForward.Normalize();
            camRight.Normalize();

            // 注意这里用的是 delayedVertical / delayedHorizontal
            moveDirection = (camForward * delayedVertical + camRight * delayedHorizontal).normalized;

            if (moveDirection.magnitude > 0.1f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
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

        if (Input.anyKeyDown || Input.GetMouseButtonDown(0))
        {
            if (!StartScreenTexts.isPaused)
            {
                //inputDelay += 0.008f;
                Time.timeScale += 0.04F;
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
            inputQueue.Clear();
            delayedHorizontal = 0f;
            delayedVertical = 0f;
            moveDirection = Vector3.zero;
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

    void EnqueueInput(float horizontal, float vertical, bool jumpPressed)
    {
        InputSnapshot snapshot = new InputSnapshot
        {
            time = Time.time,
            horizontal = horizontal,
            vertical = vertical,
            jumpPressed = jumpPressed
        };
        inputQueue.Enqueue(snapshot);
    }

    void ProcessDelayedInputs()
    {
        // 把队列里“已经超过 delay 时间”的输入拿出来依次执行
        while (inputQueue.Count > 0)
        {
            var snap = inputQueue.Peek();
            if (Time.time - snap.time >= inputDelay)
            {
                // 这个输入现在才“生效”
                delayedHorizontal = snap.horizontal;
                delayedVertical = snap.vertical;

                // 跳跃：在延迟之后才真正触发 AddForce
                if (snap.jumpPressed && isGrounded)
                {
                    rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                }

                inputQueue.Dequeue();
            }
            else
            {
                // 队首还没到时间，后面的就更不可能到时间了
                break;
            }
        }
    }
}
