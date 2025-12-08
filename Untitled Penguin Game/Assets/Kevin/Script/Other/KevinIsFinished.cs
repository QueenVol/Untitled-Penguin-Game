using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KevinIsFinished : MonoBehaviour
{
    public static bool kevinIsFinished = false;

    public void TheEnd()
    {
        EggSpawner spawner = FindObjectOfType<EggSpawner>();

        if (spawner != null)
        {
            spawner.YKYVersionLoadRandomSceneBasedOnBools();
        }
    }
}