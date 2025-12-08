using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartSceneManager : MonoBehaviour
{
    [Header("Objects")]
    public GameObject[] images;
    public GameObject title;
    public GameObject startButton;
    public string[] randomScenes;

    [Header("Timings")]
    public float fadeDuration = 0.5f;
    public float stayDuration = 0.7f;
    public float titleDelay = 0.3f;

    [Header("Random Position & Scale")]
    public Vector2 randomXRange = new Vector2(-400f, 400f);
    public Vector2 randomYRange = new Vector2(-200f, 200f);
    public Vector2 randomScaleRange = new Vector2(1f, 5f);

    void Start()
    {
        if (title != null) title.SetActive(false);
        if (startButton != null) startButton.SetActive(false);

        foreach (GameObject logo in images)
        {
            if (logo != null)
                logo.SetActive(false);
        }

        StartCoroutine(PlayIntro());
    }

    IEnumerator PlayIntro()
    {
        foreach (GameObject logo in images)
        {
            if (logo == null) continue;

            logo.SetActive(true);

            RectTransform rt = logo.GetComponent<RectTransform>();

            float randomX = Random.Range(randomXRange.x, randomXRange.y);
            float randomY = Random.Range(randomYRange.x, randomYRange.y);
            rt.anchoredPosition = new Vector2(randomX, randomY);

            float randomScale = Random.Range(randomScaleRange.x, randomScaleRange.y);
            rt.localScale = new Vector3(randomScale, randomScale, 1f);

            CanvasGroup cg = logo.GetComponent<CanvasGroup>();
            if (cg == null)
                cg = logo.AddComponent<CanvasGroup>();

            cg.alpha = 0f;

            if (cg == null)
                cg = logo.AddComponent<CanvasGroup>();

            cg.alpha = 0f;

            float t = 0f;
            while (t < fadeDuration)
            {
                t += Time.deltaTime;
                cg.alpha = Mathf.Clamp01(t / fadeDuration);
                yield return null;
            }

            yield return new WaitForSeconds(stayDuration);

            t = 0f;
            while (t < fadeDuration)
            {
                t += Time.deltaTime;
                cg.alpha = 1f - Mathf.Clamp01(t / fadeDuration);
                yield return null;
            }

            cg.alpha = 0f;
            logo.SetActive(false);
        }

        yield return new WaitForSeconds(titleDelay);

        if (title != null)
            title.SetActive(true);

        if (startButton != null)
            startButton.SetActive(true);
    }

    public void StartGame()
    {
        if (randomScenes.Length == 0)
        {
            return;
        }

        GameStartFlag.isNewGame = true;
        int index = Random.Range(0, randomScenes.Length);
        SceneManager.LoadScene(randomScenes[index]);
    }
}
