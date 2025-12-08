using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterAnimation : MonoBehaviour
{
    public string clipName; // 动画名字

    private Animator anim;


    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        AnimatorStateInfo info = anim.GetCurrentAnimatorStateInfo(0);

        // 如果当前播放的是 clipName，并且播放完成
        if (info.IsName(clipName) && info.normalizedTime >= 1f)
        {
            Destroy(gameObject);
        }
    }
}
