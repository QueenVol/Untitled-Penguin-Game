using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CatchPlayer : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!ChaseZone.canCatchPlayer) return;

        if (other.CompareTag("Player"))
        {
            PlayerRespawn respawn = other.GetComponent<PlayerRespawn>();
            if (respawn != null)
            {
                respawn.Respawn();
            }

            if (MusicManager.Instance != null)
            {
                MusicManager.Instance.PlayNormalMusic();
            }

            ChaseZone zone = FindObjectOfType<ChaseZone>();
            if (zone != null)
            {
                zone.ClearEnemy();
            }

            Destroy(gameObject);

            ChaseZone.canCatchPlayer = false;
        }
    }
}
