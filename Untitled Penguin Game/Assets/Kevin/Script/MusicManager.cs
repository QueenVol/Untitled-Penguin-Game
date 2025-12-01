using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;

    public AudioSource musicSource;
    public AudioClip normalMusic;
    public AudioClip chaseMusic;

    void Awake()
    {
        Instance = this;
        PlayNormalMusic();
    }

    public void PlayNormalMusic()
    {
        if (musicSource.clip == normalMusic) return;

        musicSource.clip = normalMusic;
        musicSource.volume = 0.2f;
        musicSource.Play();
    }

    public void PlayChaseMusic()
    {
        if (musicSource.clip == chaseMusic) return;

        musicSource.clip = chaseMusic;
        musicSource.volume = 1f;
        musicSource.Play();
    }
}
