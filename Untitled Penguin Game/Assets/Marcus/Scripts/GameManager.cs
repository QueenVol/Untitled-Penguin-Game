using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public static bool isGameWon = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        isGameWon = false;
        
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void WinGame()
    {
        if (!isGameWon)
        {
            isGameWon = true;
            Debug.Log("Game Won!");
            StartCoroutine(ResetGameRoutine());
        }
    }

    private IEnumerator ResetGameRoutine()
    {
        Debug.Log("Resetting game in 1 seconds...");
        yield return new WaitForSeconds(1f);

        // Reset the load
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        Debug.Log("Save data cleared.");


        ThirdPlayerShooter thirdPlayerShooter = FindObjectOfType<ThirdPlayerShooter>();
        if (thirdPlayerShooter != null)
        {
            thirdPlayerShooter.LoadRandomSceneBasedOnBools();
        }
    }
}
