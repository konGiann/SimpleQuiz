using System;
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

    #region Inspector properties

    [Header("Game Settings")]
    public int TimeForAnswer;
    public float NextQuestionDelay;

    [Header("Κατηγορία: Θρησκεία")]
    public Question[] ReligionQuestions;

    [Header("Κατηγορία: Πολιτισμός")]
    public Question[] CultureQuestions;

    [Header("Κατηγορία: Φύση")]
    public Question[] NatureQuestions;

    [Header("Κατηγορία: COVID-19")]
    public Question[] CovidQuestions;

    [Header("Default Εικόνες κατηγοριών")]
    public Sprite ReligionImage;
    public Sprite CultureImage;
    public Sprite NatureImage;
    public Sprite CovidImage;

    #endregion

    #region private fields

    // we will copy to this list the chosen by the user category questions
    List<Question> selectedQuestions;

    // keep track of the current question
    Question currentQuestion;

    // score
    int totalCorrectAnswers;
    int totalQuestions;
    int highScore;

    float timer;
    bool isQuestionAnswered;

    #endregion

    private void Awake()
    {
        if (gm == null)
        {
            gm = GetComponent<GameManager>();
        }

        // set initial timer
        timer = TimeForAnswer;

        // reset score and set highscore
        PlayerPrefmanager.ResetStats();
        highScore = PlayerPrefmanager.GetHighScore();

        CheckCategoryOfQuestions();
    }

    private void Update()
    {
        AnswerCountDown();
    }

    // Check which category is selected from MenuManager
    // and copy from our predifined questions arrays in the inspector
    // to a new list, so we can keep track which questions have already been answered
    private void CheckCategoryOfQuestions()
    {
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

    // must answer in this time span
    // else go to next question and update score
    private void AnswerCountDown()
    {
        if (!isQuestionAnswered)
        {
            timer -= Time.deltaTime;
        }
        int displayAsInt = (int)timer;
        GuiManager.gui.TimeText.text = displayAsInt.ToString();
        if (displayAsInt == 0)
        {
            LostDueToTimer();
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
        if (selectedQuestions.Count != 0)
        {
            int randomIndex = UnityEngine.Random.Range(0, selectedQuestions.Count);
            currentQuestion = selectedQuestions[randomIndex];

            GuiManager.gui.QuestionText.text = currentQuestion.Text;

            if(currentQuestion.Image == null)
            {
                GuiManager.gui.QuestionImage.sprite = ReligionImage;
            }
            else
            {
                GuiManager.gui.QuestionImage.sprite = currentQuestion.Image;
            }

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
        if (name == correctIndex.ToString())
        {
            GuiManager.gui.Answers[correctIndex].GetComponent<Image>().color = Color.green;

            // play random effect
            int randomIndex = UnityEngine.Random.Range(0, SoundManager.sm.CorrectAnswers.Length);

            SoundManager.sm.audioController.PlayOneShot(SoundManager.sm.CorrectAnswers[randomIndex]);

            totalCorrectAnswers++;
            if (totalCorrectAnswers >= highScore)
            {
                highScore = totalCorrectAnswers;
                GuiManager.gui.TopScore.text = highScore.ToString();
                PlayerPrefmanager.SetHighScore(highScore);
            }

            PlayerPrefmanager.SetScore(totalCorrectAnswers);
        } // correct

        else
        {
            GuiManager.gui.Answers[int.Parse(name)].GetComponent<Image>().color = Color.red;

            // play random effect
            int randomIndex = UnityEngine.Random.Range(0, SoundManager.sm.WrongAnswers.Length);
            SoundManager.sm.audioController.PlayOneShot(SoundManager.sm.WrongAnswers[randomIndex]);
        } // wrong
        totalQuestions++;

        UpdateGUIScore();

        StartCoroutine(GotoNextQuestionWithDelay(NextQuestionDelay));
    }

    // send score values to gui
    private void UpdateGUIScore()
    {
        GuiManager.gui.CorrectAnswersScore.text = totalCorrectAnswers.ToString();
        GuiManager.gui.TotalAnswersScore.text = "/" + totalQuestions.ToString();
    }

    // call this when timer is off
    // load next question whithout any delay
    // update score and reset timer
    private void LostDueToTimer()
    {
        timer = TimeForAnswer;
        selectedQuestions.Remove(currentQuestion);
        int randomIndex = UnityEngine.Random.Range(0, SoundManager.sm.WrongAnswers.Length);
        SoundManager.sm.audioController.PlayOneShot(SoundManager.sm.WrongAnswers[randomIndex]);
        GuiManager.gui.ResetButtonColors();
        totalQuestions++;
        UpdateGUIScore();
        SelectRandomQuestion();
    }

    // Remove the current question from the question list
    // disable buttons interactivity
    // wait for seconds for the user to see the results
    // reset buttons' colors back to normal
    // finally, load the next question
    IEnumerator GotoNextQuestionWithDelay(float delay)
    {
        isQuestionAnswered = true;
        selectedQuestions.Remove(currentQuestion);

        foreach (var button in GuiManager.gui.Answers)
        {
            button.interactable = false;
        }

        yield return new WaitForSeconds(delay);

        isQuestionAnswered = false;
        timer = TimeForAnswer;
        GuiManager.gui.ResetButtonColors();
        SelectRandomQuestion();
    }
}
