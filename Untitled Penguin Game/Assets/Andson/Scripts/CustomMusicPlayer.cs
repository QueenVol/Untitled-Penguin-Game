using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomMusicPlayer : MonoBehaviour
{
    private float randomPlayTime;


    private void Start()
    {
        StartNewSong();
    }

    private void Update()
    {
        randomPlayTime -= Time.deltaTime;

        if (randomPlayTime <= 0)
        {
            StartNewSong();
        }
    }

    private void StartNewSong()
    {
        AndsonSoundSystem.instance.StopAllSounds();
        randomPlayTime = Random.Range(20, 60);
        int randomer = Random.Range(1, 5);
        string randomerString = randomer.ToString();
        AndsonSoundSystem.instance.PlaySound(randomerString);
    }
}
