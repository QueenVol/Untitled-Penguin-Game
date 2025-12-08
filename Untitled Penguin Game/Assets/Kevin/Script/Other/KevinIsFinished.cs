using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KevinIsFinished : MonoBehaviour
{
    public static bool kevinIsFinished = false;

    public float delayTime = 2f;

    private bool hasTriggered = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (hasTriggered) return;

        if (other.CompareTag("Player"))
        {
            hasTriggered = true;

            StartCoroutine(DelayTriggerEnd());
        }
    }

    IEnumerator DelayTriggerEnd()
    {
        yield return new WaitForSeconds(delayTime);
        kevinIsFinished = true;
        EggSpawner spawner = FindObjectOfType<EggSpawner>();
        if (spawner != null)
        {
            spawner.YKYVersionLoadRandomSceneBasedOnBools();
        }
    }
}