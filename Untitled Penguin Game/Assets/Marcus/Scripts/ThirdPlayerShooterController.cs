using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using StarterAssets;
public class ThirdPlayerShooter : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera _aimVirtualCamera;
    private StarterAssetsInputs _input;
    private void Awake()
    {
        _input = GetComponent<StarterAssetsInputs>();
    }

    private void Update()
    {
        if (_input.aim) {
            _aimVirtualCamera.gameObject.SetActive(true);
        } else {
            _aimVirtualCamera.gameObject.SetActive(false);
        }
    }
}
