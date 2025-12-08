using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SaveLoad : MonoBehaviour
{
    public static SaveLoad Instance;
    public PlayerController player;
    public Vector2 playerPosition;

    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        if (SceneManager.GetActiveScene().name == "Andy")
        {
            LoadPlayer();
        }
    }

    public void SavePlayer()
    {
        if (player == null) return;

        //PlayerPrefs.SetFloat("PlayerX", player.transform.position.x);
       // PlayerPrefs.SetFloat("PlayerY", player.transform.position.y);
        playerPosition = player.transform.position;
       // PlayerPrefs.Save();

      //  Debug.Log("Saved Position: " + player.transform.position);
    }

    public void LoadPlayer()
    {
        if (player == null) print("222");

        //if (!PlayerPrefs.HasKey("PlayerX"))
        //       return;

        // float x = PlayerPrefs.GetFloat("PlayerX");
        //  float y = PlayerPrefs.GetFloat("PlayerY");

        player.transform.position = playerPosition;

       // Debug.Log("Loaded Position: " + new Vector2(x, y));
    }

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("切换到 Scene：" + scene.name);
        StartCoroutine(DelayedSceneEnter());
    }

    IEnumerator DelayedSceneEnter()
    {
        // 等待一帧
        yield return null;

        // 尝试寻找 Player
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");

        // 如果该场景根本没有 Player，则直接跳过（避免报错）
        if (playerObj == null)
        {
            Debug.Log("Scene 不包含 Player，不执行玩家位置复原。");
            yield break;
        }

        player = playerObj.GetComponent<PlayerController>();

        // 再保险一次：player 脚本不存在
        if (player == null)
        {
            Debug.LogWarning("找到 Player 物体，但没有 PlayerController 组件。");
            yield break;
        }

        // 最终安全设置位置
        player.transform.position = playerPosition;
        Debug.Log("玩家位置已复原：" + playerPosition);
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
