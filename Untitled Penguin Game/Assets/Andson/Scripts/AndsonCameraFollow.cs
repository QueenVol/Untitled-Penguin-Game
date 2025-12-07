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

    void Start()
    {
       
    }

    void LateUpdate()
    {
        if (!StartScreenTexts.isPaused)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            // --- Mouse look ---
            yaw += Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            pitch -= Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
            pitch = Mathf.Clamp(pitch, -40f, 90f);

            // --- Apply rotation to CameraRig ---
            transform.rotation = Quaternion.Euler(pitch, yaw, 0);

            // --- Put Camera behind and above player ---
            Vector3 offset = transform.rotation * new Vector3(0, height, -distance);
            transform.position = target.position + offset;
        }

       
    }
}
