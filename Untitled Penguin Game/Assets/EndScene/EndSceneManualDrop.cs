using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndSceneManualDrop : MonoBehaviour
{
    [Header("Image Parent")]
    public Transform imageParent;

    [Header("Grid Settings")]
    public int columns = 8;
    public float spacingX = 140f;
    public float spacingY = 140f;
    public float scale = 2f;

    [Header("Drop Settings")]
    public float startHeight = 900f;
    public float dropDuration = 0.5f;
    public float delayBetweenEach = 0.06f;

    [Header("End UI")]
    public GameObject endText;
    public GameObject restartButton;
    public GameObject endPanel;

    private List<RectTransform> images = new List<RectTransform>();
    private List<Vector2> targetPositions = new List<Vector2>();

    void Start()
    {
        endText.SetActive(false);
        restartButton.SetActive(false);
        endPanel.SetActive(false);

        CollectImages();
        AutoLayoutGrid();
        LiftImages();
        StartCoroutine(DropSequence());
    }

    void CollectImages()
    {
        images.Clear();

        foreach (Transform child in imageParent)
        {
            RectTransform rt = child.GetComponent<RectTransform>();
            if (rt != null)
                images.Add(rt);
        }
    }

    void AutoLayoutGrid()
    {
        targetPositions.Clear();

        int total = images.Count;
        int rows = Mathf.CeilToInt((float)total / columns);

        float totalWidth = (columns - 1) * spacingX;
        float totalHeight = (rows - 1) * spacingY;

        Vector2 startOffset = new Vector2(
            -totalWidth * 0.5f,
             totalHeight * 0.5f
        );

        int row = 0;
        int col = 0;

        for (int i = 0; i < images.Count; i++)
        {
            float x = startOffset.x + col * spacingX;
            float y = startOffset.y - row * spacingY;

            Vector2 finalPos = new Vector2(x, y);

            images[i].anchoredPosition = finalPos;
            images[i].localScale = Vector3.one * scale;
            targetPositions.Add(finalPos);

            col++;
            if (col >= columns)
            {
                col = 0;
                row++;
            }
        }
    }

    void LiftImages()
    {
        for (int i = 0; i < images.Count; i++)
        {
            images[i].anchoredPosition += Vector2.up * startHeight;
        }
    }

    IEnumerator DropSequence()
    {
        for (int i = 0; i < images.Count; i++)
        {
            StartCoroutine(DropOne(images[i], targetPositions[i]));
            yield return new WaitForSeconds(delayBetweenEach);
        }

        yield return new WaitForSeconds(dropDuration + 0.3f);
        endText.SetActive(true);
        restartButton.SetActive(true);
        endPanel.SetActive(true);
    }

    IEnumerator DropOne(RectTransform img, Vector2 targetPos)
    {
        Vector2 startPos = img.anchoredPosition;
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime / dropDuration;
            float ease = 1f - Mathf.Pow(1f - t, 3f);

            img.anchoredPosition = Vector2.Lerp(startPos, targetPos, ease);
            yield return null;
        }

        img.anchoredPosition = targetPos;
    }

    public void RestartGame()
    {
        GameStartFlag.isNewGame = true;
        SceneManager.LoadScene(0);
    }
}