using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CatchPlayer : MonoBehaviour
{
    public static bool canCatchPlayer = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!canCatchPlayer) return;

        if (other.CompareTag("Player"))
        {
            SavePlayerPosition.isReloadingByDeath = true;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

}
