using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StartScreenTexts : MonoBehaviour
{
    public List<string> startScreenWordsList;
    public TextMeshProUGUI startScreenText;
    private static int currentTextIndex = 0;
    public GameObject startScreen;

    public static bool isPaused = true;

    private static bool hasRun = false;

    // Start is called before the first frame update
    void Start()
    {
        if (!hasRun)
        {
            startScreenText.text = startScreenWordsList[currentTextIndex];

            Time.timeScale = 0;

            startScreen.SetActive(true);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClickNextWord()
    {
        if (currentTextIndex < startScreenWordsList.Count - 1)
        {
            currentTextIndex += 1;
            startScreenText.text = startScreenWordsList[currentTextIndex];

        }
        else if (currentTextIndex >= startScreenWordsList.Count - 1)
        {
            startScreen.SetActive(false);
            isPaused = false;
            hasRun = true;
            Time.timeScale = 1;

        }


    }
}
