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
    private List<QuizQuestion> answeredQuestions = new List<QuizQuestion>(); // �𓚍ς݂̖�胊�X�g
    private QuizQuestion currentQuestion;

    private int totalQuestions = 5; // �o�肷�鑍��萔
    private bool canAnswer = false; // �����邱�Ƃ��ł��邩�ǂ����̃t���O

    void Start()
    {
        LoadQuestionsFromCSV();
        NextQuestion();
        canAnswer = true; // �ŏ��̖�肪�\�����ꂽ�瓚���邱�Ƃ��ł���悤�ɂ���
        Debug.Log("Answered questions: ");
    }

    void Update()
    {
        if (canAnswer)
        {
            // �X�y�[�X�L�[�őI�����m�肷��
            if (Input.GetKeyDown(KeyCode.Space))
            {
                int selectedChoiceIndex = quizUIManager.GetSelectedChoiceIndex();
                CheckAnswer(selectedChoiceIndex);
            }

            // ���E�L�[�őI������؂�ւ���
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
            // ���������_���ɑI��
            int randomIndex = Random.Range(0, unansweredQuestions.Count);
            currentQuestion = unansweredQuestions[randomIndex];

            // ����UI�ɕ\�����鏈�����Ăяo��
            quizUIManager.DisplayQuestion(currentQuestion);
            canAnswer = true; // �𓚉\�ɂ���
        }
        else
        {
            // ���𓚖�肪�Ȃ��Ȃ����ꍇ�̓N�C�Y�I��
            Debug.Log("All questions answered. Game over.");
        }
    }

    public void CheckAnswer(int selectedChoiceIndex)
    {
        canAnswer = false; // �𓚒��͐V�����I�����ł��Ȃ��悤�ɂ���

        if (selectedChoiceIndex == currentQuestion.CorrectChoiceIndex)
        {
            Debug.Log("Correct!");
            answeredQuestions.Add(currentQuestion); // �𓚍ς݃��X�g�ɒǉ�����
            unansweredQuestions.Remove(currentQuestion); // ���𓚃��X�g����폜����
            if (answeredQuestions.Count < totalQuestions)
            {
                NextQuestion(); // ���̖����o�肷��
            }
            else
            {
                Debug.Log("All questions answered correctly. Game over.");
            }
        }
        else
        {
            Debug.Log("Incorrect!");
            // �s�������̏�������������i�Ⴆ�Εʂ̖�������ōēx�o�肷��j
            NextQuestion();
        }
    }
}
