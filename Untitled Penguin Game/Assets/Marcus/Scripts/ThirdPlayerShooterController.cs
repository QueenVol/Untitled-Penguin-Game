using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using StarterAssets;
public class ThirdPlayerShooter : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera _aimVirtualCamera;
    [SerializeField] private float _aimSensitivity = 1.0f;
    [SerializeField] private float _normalSensitivity = 1.0f;
    private StarterAssetsInputs _input;
    private ThirdPersonController _thirdPersonController;
    [SerializeField] private LayerMask aimColliderLayer = new LayerMask();
    [SerializeField] private Transform _debugTransform;
    private void Awake()
    {
        _input = GetComponent<StarterAssetsInputs>();
        _thirdPersonController = GetComponent<ThirdPersonController>();
    }

    private void Update()
    {
        Vector3 mousePosition = Vector3.zero;

        Vector2 screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);
        Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);
        Transform hitTransform = null;
        if (Physics.Raycast(ray, out RaycastHit hit, 999f, aimColliderLayer)) {
            _debugTransform.position = hit.point;
            mousePosition = hit.point;
            hitTransform = hit.transform;
        } 
        if (_input.aim) {
            _aimVirtualCamera.gameObject.SetActive(true);
            _thirdPersonController.SetSensitivity(_aimSensitivity);
            _thirdPersonController.SetCanRotate(false);

            Vector3 worldAimTarget = mousePosition;
            worldAimTarget.y = transform.position.y;
            Vector3 aimDirection = (worldAimTarget - transform.position).normalized;

            transform.forward = Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime * 20f);
        } else {
            _aimVirtualCamera.gameObject.SetActive(false);
            _thirdPersonController.SetSensitivity(_normalSensitivity);
            _thirdPersonController.SetCanRotate(true);
        }
        
        if (_input.shoot) {
            if (hitTransform != null) {
                if (hitTransform.GetComponent<Health>() != null) {
                    Debug.Log("Shooting at " + hitTransform.name);
                }
                else {
                    Debug.Log("No health component found on " + hitTransform.name);
                }
            }

            _input.shoot = false;
        }
    }
}
