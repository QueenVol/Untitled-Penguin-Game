using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    [Header("Fixed Spawn Point")]
    public Transform spawnPoint;

    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        if (SceneEnterRuntimeFlag.hasEnteredKevinMainScene)
            return;

        GetComponent<PlayerRespawn>()?.Respawn();
    }

    public void Respawn()
    {
        if (spawnPoint == null)
        {
            return;
        }

        transform.position = spawnPoint.position;

        if (rb != null)
        {
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }
    }
}
