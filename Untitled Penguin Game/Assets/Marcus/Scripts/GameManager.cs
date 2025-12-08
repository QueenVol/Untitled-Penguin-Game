using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public bool isGameWon = false;

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
        }
    }
}

