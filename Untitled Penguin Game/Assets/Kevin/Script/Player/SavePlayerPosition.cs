using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SavePlayerPosition : MonoBehaviour
{
    private string sceneName;
    private string keyX;
    private string keyY;

    public static bool isReloadByDeath = false;
    private static bool allowSave = true;

    void Awake()
    {
        sceneName = SceneManager.GetActiveScene().name;

        keyX = "MYPLAYER_" + sceneName + "_x";
        keyY = "MYPLAYER_" + sceneName + "_y";

        allowSave = true;

        if (PlayerPrefs.HasKey(keyX))
        {
            float x = PlayerPrefs.GetFloat(keyX);
            float y = PlayerPrefs.GetFloat(keyY);

            transform.position = new Vector3(x, y, transform.position.z);
        }
    }

    void OnDestroy()
    {
        if (!isReloadByDeath && allowSave)
        {
            allowSave = false;

            PlayerPrefs.SetFloat(keyX, transform.position.x);
            PlayerPrefs.SetFloat(keyY, transform.position.y);
            PlayerPrefs.Save();
        }
    }
}
