using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SavePlayerPosition : MonoBehaviour
{
    private string sceneName;

    public static bool isReloadingByDeath = false;

    void Awake()
    {
        PlayerPrefs.DeleteAll();
        sceneName = SceneManager.GetActiveScene().name;
        LoadPosition();
    }

    void OnDestroy()
    {
        if (!isReloadingByDeath)
        {
            SavePosition();
        }
        else
        {
            isReloadingByDeath = false;
        }
    }

    public void SavePosition()
    {
        PlayerPrefs.SetFloat(sceneName + "_x", transform.position.x);
        PlayerPrefs.SetFloat(sceneName + "_y", transform.position.y);
        PlayerPrefs.Save();
    }

    void LoadPosition()
    {
        if (PlayerPrefs.HasKey(sceneName + "_x"))
        {
            float x = PlayerPrefs.GetFloat(sceneName + "_x");
            float y = PlayerPrefs.GetFloat(sceneName + "_y");

            transform.position = new Vector3(x, y, transform.position.z);
        }
    }
}
