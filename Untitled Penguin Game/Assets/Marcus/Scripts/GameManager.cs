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

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void WinGame()
    {
        if (!isGameWon)
        {
            isGameWon = true;
            Debug.Log("Game Won!");
            // You can add more win logic here (e.g., show UI, load scene)
            StartCoroutine(ResetGameRoutine());
        }
    }

    private IEnumerator ResetGameRoutine()
    {
        Debug.Log("Resetting game in 5 seconds...");
        yield return new WaitForSeconds(5f);

        // Reset the load
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        Debug.Log("Save data cleared.");

        // Reset game state
        isGameWon = false;

        // Start from the start (Scene 0)
        SceneManager.LoadScene(0);
    }
}
