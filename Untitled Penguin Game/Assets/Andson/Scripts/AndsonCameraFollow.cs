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

    void Start()
    {
       
    }

    void LateUpdate()
    {
        if (!StartScreenTexts.isPaused)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            // ====== 1. 捕获真实鼠标输入，入队 ======

            float mx = Input.GetAxis("Mouse X");
            float my = Input.GetAxis("Mouse Y");

            inputQueue.Enqueue(new InputSnapshot
            {
                time = Time.time,
                mouseX = mx,
                mouseY = my
            });

            // ====== 2. 处理延迟后的输入 ======
            ProcessDelayedInputs();

            // ====== 3. 根据延迟后的 yaw、pitch 移动摄像机 ======

            transform.rotation = Quaternion.Euler(pitch, yaw, 0);

            Vector3 offset = transform.rotation * new Vector3(0, height, -distance);
            transform.position = target.position + offset;
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
}
