using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GuiManager : MonoBehaviour
{
    public static GuiManager gui;

    // Inspector
    public Text QuestionText;
    public Text CorrectAnswersScore;
    public Text TotalAnswersScore;
    public Text TopScore;
    public Text TimeText;

    public Button[] Answers;   

    private void Awake()
    {
        if(gui == null)
        {
            gui = GetComponent<GuiManager>();
        }

        // initialize score values
        CorrectAnswersScore.text = "0";
        TotalAnswersScore.text = "/0";
        TopScore.text = PlayerPrefmanager.GetHighScore().ToString();
    }

    public void ResetButtonColors()
    {
        foreach (var button in Answers)
        {
            button.GetComponent<Image>().color = new Color32(255, 172, 174, 255);
        }
    }
}
