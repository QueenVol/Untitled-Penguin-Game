using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameProgressUI : MonoBehaviour
{
    public static GameProgressUI Instance;

    public TextMeshProUGUI gameProressIndicator;

    private int finishedCount = 0;

    private bool lastKevin = false;
    private bool lastAndy = false;
    private bool lastGameWon = false;
    private bool lastAndson = false;




    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);  
        }
        else
        {
            Destroy(gameObject);            // 防止在别的场景又多出一份 UI
        }
    }

    void Update()
    {
        CheckBoolChange(ref lastKevin, KevinIsFinished.kevinIsFinished);
        CheckBoolChange(ref lastAndy, PlayerController.StupidAndyFinished);
        CheckBoolChange(ref lastGameWon, GameManager.isGameWon);
        CheckBoolChange(ref lastAndson, AndsonAcrossSceneSaver.AndsonHasFinished);

        gameProressIndicator.text = $"{finishedCount}/4 To End";
    }

    void CheckBoolChange(ref bool lastValue, bool currentValue)
    {
        // 只有从 false → true 才加一
        if (!lastValue && currentValue)
        {
            finishedCount++;
            Debug.Log("增加一次 finishedCount，当前 = " + finishedCount);
        }

        // 更新记录
        lastValue = currentValue;
    }
}
