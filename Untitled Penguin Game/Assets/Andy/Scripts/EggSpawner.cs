using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EggSpawner : MonoBehaviour
{
    public GameObject disturbPrefab;
    public Canvas canvas;
    public int maxCount = 18;
    public string nextSceneName;

    private int currentCount = 0;
    private bool keyPreviouslyDown = false;

    void Update()
    {
        bool keyDown =
            Input.GetKey(KeyCode.A) ||
            Input.GetKey(KeyCode.D) ||
            Input.GetKey(KeyCode.W) ||
            Input.GetKey(KeyCode.Space) ||
            Input.GetKey(KeyCode.LeftArrow) ||
            Input.GetKey(KeyCode.RightArrow);

        if (keyDown && !keyPreviouslyDown)
        {
            TriggerDisturb();
        }

        keyPreviouslyDown = keyDown;
    }

    void TriggerDisturb()
    {
        currentCount++;

        SpawnRandomUI();

        if (currentCount >= maxCount)
        {
            Debug.Log("ChangeScene");
            //SceneManager.LoadScene(nextSceneName);
        }
    }

    void SpawnRandomUI()
    {
        GameObject obj = Instantiate(disturbPrefab, canvas.transform);

        RectTransform r = obj.GetComponent<RectTransform>();

        r.anchoredPosition = new Vector2(
            Random.Range(-Screen.width * 0.5f, Screen.width * 0.5f),
            Random.Range(-Screen.height * 0.5f, Screen.height * 0.5f)
        );

        float size = Random.Range(50f, 120f);
        r.sizeDelta = new Vector2(size, size);

        r.localRotation = Quaternion.Euler(0, 0, Random.Range(0f, 360f));
    }
}
