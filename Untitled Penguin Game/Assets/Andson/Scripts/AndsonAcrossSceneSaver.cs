using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AndsonAcrossSceneSaver : MonoBehaviour
{
    public static AndsonAcrossSceneSaver Instance;

    public Vector3 playerPosition;


    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (SceneManager.GetActiveScene().name == "AndsonScene")
            {
                playerPosition = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>().position;
            }
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            SceneManager.LoadScene("AndsonTestScene");
            AndsonSoundSystem.instance.StopAllSounds();
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            SceneManager.LoadScene("AndsonScene");
            StartCoroutine(SetPlayerPosNextFrame());
        }
    }

    private IEnumerator SetPlayerPosNextFrame()
    {
        // 等一帧，确保场景里的对象都已经初始化好了
        yield return null;

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            player.transform.position = playerPosition;
        }
    }
}
