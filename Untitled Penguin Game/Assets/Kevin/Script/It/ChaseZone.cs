using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseZone : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform spawnPoint;
    public Transform player;

    public static bool canCatchPlayer = false;

    private GameObject spawnedEnemy;

    void OnTriggerStay2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        MusicManager.Instance.PlayChaseMusic();

        if (spawnedEnemy == null)
        {
            spawnedEnemy = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);

            EnemyChase chase = spawnedEnemy.GetComponent<EnemyChase>();
            chase.target = player;
            chase.isChasing = true;

            canCatchPlayer = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        if (spawnedEnemy != null)
        {
            spawnedEnemy.GetComponent<EnemyChase>().isChasing = false;
        }

        MusicManager.Instance.PlayNormalMusic();
        canCatchPlayer = false;
    }

    void Update()
    {
        if (spawnedEnemy == null && canCatchPlayer == false)
        {

        }
    }

    public void ClearEnemy()
    {
        spawnedEnemy = null;
    }
}
