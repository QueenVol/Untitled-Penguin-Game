using UnityEngine;
using StarterAssets;

public class EndGameCheckpoint : MonoBehaviour
{
    [Tooltip("The visual effect to play when the game is won.")]
    public GameObject winEffectPrefab;

    [Tooltip("Where to spawn the effect. If null, spawns at this object's position.")]
    public Transform effectSpawnPoint;

    [Tooltip("Optional: The visual beacon/object to hide when the checkpoint is triggered.")]
    public GameObject beaconVisual;

    private bool _hasTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (_hasTriggered) return;

        if (other.CompareTag("Player") || other.GetComponent<ThirdPersonController>() != null || other.GetComponentInParent<ThirdPersonController>() != null)
        {
            _hasTriggered = true;
            GameManager.Instance.WinGame();
            ThirdPlayerShooter thirdPlayerShooter = FindObjectOfType<ThirdPlayerShooter>();
            if (thirdPlayerShooter != null)
            {
                thirdPlayerShooter.disturbThresholdReached = true;
            }
            PlayWinEffect();
            
            if (beaconVisual != null)
            {
                beaconVisual.SetActive(false);
            }
        }
    }

    private void PlayWinEffect()
    {
        if (winEffectPrefab != null)
        {
            Vector3 spawnPos = effectSpawnPoint != null ? effectSpawnPoint.position : transform.position;
            Quaternion spawnRot = effectSpawnPoint != null ? effectSpawnPoint.rotation : Quaternion.identity;
            Instantiate(winEffectPrefab, spawnPos, spawnRot);
        }
    }
}

