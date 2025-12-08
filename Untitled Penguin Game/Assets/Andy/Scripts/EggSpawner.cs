using System.Collections;
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

    private int currentCount = 0;
    private bool keyPreviouslyDown = false;

    private int finalDisturbMode;

    void Start()
    {
        finalDisturbMode = Random.Range(0, 3);
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

        if (finalDisturbMode == 0)
            SpawnRandomUI();
        else if (finalDisturbMode == 1)
            StartCoroutine(CameraShake());
        else
            IncreaseFadeImage();

        if (currentCount >= maxCount)
        {
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

        fadeAlpha += 14f / 255f;
        fadeAlpha = Mathf.Clamp01(fadeAlpha);

        Color c = fadeImage.color;
        c.a = fadeAlpha;
        fadeImage.color = c;
    }
}
