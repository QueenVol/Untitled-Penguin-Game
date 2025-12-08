using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitlePop : MonoBehaviour
{
    public float popTime = 0.25f;
    public float overshootScale = 1.2f;

    void OnEnable()
    {
        StartCoroutine(Pop());
    }

    IEnumerator Pop()
    {
        transform.localScale = Vector3.zero;

        float t = 0f;
        while (t < popTime)
        {
            t += Time.deltaTime;
            float progress = Mathf.Clamp01(t / popTime);
            float scale = Mathf.Lerp(0f, overshootScale, progress);
            transform.localScale = new Vector3(scale, scale, 1f);
            yield return null;
        }

        t = 0f;
        float settleTime = popTime * 0.7f;
        while (t < settleTime)
        {
            t += Time.deltaTime;
            float progress = Mathf.Clamp01(t / settleTime);
            float scale = Mathf.Lerp(overshootScale, 1f, progress);
            transform.localScale = new Vector3(scale, scale, 1f);
            yield return null;
        }

        transform.localScale = Vector3.one;
    }
}
