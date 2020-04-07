using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DisplayStats : MonoBehaviour
{
    public Text FinalScore;
    public Text HighScore;

    // Start is called before the first frame update
    void Awake()
    {
        FinalScore.text = PlayerPrefmanager.GetScore().ToString();
        HighScore.text = PlayerPrefmanager.GetHighScore().ToString();
    }   
    
    public void PlayAgain()
    {
        SceneManager.LoadScene(1);
    }
}
