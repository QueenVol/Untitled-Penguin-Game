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

    void Update()
    {
        // 按下按键时才发射射线
        if (Input.GetKeyDown(fireKey))
        {
            FireRayFromCenter();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            player.transform.position = startPlace.position;
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
            }
            else
            {
                Debug.LogWarning("spawnPrefab 没有在 Inspector 里赋值！");
            }
        }
    }
}
