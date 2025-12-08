using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EggSpawner : MonoBehaviour
{
    [Header("UI Egg Disturb")]
    public GameObject disturbPrefab;
    public Canvas canvas;

    [Header("Camera Shake Disturb")]
    public Camera cameraToShake;
    public float baseShakeMagnitude = 0.05f;
    public float baseShakeDuration = 0.1f;

    [Header("Fade-in Disturb")]
    public Image fadeImage;
    private float fadeAlpha = 0f;

    [Header("General Settings")]
    public int maxCount = 18;
    public string nextSceneName;

    public int currentCount = 0;
    private bool keyPreviouslyDown = false;

    private int finalDisturbMode;
    private int andsonFinalDisturbMode;
    private int marcusFinalDisturbMode;
    private int nextRandomScene;

    public AndsonCameraFollow cameraFollow;

    void Start()
    {
        finalDisturbMode = Random.Range(0, 4);
        andsonFinalDisturbMode = Random.Range(0, 5);
        marcusFinalDisturbMode = Random.Range(0, 5);
        Debug.Log("Selected Disturb Mode = " + finalDisturbMode);

        if (fadeImage != null)
        {
            Color c = fadeImage.color;
            c.a = 0f;
            fadeImage.color = c;
        }
    }

    void Update()
    {
        if (SceneManager.GetActiveScene().name == "Andy")
        {
            if (Input.anyKeyDown || Input.GetMouseButtonDown(0))
            {
                TriggerDisturb();
            }
        }
        else if (SceneManager.GetActiveScene().name == "AndsonScene")
        {
            if (Input.anyKeyDown || Input.GetMouseButtonDown(0))
            {
                AndsonTriggerDisturb();
            }
        }

        else if (SceneManager.GetActiveScene().name == "KevinMainScene")
        {
            if (Input.anyKeyDown || Input.GetMouseButtonDown(0))
            {
                YaTriggerDisturb();
            }
        }else if (SceneManager.GetActiveScene().name == "Playground 1")
        {
            if (Input.anyKeyDown || Input.GetMouseButtonDown(0))
            {
                MarcusTriggerDisturb();
            }
        }
    }

    void MarcusTriggerDisturb()
    {
        currentCount++;
        if (marcusFinalDisturbMode == 0)
            SpawnRandomUI();
        else if (marcusFinalDisturbMode == 1)
            StartCoroutine(CameraShake());
        else if (marcusFinalDisturbMode == 2)
            IncreaseFadeImage();
        else if (marcusFinalDisturbMode == 3)
            IncreaseGameSpeed();
        else if (marcusFinalDisturbMode == 4)
            IncreaseMouseSensitivity();

        if (currentCount >= maxCount)
        {
            ThirdPlayerShooter thirdPlayerShooter = FindObjectOfType<ThirdPlayerShooter>();
            if (thirdPlayerShooter != null)
            {
                thirdPlayerShooter.disturbThresholdReached = true;
            }
        }
    }

    void TriggerDisturb()
    {
        currentCount++;

        if (finalDisturbMode == 0)
            SpawnRandomUI();
        else if (finalDisturbMode == 1)
            StartCoroutine(CameraShake());
        else if (finalDisturbMode == 2)
            IncreaseFadeImage();
        else if (finalDisturbMode == 3)
            IncreaseGameSpeed();
            
        if (currentCount >= maxCount)
        {
            SaveLoad.Instance.SavePlayer();

            Time.timeScale = 1;

            LoadRandomSceneBasedOnBools();
        }

    }

    public void LoadRandomSceneBasedOnBools()
    {
        List<string> scenePool = new List<string>();

        // 只要对应的 bool == false，就加入随机列表
        if (!KevinIsFinished.kevinIsFinished)
            scenePool.Add("KevinMainScene");

        if (!AndsonAcrossSceneSaver.AndsonHasFinished)
            scenePool.Add("AndsonScene");

        if (!GameManager.isGameWon)
            scenePool.Add("Playground 1");   // 举例（如果你有第三个场景的话）

        // 如果列表为空 → 全部完成
        if (scenePool.Count == 0)
        {
            if (!PlayerController.StupidAndyFinished)
            {
                SceneManager.LoadScene("Andy");
            }
            else
            {

            }
        }

        // 随机挑选
        int index = Random.Range(0, scenePool.Count);
        string targetScene = scenePool[index];

        Debug.Log("切换到 Scene：" + targetScene);
        SceneManager.LoadScene(targetScene);
    }

    public void YKYVersionLoadRandomSceneBasedOnBools()
    {
        List<string> scenePool = new List<string>();

        // 只要对应的 bool == false，就加入随机列表
        if (!PlayerController.StupidAndyFinished)
            scenePool.Add("Andy");

        if (!AndsonAcrossSceneSaver.AndsonHasFinished)
            scenePool.Add("AndsonScene");

        if (!GameManager.isGameWon)
            scenePool.Add("Playground 1");

        // 如果列表为空 → 全部完成
        if (scenePool.Count == 0)
        {
            if (!KevinIsFinished.kevinIsFinished)
            {
                SceneManager.LoadScene("KevinMainScene");
            }
            else
            {

            }
        }

        // 随机挑选
        int index = Random.Range(0, scenePool.Count);
        string targetScene = scenePool[index];

        Debug.Log("切换到 Scene：" + targetScene);
        SceneManager.LoadScene(targetScene);
    }


    void AndsonTriggerDisturb()
    {
        currentCount++;
        if (!StartScreenTexts.isPaused)
        {
            if (andsonFinalDisturbMode == 0)
                SpawnRandomUI();
            else if (andsonFinalDisturbMode == 1)
                cameraFollow.StartShake(currentCount, baseShakeMagnitude/5, baseShakeDuration/5);
            else if (andsonFinalDisturbMode == 2)
                IncreaseFadeImage();
            else if (andsonFinalDisturbMode == 3)
                IncreaseInputDelay();
            else
                IncreaseGameSpeed();

            if (currentCount >= maxCount)
            {
                //SceneManager.LoadScene(nextSceneName);
            }
        }

        
    }

    void YaTriggerDisturb()
    {
        currentCount++;

        if (finalDisturbMode == 0)
            SpawnRandomUI();
        else if (finalDisturbMode == 1)
            StartCoroutine(CameraShake());
        else if (finalDisturbMode == 2)
            IncreaseFadeImage();
        else if (finalDisturbMode == 3)
            IncreaseGameSpeed();
       

        if (currentCount >= maxCount)
        {
            Time.timeScale = 1;
            YKYVersionLoadRandomSceneBasedOnBools();
        }


    }

    void IncreaseGameSpeed()
    {
         Time.timeScale += 0.1F;

    }

    void IncreaseInputDelay()
    {
        if (SceneManager.GetActiveScene().name == "AndsonScene")
        {
            AndsonPlayerMovement.inputDelay += 0.015f;
        }
    }

    void IncreaseMouseSensitivity()
    {
        // Assuming ThirdPlayerShooter has a static instance or we find it
        ThirdPlayerShooter shooter = FindObjectOfType<ThirdPlayerShooter>();
        if (shooter != null)
        {
            shooter.IncreaseSensitivity(3f);
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
    IEnumerator CameraShake()
    {
        float magnitude = baseShakeMagnitude * (1 + currentCount * 0.5f);
        float duration = baseShakeDuration + currentCount * 0.05f;

        float timer = 0;

        Vector3 originalPos = cameraToShake.transform.localPosition;

        while (timer < duration)
        {
            Vector2 dir = new Vector2(1, 1).normalized;
            Vector2 offset = dir * (Random.Range(-1f, 1f) * magnitude);

            cameraToShake.transform.localPosition = originalPos + (Vector3)offset;

            timer += Time.deltaTime;
            yield return null;
        }

        cameraToShake.transform.localPosition = originalPos;
    }

  

    void IncreaseFadeImage()
    {
        if (fadeImage == null) return;
        if (maxCount == 0) return;   // ????? 0

        // ??? 0~1 ?????
        float alpha = (float)currentCount / maxCount;
        alpha = Mathf.Clamp01(alpha);

        Color c = fadeImage.color;
        c.a = alpha;
        fadeImage.color = c;
    }
}
