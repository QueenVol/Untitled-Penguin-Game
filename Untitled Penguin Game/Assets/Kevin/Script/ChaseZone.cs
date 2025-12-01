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
                spawnedEnemy = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);

                EnemyChase chase = spawnedEnemy.GetComponent<EnemyChase>();
                chase.target = player;
                chase.isChasing = true;
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
        }
    }
}
