using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;

public class QuizManager : MonoBehaviour
{
    public string csvFilePath = "Assets/Mogura/quiz.csv";
    public QuizUIManager quizUIManager;

    private List<QuizQuestion> questions = new List<QuizQuestion>();
    private List<QuizQuestion> unansweredQuestions;
    private List<QuizQuestion> answeredQuestions = new List<QuizQuestion>(); // 解答済みの問題リスト
    private QuizQuestion currentQuestion;

    private int totalQuestions = 5; // 出題する総問題数
    private bool canAnswer = false; // 答えることができるかどうかのフラグ

    void Start()
    {
        LoadQuestionsFromCSV();
        NextQuestion();
        canAnswer = true; // 最初の問題が表示されたら答えることができるようにする
        Debug.Log("Answered questions: ");
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

            // 問題をUIに表示する処理を呼び出す
            quizUIManager.DisplayQuestion(currentQuestion);
            canAnswer = true; // 解答可能にする
        }
        else
        {
            // 未解答問題がなくなった場合はクイズ終了
            Debug.Log("All questions answered. Game over.");
        }
    }

    public void CheckAnswer(int selectedChoiceIndex)
    {
        canAnswer = false; // 解答中は新しい選択ができないようにする

        if (selectedChoiceIndex == currentQuestion.CorrectChoiceIndex)
        {
            Debug.Log("Correct!");
            answeredQuestions.Add(currentQuestion); // 解答済みリストに追加する
            unansweredQuestions.Remove(currentQuestion); // 未解答リストから削除する
            if (answeredQuestions.Count < totalQuestions)
            {
                NextQuestion(); // 次の問題を出題する
            }
            else
            {
                Debug.Log("All questions answered correctly. Game over.");
            }
        }
        else
        {
            Debug.Log("Incorrect!");
            // 不正解時の処理を実装する（例えば別の問題を挟んで再度出題する）
            NextQuestion();
        }
    }
}
