using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager gm;

    // Inspector properties

    [Header("Game Settings")]
    public int TimeForAnswer;

    [Header("Κατηγορία: Θρησκεία")]
    public Question[] ReligionQuestions;

    [Header("Κατηγορία: Πολιτισμός")]
    public Question[] CultureQuestions;

    [Header("Κατηγορία: Φύση")]
    public Question[] NatureQuestions;

    [Header("Κατηγορία: COVID-19")]
    public Question[] CovidQuestions;


    #region private fields
    
    // we will copy to this list the chosen by the user category questions
    List<Question> selectedQuestions;

    // keep track of the current question
    Question currentQuestion;

    // score
    int totalCorrectAnswers;
    int totalsQuestions;
    int highScore;

    #endregion

    private void Awake()
    {
        if (gm == null)
        {
            gm = GetComponent<GameManager>();
        }

        // reset score
        PlayerPrefmanager.ResetStats();
        highScore = PlayerPrefmanager.GetHighScore();

        // Check which category is selected from MenuManager
        // and copy from our predifined questions arrays in the inspector
        // to a new list, so we can keep track which questions have already been answered
        switch (MenuManager.menu.selectedCategory)
        {
            case "Religion":
                if (selectedQuestions == null || selectedQuestions.Count == 0)
                {
                    selectedQuestions = ReligionQuestions.ToList();
                }
                break;
            case "Culture":
                if (selectedQuestions == null || selectedQuestions.Count == 0)
                {
                    selectedQuestions = CultureQuestions.ToList();
                }
                break;
            case "Nature":
                if (selectedQuestions == null || selectedQuestions.Count == 0)
                {
                    selectedQuestions = NatureQuestions.ToList();
                }
                break;
            case "COVID-19":
                if (selectedQuestions == null || selectedQuestions.Count == 0)
                {
                    selectedQuestions = CovidQuestions.ToList();
                }
                break;

            default:
                break;
        }                
    }

    // load the first question when the game starts
    private void Start()
    {
        SelectRandomQuestion();
    }

    // first check if there are more questions or all have been answered
    // select a random question from the chosen category
    // update GUI to show the question text and the four possible answers to the user and reenable buttons
    // else load game over screen
    void SelectRandomQuestion()
    {
        if(selectedQuestions.Count != 0)
        {
            int randomIndex = Random.Range(0, selectedQuestions.Count);
            currentQuestion = selectedQuestions[randomIndex];

            GuiManager.gui.QuestionText.text = currentQuestion.Text;
            for (int i = 0; i < GuiManager.gui.Answers.Length; i++)
            {
                GuiManager.gui.Answers[i].GetComponentInChildren<Text>().text = currentQuestion.Answers[i].text;
                GuiManager.gui.Answers[i].interactable = true;
            }           
        }
        else
        {
            // load game over screen
            SceneManager.LoadScene(3);
        }        
    }
    
    // check if the button pressed contains the correct answer
    // change the buttons' colors (red for wrong, green for correct answer) based on the result
    // play a random sound effect
    // update score and GUI
    // start a coroutin to go load the next question
    public void CheckAnswer()
    {
        int correctIndex = -1;

        string name = EventSystem.current.currentSelectedGameObject.name;

        // find correct answer
        for (int i = 0; i < currentQuestion.Answers.Length; i++)
        {
            if (currentQuestion.Answers[i].isCorrect)
            {
                correctIndex = i;
            }
        }

        // check user's answer
        if(name == correctIndex.ToString())
        {
            GuiManager.gui.Answers[correctIndex].GetComponent<Image>().color = Color.green;

            // play random effect
            int randomIndex = Random.Range(0, SoundManager.sm.CorrectAnswers.Length);

            SoundManager.sm.audioController.PlayOneShot(SoundManager.sm.CorrectAnswers[randomIndex]);

            totalCorrectAnswers++;
            if(totalCorrectAnswers >= highScore)
            {
                highScore = totalCorrectAnswers;
                GuiManager.gui.TopScore.text = highScore.ToString();
                PlayerPrefmanager.SetHighScore(highScore);
            }

            PlayerPrefmanager.SetScore(totalCorrectAnswers);
        }

        else
        {
            GuiManager.gui.Answers[int.Parse(name)].GetComponent<Image>().color = Color.red;

            // play random effect
            int randomIndex = Random.Range(0, SoundManager.sm.WrongAnswers.Length);
            SoundManager.sm.audioController.PlayOneShot(SoundManager.sm.WrongAnswers[randomIndex]);
        }
        totalsQuestions++;

        // update score in GUI
        GuiManager.gui.CorrectAnswersScore.text = totalCorrectAnswers.ToString();
        GuiManager.gui.TotalAnswersScore.text = "/" + totalsQuestions.ToString();

        StartCoroutine(GotoNextQuestion());
    }

    // Remove the current question from the question list
    // disable buttons interactivity
    // wait for seconds for the user to see the results
    // reset buttons' colors back to normal
    // finally, load the next question
    IEnumerator GotoNextQuestion()
    {
        selectedQuestions.Remove(currentQuestion);

        foreach (var button in GuiManager.gui.Answers)
        {
            button.interactable = false;
        }        

        yield return new WaitForSeconds(2f);
        GuiManager.gui.ResetButtonColors();
        SelectRandomQuestion();
    }
}
