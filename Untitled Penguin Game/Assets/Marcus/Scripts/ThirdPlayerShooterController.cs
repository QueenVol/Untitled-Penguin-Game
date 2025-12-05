using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using StarterAssets;
using UnityEngine.InputSystem;
using System;

public class ThirdPlayerShooter : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera _aimVirtualCamera;
    [SerializeField] private float _aimSensitivity = 1.0f;
    [SerializeField] private float _normalSensitivity = 1.0f;
    [SerializeField] private GameObject pfBubbleBullet;
    [SerializeField] private Transform spawnBulletPosition;
    
    private bool isBubbleMode = false;
    private StarterAssetsInputs _input;
    private ThirdPersonController _thirdPersonController;
    [SerializeField] private LayerMask aimColliderLayer = new LayerMask();
    [SerializeField] private Transform _debugTransform;
    [SerializeField] private Animator _animator;
    private void Awake()
    {
        _input = GetComponent<StarterAssetsInputs>();
        _thirdPersonController = GetComponent<ThirdPersonController>();
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        Vector3 mousePosition = Vector3.zero;

        Vector2 screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);
        Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);
        Transform hitTransform = null;
        if (Physics.Raycast(ray, out RaycastHit hit, 999f, aimColliderLayer)) {
            //_debugTransform.position = hit.point;
            mousePosition = hit.point;
            hitTransform = hit.transform;
        } else {
            mousePosition = ray.GetPoint(999f);
            //_debugTransform.position = mousePosition;
        } 

        // Toggle Shooting Mode
        if (Keyboard.current != null && Keyboard.current.gKey.wasPressedThisFrame)
        {
            isBubbleMode = !isBubbleMode;
            Debug.Log("Switched to " + (isBubbleMode ? "Bubble" : "Normal") + " mode");
        }

        if (_input.aim) {
            _aimVirtualCamera.gameObject.SetActive(true);
            _thirdPersonController.SetSensitivity(_aimSensitivity);
            _thirdPersonController.SetCanRotate(false);
            _animator.SetLayerWeight(1, Mathf.Lerp(_animator.GetLayerWeight(1), 1f, Time.deltaTime * 10f));

            Vector3 worldAimTarget = mousePosition;
            worldAimTarget.y = transform.position.y;
            Vector3 aimDirection = (worldAimTarget - transform.position).normalized;

            transform.forward = Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime * 20f);
        } else {
            _aimVirtualCamera.gameObject.SetActive(false);
            _thirdPersonController.SetSensitivity(_normalSensitivity);
            _thirdPersonController.SetCanRotate(true);
            _animator.SetLayerWeight(1, Mathf.Lerp(_animator.GetLayerWeight(1), 0f, Time.deltaTime * 10f));
        }
        
        if (_input.shoot) {
            if (isBubbleMode)
            {
                if (pfBubbleBullet != null)
                {
                    Vector3 spawnPos = spawnBulletPosition != null ? spawnBulletPosition.position : transform.position + Vector3.up * 1.5f;
                    Vector3 aimDir = (mousePosition - spawnPos).normalized;
                    Instantiate(pfBubbleBullet, spawnPos, Quaternion.LookRotation(aimDir, Vector3.up));
                }
                else
                {
                    Debug.LogWarning("Bubble Bullet Prefab not assigned!");
                }
            }
            else
            {
                if (hitTransform != null) {
                    if (hitTransform.GetComponent<Health>() != null) {
                        Debug.Log("Shooting at " + hitTransform.name);
                    }
                    else {
                        Debug.Log("No health component found on " + hitTransform.name);
                    }
                }
            }

            _input.shoot = false;
        }
    }
}
