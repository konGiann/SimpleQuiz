using UnityEngine;

public class PlayerPrefmanager : MonoBehaviour
{
    public static int GetScore()
    {
        if (PlayerPrefs.HasKey("Score"))
            return PlayerPrefs.GetInt("Score");

        else return 0;
    }

    public static void SetScore(int score)
    {
        PlayerPrefs.SetInt("Score", score);
    }

    public static int GetHighScore()
    {
        if (PlayerPrefs.HasKey("HighScore"))
            return PlayerPrefs.GetInt("HighScore");

        else return 0;
    }

    public static void SetHighScore(int highScore)
    {
        PlayerPrefs.SetInt("HighScore", highScore);
    }

    public static void ResetStats()
    {
        PlayerPrefs.SetInt("Score", 0);        
    }
}
