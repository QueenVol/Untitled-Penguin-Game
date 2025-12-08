using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AndsonAcrossSceneSaver : MonoBehaviour
{
    public static AndsonAcrossSceneSaver Instance;
    public EggSpawner spawner;
    public Vector3 playerPosition;

    public static bool AndsonHasFinished = false;

    private bool hasSavedPlayerPosition = false;  // 是否已经有保存的位置

    public static bool hasEndScene = false;

  


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
                LoadAnotherRandomScene();
            }
        }

        IfEndScene();

        // 先判断 spawner 是否存在，避免在其他场景里报 NullReference

    }

    public void LoadAnotherRandomScene()
    {
        // 切走之前，保存玩家位置
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerPosition = player.transform.position;
            hasSavedPlayerPosition = true;
        }

        AndsonSoundSystem.instance.StopAllSounds();
        Time.timeScale = 1;
        AndsonPlayerMovement.inputDelay = 0f;

        LoadRandomSceneBasedOnBools();
    }
    public void LoadRandomSceneBasedOnBools()
    {
        List<string> scenePool = new List<string>();

        // 只要对应的 bool == false，就加入随机列表
        if (!KevinIsFinished.kevinIsFinished)
            scenePool.Add("KevinMainScene");

        if (!PlayerController.StupidAndyFinished)
            scenePool.Add("Andy");

        if (!GameManager.isGameWon)
            scenePool.Add("Playground 1");   

        // 如果列表为空 → 全部完成
        if (scenePool.Count == 0)
        {
            if (!AndsonHasFinished)
            {
                SceneManager.LoadScene("AndsonScene");
            }
            else
            {
            
            }
        }

        if (scenePool.Count != 0)
        {
            // 随机挑选
            int index = Random.Range(0, scenePool.Count);
            string targetScene = scenePool[index];

            Debug.Log("切换到 Scene：" + targetScene);
            SceneManager.LoadScene(targetScene);
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

    public void IfEndScene()
    {
        if (KevinIsFinished.kevinIsFinished && PlayerController.StupidAndyFinished && GameManager.isGameWon && AndsonHasFinished && !hasEndScene)
        {
            SceneManager.LoadScene("EndScene");
            hasEndScene = true;
        }
        
    }
}
