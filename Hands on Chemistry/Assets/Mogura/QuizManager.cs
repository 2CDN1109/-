using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.IO;
using UnityEngine.SceneManagement;

public class QuizManager : MonoBehaviour
{
    public string csvFilePath = "Assets/Mogura/quiz.csv";
    public QuizUIManager quizUIManager;
    public JudgeMovieCtrl judgeMovieCtrl;

    public TMP_Text RestQuestion;

    private List<QuizQuestion> questions = new List<QuizQuestion>();
    private List<QuizQuestion> unansweredQuestions;
    private List<QuizQuestion> answeredQuestions = new List<QuizQuestion>(); // 解答済みの問題リスト
    private QuizQuestion currentQuestion;

//    private int totalQuestions = 8; // 出題する総問題数
    private bool canAnswer = false; // 答えることができるかどうかのフラグ
    public GameObject Result;       // リザルト画面の表示

    void Start()
    {
        LoadQuestionsFromCSV();
        Result.SetActive(false);    // 結果発表を非表示
        NextQuestion();
        canAnswer = true; // 最初の問題が表示されたら答えることができるようにする
    }

    void Update()
    {
        if (canAnswer)
        {
            // スペースキーで選択を確定する
            if (Input.GetKeyDown(KeyCode.Space))
            {
                int selectedChoiceIndex = quizUIManager.GetSelectedChoiceIndex();
                CheckAnswer(selectedChoiceIndex);
            }

            // 左右キーで選択肢を切り替える
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                quizUIManager.SelectPreviousChoice();
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                quizUIManager.SelectNextChoice();
            }
            else if (Input.GetKey(KeyCode.S) && Input.GetKeyDown(KeyCode.P))
            {
                GameClear();
            }
        }
    }

    void LoadQuestionsFromCSV()
    {
        if (File.Exists(csvFilePath))
        {
            string[] lines = File.ReadAllLines(csvFilePath);
            for (int i = 1; i < lines.Length; i++)
            {
                string[] data = lines[i].Split(',');
                if (data.Length >= 9)
                {
                    QuizQuestion question = new QuizQuestion();
                    question.QuestionNumber = int.Parse(data[0]);
                    question.QuestionText = data[1];
                    question.Choice1 = data[2];
                    question.Choice2 = data[3];
                    question.Choice3 = data[4];
                    question.CorrectChoiceIndex = int.Parse(data[5]) - 1; // 0-based index
                    question.Choice1ImagePath = data[6].Trim();
                    question.Choice2ImagePath = data[7].Trim();
                    question.Choice3ImagePath = data[8].Trim();
                    questions.Add(question);
                }
                else
                {
                    Debug.LogError("Invalid data format in CSV line: " + lines[i]);
                }
            }
            unansweredQuestions = new List<QuizQuestion>(questions);
        }
        else
        {
            Debug.LogError("CSV file not found at path: " + csvFilePath);
        }
    }

    void NextQuestion()
    {
        if (unansweredQuestions.Count > 0)
        {
            // 問題をランダムに選ぶ
            int randomIndex = Random.Range(0, unansweredQuestions.Count);
            currentQuestion = unansweredQuestions[randomIndex];
            RestQuestion.text = "あと" + unansweredQuestions.Count + "つ！";

            // 問題をUIに表示する処理を呼び出す
            quizUIManager.DisplayQuestion(currentQuestion);
            canAnswer = true; // 解答可能にする
        }
        else
        {
            GameClear();
        }
    }

    public void CheckAnswer(int selectedChoiceIndex)
    {
        canAnswer = false; // 解答中は新しい選択ができないようにする

        Vector3 cursorPosition = quizUIManager.GetCursorPosition(); // カーソルの位置を取得

        if (selectedChoiceIndex == currentQuestion.CorrectChoiceIndex)
        {
            Debug.Log("Correct!");
            judgeMovieCtrl.JudgmentObj[0].transform.position = new Vector3(cursorPosition.x, judgeMovieCtrl.JudgmentObj[0].transform.position.y, judgeMovieCtrl.JudgmentObj[0].transform.position.z);
            judgeMovieCtrl.JudgmentObj[0].SetActive(true);
            judgeMovieCtrl.videoPlayers[0].Play();
            answeredQuestions.Add(currentQuestion); // 解答済みリストに追加する
            unansweredQuestions.Remove(currentQuestion); // 未解答リストから削除する
            Invoke("OnCorrectAnswer", 1.7f); // 1.0秒後にOnCorrectAnswerメソッドを呼び出す
        }
        else
        {
            Debug.Log("Incorrect!");
            judgeMovieCtrl.JudgmentObj[1].transform.position = new Vector3(cursorPosition.x, judgeMovieCtrl.JudgmentObj[1].transform.position.y, judgeMovieCtrl.JudgmentObj[1].transform.position.z);
            judgeMovieCtrl.JudgmentObj[1].SetActive(true);
            judgeMovieCtrl.videoPlayers[1].Play();
            Invoke("OnIncorrectAnswer", 1.7f); // 2.0秒後にOnIncorrectAnswerメソッドを呼び出す
        }
    }

    void OnCorrectAnswer()
    {
        NextQuestion();
        Invoke("judgeMovieCtrl.HideJudgment", 0.3f); // エフェクトを非表示にする
        canAnswer = true; // 次の質問が表示されたら解答可能にする
    }

    void OnIncorrectAnswer()
    {
        NextQuestion();
        Invoke("judgeMovieCtrl.HideJudgment", 0.3f); // エフェクトを非表示にする
        canAnswer = true; // 次の質問が表示されたら解答可能にする
    }

    void GameClear()
    {
        // 未解答問題がなくなった場合はクイズ終了
        Debug.Log("All questions answered. Game over.");
        Invoke("ShowResult", 0.5f); // 2.0秒後に ChangeScene メソッドを呼び出す
        Invoke("ShowLoading", 3.0f); // 2.0秒後に ChangeScene メソッドを呼び出す
        Invoke("ChangeScene", 6.8f); // 2.0秒後に ChangeScene メソッドを呼び出す
    }

    void ShowResult()
    {
        judgeMovieCtrl.JudgmentObj[2].SetActive(true);
        judgeMovieCtrl.videoPlayers[2].Play();
    }

    void ShowLoading()
    {
        judgeMovieCtrl.JudgmentObj[3].SetActive(true);
        judgeMovieCtrl.videoPlayers[3].Play();
    }

    void ChangeScene()
    {
        SceneManager.LoadScene("TimingGame");
    }
}