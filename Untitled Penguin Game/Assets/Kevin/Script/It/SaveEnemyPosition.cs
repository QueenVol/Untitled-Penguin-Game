using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveEnemyPosition : MonoBehaviour
{
    private static Vector3 cachedEnemyPos;
    private static string cachedSceneName = "";

    void Awake()
    {
        string currentScene = SceneManager.GetActiveScene().name;

        if (cachedSceneName == currentScene && cachedEnemyPos != Vector3.zero)
        {
            transform.position = cachedEnemyPos;
        }
        else
        {
            cachedSceneName = currentScene;
            cachedEnemyPos = Vector3.zero;
        }
    }

    void OnDestroy()
    {
        cachedEnemyPos = transform.position;
    }
}
