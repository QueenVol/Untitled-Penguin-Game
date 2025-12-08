using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SavePlayerPosition : MonoBehaviour
{
    private string sceneName;
    private string keyX;
    private string keyY;

    void Awake()
    {
        sceneName = SceneManager.GetActiveScene().name;

        if (!sceneName.Contains("KevinMainScene"))
            return;

        keyX = "MYPLAYER_" + sceneName + "_x";
        keyY = "MYPLAYER_" + sceneName + "_y";

        if (!SceneEnterRuntimeFlag.hasEnteredKevinMainScene)
        {
            SceneEnterRuntimeFlag.hasEnteredKevinMainScene = true;
            return;
        }

        if (PlayerPrefs.HasKey(keyX))
        {
            float x = PlayerPrefs.GetFloat(keyX);
            float y = PlayerPrefs.GetFloat(keyY);
            transform.position = new Vector3(x, y, transform.position.z);
        }
    }

    void OnDestroy()
    {
        sceneName = SceneManager.GetActiveScene().name;

        if (!sceneName.Contains("KevinMainScene"))
            return;

        keyX = "MYPLAYER_" + sceneName + "_x";
        keyY = "MYPLAYER_" + sceneName + "_y";

        PlayerPrefs.SetFloat(keyX, transform.position.x);
        PlayerPrefs.SetFloat(keyY, transform.position.y);
        PlayerPrefs.Save();
    }
}
