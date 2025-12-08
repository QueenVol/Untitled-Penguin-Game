using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AndsonCameraFollow : MonoBehaviour
{
    public Transform target;       // Player
    public float distance = 4f;    // 摄像机距离
    public float height = 2f;      // 摄像机高度
    public float mouseSensitivity = 130f;

    private float yaw;   // 水平旋转
    private float pitch; // 垂直旋转

    // ========== 延迟输入系统 ==========
    private struct InputSnapshot
    {
        public float time;
        public float mouseX;
        public float mouseY;
    }

    private Queue<InputSnapshot> inputQueue = new Queue<InputSnapshot>();
    // ==================================

    // ====== 新增：抖动偏移 & 协程句柄 ======
    private Vector3 shakeOffset = Vector3.zero;
    private Coroutine shakeCoroutine;
    // ===================================

    void LateUpdate()
    {
        if (!StartScreenTexts.isPaused)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            // 1. 捕获真实鼠标输入，入队
            float mx = Input.GetAxis("Mouse X");
            float my = Input.GetAxis("Mouse Y");

            inputQueue.Enqueue(new InputSnapshot
            {
                time = Time.time,
                mouseX = mx,
                mouseY = my
            });

            // 2. 处理延迟后的输入
            ProcessDelayedInputs();

            // 3. 根据延迟后的 yaw、pitch 移动摄像机（注意这里加上 shakeOffset）
            transform.rotation = Quaternion.Euler(pitch, yaw, 0);

            Vector3 offset = transform.rotation * new Vector3(0, height, -distance);
            //  在原有的基础上加上抖动偏移
            transform.position = target.position + offset + shakeOffset;
        }
    }

    void ProcessDelayedInputs()
    {
        while (inputQueue.Count > 0)
        {
            var snap = inputQueue.Peek();

            if (Time.time - snap.time >= AndsonPlayerMovement.inputDelay)
            {
                yaw += snap.mouseX * mouseSensitivity * Time.deltaTime;
                pitch -= snap.mouseY * mouseSensitivity * Time.deltaTime;

                pitch = Mathf.Clamp(pitch, -40f, 90f);

                inputQueue.Dequeue();
            }
            else
            {
                break;
            }
        }
    }

    // ====== 新增：对外暴露一个Shake接口 ======
    public void StartShake(int currentCount, float baseShakeMagnitude, float baseShakeDuration)
    {
        if (shakeCoroutine != null)
            StopCoroutine(shakeCoroutine);

        shakeCoroutine = StartCoroutine(ShakeRoutine(currentCount, baseShakeMagnitude, baseShakeDuration));
    }

    private IEnumerator ShakeRoutine(int currentCount, float baseShakeMagnitude, float baseShakeDuration)
    {
        float magnitude = baseShakeMagnitude * (1 + currentCount * 0.5f);
        float duration = baseShakeDuration + currentCount * 0.05f;

        float timer = 0f;

        while (timer < duration)
        {
            // 用随机方向，更自然
            Vector2 dir = Random.insideUnitCircle.normalized;
            Vector2 offset2D = dir * (Random.Range(-1f, 1f) * magnitude);

            // 只在 X/Y 上抖，不动 Z
            shakeOffset = new Vector3(offset2D.x, offset2D.y, 0f);

            timer += Time.deltaTime;
            yield return null;
        }

        shakeOffset = Vector3.zero;
        shakeCoroutine = null;
    }
}
