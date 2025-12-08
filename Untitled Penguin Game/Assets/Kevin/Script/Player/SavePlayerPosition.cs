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

        keyX = "MYPLAYER_" + sceneName + "_x";
        keyY = "MYPLAYER_" + sceneName + "_y";

        if (!GameStartFlag.isNewGame && PlayerPrefs.HasKey(keyX))
        {
            float x = PlayerPrefs.GetFloat(keyX);
            float y = PlayerPrefs.GetFloat(keyY);
            transform.position = new Vector3(x, y, transform.position.z);
        }
    }

    void OnDestroy()
    {
        PlayerPrefs.SetFloat(keyX, transform.position.x);
        PlayerPrefs.SetFloat(keyY, transform.position.y);
        PlayerPrefs.Save();

        GameStartFlag.isNewGame = false;
    }
}
