using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseZone : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform spawnPoint;
    public Transform player;

    private GameObject spawnedEnemy;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            MusicManager.Instance.PlayChaseMusic();

            if (spawnedEnemy == null)
            {
                Vector3 spawnPos = spawnPoint.position;

                string sceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
                string enemyKey = sceneName + "_Enemy";

                if (PlayerPrefs.HasKey(enemyKey + "_x"))
                {
                    float x = PlayerPrefs.GetFloat(enemyKey + "_x");
                    float y = PlayerPrefs.GetFloat(enemyKey + "_y");
                    spawnPos = new Vector3(x, y, 0);
                }

                spawnedEnemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);

                EnemyChase chase = spawnedEnemy.GetComponent<EnemyChase>();
                chase.target = player;
                chase.isChasing = true;

                CatchPlayer.canCatchPlayer = true;
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (spawnedEnemy != null)
            {
                spawnedEnemy.GetComponent<EnemyChase>().isChasing = false;
            }

            MusicManager.Instance.PlayNormalMusic();

            CatchPlayer.canCatchPlayer = false;
        }
    }
}
