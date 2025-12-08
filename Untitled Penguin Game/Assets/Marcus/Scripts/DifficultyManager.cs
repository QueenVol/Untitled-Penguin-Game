using UnityEngine;
using StarterAssets;

public class DifficultyManager : MonoBehaviour
{
    [Header("Sensitivity Settings")]
    [Tooltip("Initial mouse sensitivity.")]
    public float initialSensitivity = 1.0f;

    [Tooltip("Maximum mouse sensitivity cap.")]
    public float maxSensitivity = 10.0f;

    [Tooltip("How much sensitivity increases per second.")]
    public float sensitivityIncreaseRate = 0.1f;

    private float _currentSensitivity;
    private ThirdPersonController _playerController;

    private void Start()
    {
        // Find the player's controller
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            _playerController = player.GetComponent<ThirdPersonController>();
        }

        if (_playerController != null)
        {
            _currentSensitivity = initialSensitivity;
            _playerController.SetSensitivity(_currentSensitivity);
        }
        else
        {
            Debug.LogWarning("DifficultyManager: Could not find ThirdPersonController on Player object.");
        }
    }

    private void Update()
    {
        if (_playerController == null) return;

        // Increase sensitivity over time
        if (_currentSensitivity < maxSensitivity)
        {
            _currentSensitivity += sensitivityIncreaseRate * Time.deltaTime;
            _currentSensitivity = Mathf.Min(_currentSensitivity, maxSensitivity);
            
            _playerController.SetSensitivity(_currentSensitivity);
        }
    }
}

