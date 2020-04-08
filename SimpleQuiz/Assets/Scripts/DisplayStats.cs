using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DisplayStats : MonoBehaviour
{
    public Text FinalScore;
    public Text HighScore;
    public Text HasHighscore;
    public Text SuccessPercentage;


    // Start is called before the first frame update
    void Awake()
    {
        FinalScore.text = PlayerPrefmanager.GetScore().ToString();
        HighScore.text = PlayerPrefmanager.GetHighScore().ToString();
        if (!GameManager.gm.hasNewHighscore)
        {
            HasHighscore.gameObject.SetActive(false);
        }

        float percentage = (float)(GameManager.gm.totalCorrectAnswers / (float)GameManager.gm.totalQuestions) * 100;
        SuccessPercentage.text = percentage.ToString() + "%";
        Debug.Log(percentage);
    }   
    
    public void PlayAgain()
    {
        SceneManager.LoadScene(1);
    }
}
