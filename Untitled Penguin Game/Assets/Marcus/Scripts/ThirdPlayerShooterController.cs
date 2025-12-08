using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using StarterAssets;
using UnityEngine.InputSystem;
using System;
using UnityEngine.SceneManagement;

public class ThirdPlayerShooter : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera _aimVirtualCamera;
    [SerializeField] private float _aimSensitivity = 1.0f;
    [SerializeField] private float _normalSensitivity = 1.0f;
    [SerializeField] private GameObject pfBubbleBullet;
    [SerializeField] private Transform spawnBulletPosition;
    
    [SerializeField] private bool isBubbleMode = false;
    private StarterAssetsInputs _input;
    private ThirdPersonController _thirdPersonController;
    [SerializeField] private LayerMask aimColliderLayer = new LayerMask();
    [SerializeField] private Transform _debugTransform;
    [SerializeField] private Animator _animator;
    [SerializeField] private float _sensitivity_additive = 0.0f;
    [SerializeField] private float _sensitivityIncreaseRate = 0.0f;

    [Header("Scene Transition Settings")]
    [SerializeField] private float _sensitivityThreshold = 40.0f;
    [SerializeField] private List<string> _randomScenes;

    private void Awake()
    {
        _input = GetComponent<StarterAssetsInputs>();
        _thirdPersonController = GetComponent<ThirdPersonController>();
        _animator = GetComponent<Animator>();
        _sensitivity_additive = 0.0f;
        
        LoadSceneState();
    }

    private void Update()
    {
        _sensitivity_additive += _sensitivityIncreaseRate * Time.deltaTime;

        if (_sensitivity_additive >= _sensitivityThreshold)
        {
            if (_randomScenes != null && _randomScenes.Count > 0)
            {
                List<string> availableScenes = new List<string>();

                foreach (string scene in _randomScenes)
                {
                    if (!IsSceneFinished(scene))
                    {
                        availableScenes.Add(scene);
                    }
                }

                // If all scenes are finished, fall back to the full list (or handle as game complete)
                if (availableScenes.Count == 0)
                {
                    Debug.Log("All scenes are finished! Picking from full list.");
                    availableScenes = _randomScenes;
                }

                int randomIndex = UnityEngine.Random.Range(0, availableScenes.Count);
                string sceneToLoad = availableScenes[randomIndex];
                
                Debug.Log($"Sensitivity threshold reached! Loading scene: {sceneToLoad}");
                
                SaveSceneState();

                _sensitivity_additive = 0f; 
                SceneManager.LoadScene(sceneToLoad);
                return;
            }
        }

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
            _thirdPersonController.SetSensitivity(_aimSensitivity + _sensitivity_additive);
            _thirdPersonController.SetCanRotate(false);
            _animator.SetLayerWeight(1, Mathf.Lerp(_animator.GetLayerWeight(1), 1f, Time.deltaTime * 10f));

            Vector3 worldAimTarget = mousePosition;
            worldAimTarget.y = transform.position.y;
            Vector3 aimDirection = (worldAimTarget - transform.position).normalized;

            transform.forward = Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime * 20f);
        } else {
            _aimVirtualCamera.gameObject.SetActive(false);
            _thirdPersonController.SetSensitivity(_normalSensitivity + _sensitivity_additive);
            _thirdPersonController.SetCanRotate(true);
            _animator.SetLayerWeight(1, Mathf.Lerp(_animator.GetLayerWeight(1), 0f, Time.deltaTime * 10f));
        }
        
        if (_input.shoot) {
            // Only shoot if aiming
            if (_input.aim)
            {
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
            }

            _input.shoot = false;
        }
    }

    private void SaveSceneState()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        string key = sceneName + "_PlayerState";

        PlayerPrefs.SetFloat(key + "_PosX", transform.position.x);
        PlayerPrefs.SetFloat(key + "_PosY", transform.position.y);
        PlayerPrefs.SetFloat(key + "_PosZ", transform.position.z);

        PlayerPrefs.SetFloat(key + "_RotX", transform.rotation.x);
        PlayerPrefs.SetFloat(key + "_RotY", transform.rotation.y);
        PlayerPrefs.SetFloat(key + "_RotZ", transform.rotation.z);
        PlayerPrefs.SetFloat(key + "_RotW", transform.rotation.w);

        PlayerPrefs.SetInt(key + "_BubbleMode", isBubbleMode ? 1 : 0);
        PlayerPrefs.SetInt(key + "_Exists", 1);
        
        PlayerPrefs.Save();
        Debug.Log($"Saved state for {sceneName}");
    }

    private void LoadSceneState()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        string key = sceneName + "_PlayerState";

        if (PlayerPrefs.HasKey(key + "_Exists"))
        {
            float x = PlayerPrefs.GetFloat(key + "_PosX");
            float y = PlayerPrefs.GetFloat(key + "_PosY");
            float z = PlayerPrefs.GetFloat(key + "_PosZ");
            
            float rotX = PlayerPrefs.GetFloat(key + "_RotX");
            float rotY = PlayerPrefs.GetFloat(key + "_RotY");
            float rotZ = PlayerPrefs.GetFloat(key + "_RotZ");
            float rotW = PlayerPrefs.GetFloat(key + "_RotW");

            bool savedBubbleMode = PlayerPrefs.GetInt(key + "_BubbleMode") == 1;
            
            // Disable CharacterController temporarily to set position
            CharacterController cc = GetComponent<CharacterController>();
            bool wasEnabled = false;
            if (cc != null) 
            {
                wasEnabled = cc.enabled;
                cc.enabled = false;
            }
            
            transform.position = new Vector3(x, y, z);
            transform.rotation = new Quaternion(rotX, rotY, rotZ, rotW);
            
            if (cc != null) cc.enabled = wasEnabled;

            isBubbleMode = savedBubbleMode;
            Debug.Log($"Loaded state for {sceneName}");
        }
    }

    public void IncreaseSensitivity(float amount)
    {
        _sensitivityIncreaseRate = amount;
    }

    private bool IsSceneFinished(string sceneName)
    {
        
        if (sceneName == "AndsonScene") return AndsonAcrossSceneSaver.AndsonHasFinished;
        if (sceneName == "KevinMainScene") return KevinIsFinished.kevinIsFinished;
        if (sceneName == "Andy") return PlayerController.stupidAndyFinished;

        return false;
    }
}