using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StartScreenTexts : MonoBehaviour
{
    public List<string> startScreenWordsList;
    public TextMeshProUGUI startScreenText;
    private int currentTextIndex = 0;
    public GameObject startScreen;
    
    // Start is called before the first frame update
    void Start()
    {
        startScreenText.text = startScreenWordsList[currentTextIndex];
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
        }
        

    }
}
