using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveEnemyPosition : MonoBehaviour
{
    private string sceneName;
    private string enemyKey;

    void Start()
    {
        sceneName = SceneManager.GetActiveScene().name;
        enemyKey = sceneName + "_Enemy";

        LoadEnemyPosition();
    }

    void OnDestroy()
    {
        if (!SavePlayerPosition.isReloadingByDeath)
        {
            SaveEnemyPositionData();
        }
        else
        {
        }
    }

    void SaveEnemyPositionData()
    {
        PlayerPrefs.SetFloat(enemyKey + "_x", transform.position.x);
        PlayerPrefs.SetFloat(enemyKey + "_y", transform.position.y);
        PlayerPrefs.Save();
    }

    void LoadEnemyPosition()
    {
        if (PlayerPrefs.HasKey(enemyKey + "_x"))
        {
            float x = PlayerPrefs.GetFloat(enemyKey + "_x");
            float y = PlayerPrefs.GetFloat(enemyKey + "_y");

            transform.position = new Vector3(x, y, transform.position.z);
        }
    }
}
