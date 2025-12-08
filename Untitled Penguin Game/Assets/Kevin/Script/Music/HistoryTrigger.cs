using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HistoryTrigger : MonoBehaviour
{
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            MusicManager.Instance.PlayHistoryMusic();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            MusicManager.Instance.PlayNormalMusic();
        }
    }
}
