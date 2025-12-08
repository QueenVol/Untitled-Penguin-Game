using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SavePlayerPosition : MonoBehaviour
{
    private string sceneName;
    private string keyX;
    private string keyY;
    private string keyVisited;

    void Awake()
    {
        sceneName = SceneManager.GetActiveScene().name;

        if (sceneName == "StartScene" || sceneName == "EndScene")
        {
            return;
        }

        keyX = "MYPLAYER_" + sceneName + "_x";
        keyY = "MYPLAYER_" + sceneName + "_y";
        keyVisited = "MYPLAYER_" + sceneName + "_visited";

        bool hasVisitedBefore = PlayerPrefs.GetInt(keyVisited, 0) == 1;

        if (hasVisitedBefore && PlayerPrefs.HasKey(keyX))
        {
            float x = PlayerPrefs.GetFloat(keyX);
            float y = PlayerPrefs.GetFloat(keyY);
            transform.position = new Vector3(x, y, transform.position.z);
        }
    }

    void OnDestroy()
    {
        sceneName = SceneManager.GetActiveScene().name;

        if (sceneName == "StartScene" || sceneName == "EndScene")
            return;

        keyX = "MYPLAYER_" + sceneName + "_x";
        keyY = "MYPLAYER_" + sceneName + "_y";
        keyVisited = "MYPLAYER_" + sceneName + "_visited";

        PlayerPrefs.SetFloat(keyX, transform.position.x);
        PlayerPrefs.SetFloat(keyY, transform.position.y);
        PlayerPrefs.SetInt(keyVisited, 1);

        PlayerPrefs.Save();
    }
}
