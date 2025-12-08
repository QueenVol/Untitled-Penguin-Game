using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AndsonCentralRaySpawner : MonoBehaviour
{
    [Header("要生成的物体预制体")]
    public GameObject spawnPrefab;

    [Header("最大射线距离")]
    public float maxDistance = 100f;

    [Header("玩家对象（用来排除自己）")]
    public Transform player;   // 在 Inspector 里拖你的 Player 上来


    [Header("发射射线的按键")]
    public KeyCode fireKey = KeyCode.Mouse0; // 左键

    public Transform startPlace;

    private struct InputEvent
    {
        public float time;           // 输入发生的时间
        public System.Action action; // 延迟后要执行的逻辑
    }

    private Queue<InputEvent> inputQueue = new Queue<InputEvent>();

    void Update()
    {
        if (Input.GetKeyDown(fireKey) && SceneManager.GetActiveScene().name == "AndsonScene")
        {
            EnqueueAction(FireRayFromCenter);
        }

        ProcessDelayedInputs();



        if (Input.GetKeyDown(KeyCode.R))
        {
            player.transform.position = startPlace.position;
        }
    }

    void EnqueueAction(System.Action action)
    {
        inputQueue.Enqueue(new InputEvent
        {
            time = Time.time,
            action = action
        });
    }

    void ProcessDelayedInputs()
    {
        while (inputQueue.Count > 0)
        {
            var evt = inputQueue.Peek();
            // 到时间了才执行
            if (Time.time - evt.time >= AndsonPlayerMovement.inputDelay)
            {
                evt.action?.Invoke();
                inputQueue.Dequeue();
            }
            else
            {
                // 队首都没到时间，后面的也不可能到
                break;
            }
        }
    }


    void FireRayFromCenter()
    {
        Camera cam = Camera.main;
        if (cam == null)
        {
            Debug.LogWarning("找不到带 MainCamera tag 的摄像机！");
            return;
        }

        // 从屏幕中心 0.5,0.5 发射一条射线
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, maxDistance))
        {
            // 如果打到的是玩家自己，直接返回，不生成
            if (player != null && hit.transform == player)
            {
                return;
            }
            else if (hit.transform.tag == "Fire")
            {
                return;
            }

            // 也可以用 Tag 来排除玩家：
            // if (hit.collider.CompareTag("Player")) return;

            // 在击中位置生成物体，保持法线方向为 up 或对着法线都行
            Quaternion rot = Quaternion.LookRotation(hit.normal); // 让它“贴”在表面
            // Quaternion rot = Quaternion.identity; // 如果你不关心旋转，可以用这个

            if (spawnPrefab != null)
            {
                Instantiate(spawnPrefab, hit.point, rot);
                AndsonSoundSystem.instance.PlaySound("explode");
            }
            else
            {
                Debug.LogWarning("spawnPrefab 没有在 Inspector 里赋值！");
            }
        }
    }
}
