using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AndsonAcrossSceneSaver : MonoBehaviour
{
    public static AndsonAcrossSceneSaver Instance;
    public EggSpawner spawner;
    public Vector3 playerPosition;

    private bool hasSavedPlayerPosition = false;  // 是否已经有保存的位置

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // 监听场景加载
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }

    // 每次有场景被加载完就会调用
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 如果是回到 AndsonScene，就自动把玩家传送回保存的位置
        if (scene.name == "AndsonScene" && hasSavedPlayerPosition)
        {
            // 顺便重新拿一遍 spawner（因为旧的那个已经是别的场景里的了）
            spawner = FindObjectOfType<EggSpawner>();

            StartCoroutine(SetPlayerPosNextFrame());
        }
    }

    private void Update()
    {
        // 这里你原来 C 键什么都没做，我先留着不动
        if (Input.GetKeyDown(KeyCode.C))
        {
            SceneManager.LoadScene("AndsonScene");
            if (SceneManager.GetActiveScene().name == "AndsonScene")
            {
                
            }
        }

        if (SceneManager.GetActiveScene().name == "AndsonScene")
        {
            if (spawner != null && spawner.currentCount >= spawner.maxCount)
            {
                // 切走之前，保存玩家位置
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                if (player != null)
                {
                    playerPosition = player.transform.position;
                    hasSavedPlayerPosition = true;
                }

                AndsonSoundSystem.instance.StopAllSounds();

                int randomer = Random.Range(0, 3);

                if (randomer == 0)
                {
                    SceneManager.LoadScene("Andy");
                }
                else if (randomer == 1)
                {
                    SceneManager.LoadScene("KevinMainScene");
                }
                else
                {
                    SceneManager.LoadScene("Andy");
                }
            }
        }

        // 先判断 spawner 是否存在，避免在其他场景里报 NullReference
       
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

    // 如果以后你想在别的地方也保存位置，可以单独写个函数
    public void SavePlayerPosition()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerPosition = player.transform.position;
            hasSavedPlayerPosition = true;
        }
    }
}
