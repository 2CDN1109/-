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
    private List<QuizQuestion> answeredQuestions = new List<QuizQuestion>(); // �𓚍ς݂̖�胊�X�g
    private QuizQuestion currentQuestion;

//    private int totalQuestions = 8; // �o�肷�鑍��萔
    private bool canAnswer = false; // �����邱�Ƃ��ł��邩�ǂ����̃t���O
    public GameObject Result;       // ���U���g��ʂ̕\��

    void Start()
    {
        LoadQuestionsFromCSV();
        Result.SetActive(false);    // ���ʔ��\���\��
        NextQuestion();
        canAnswer = true; // �ŏ��̖�肪�\�����ꂽ�瓚���邱�Ƃ��ł���悤�ɂ���
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
            // ���������_���ɑI��
            int randomIndex = Random.Range(0, unansweredQuestions.Count);
            currentQuestion = unansweredQuestions[randomIndex];
            RestQuestion.text = "����" + unansweredQuestions.Count + "�I";

            // ����UI�ɕ\�����鏈�����Ăяo��
            quizUIManager.DisplayQuestion(currentQuestion);
            canAnswer = true; // �𓚉\�ɂ���
        }
        else
        {
            GameClear();
        }
    }

    public void CheckAnswer(int selectedChoiceIndex)
    {
        canAnswer = false; // �𓚒��͐V�����I�����ł��Ȃ��悤�ɂ���

        Vector3 cursorPosition = quizUIManager.GetCursorPosition(); // �J�[�\���̈ʒu���擾

        if (selectedChoiceIndex == currentQuestion.CorrectChoiceIndex)
        {
            Debug.Log("Correct!");
            judgeMovieCtrl.JudgmentObj[0].transform.position = new Vector3(cursorPosition.x, judgeMovieCtrl.JudgmentObj[0].transform.position.y, judgeMovieCtrl.JudgmentObj[0].transform.position.z);
            judgeMovieCtrl.JudgmentObj[0].SetActive(true);
            judgeMovieCtrl.videoPlayers[0].Play();
            answeredQuestions.Add(currentQuestion); // �𓚍ς݃��X�g�ɒǉ�����
            unansweredQuestions.Remove(currentQuestion); // ���𓚃��X�g����폜����
            Invoke("OnCorrectAnswer", 1.7f); // 1.0�b���OnCorrectAnswer���\�b�h���Ăяo��
        }
        else
        {
            Debug.Log("Incorrect!");
            judgeMovieCtrl.JudgmentObj[1].transform.position = new Vector3(cursorPosition.x, judgeMovieCtrl.JudgmentObj[1].transform.position.y, judgeMovieCtrl.JudgmentObj[1].transform.position.z);
            judgeMovieCtrl.JudgmentObj[1].SetActive(true);
            judgeMovieCtrl.videoPlayers[1].Play();
            Invoke("OnIncorrectAnswer", 1.7f); // 2.0�b���OnIncorrectAnswer���\�b�h���Ăяo��
        }
    }

    void OnCorrectAnswer()
    {
        NextQuestion();
        Invoke("judgeMovieCtrl.HideJudgment", 0.3f); // �G�t�F�N�g���\���ɂ���
        canAnswer = true; // ���̎��₪�\�����ꂽ��𓚉\�ɂ���
    }

    void OnIncorrectAnswer()
    {
        NextQuestion();
        Invoke("judgeMovieCtrl.HideJudgment", 0.3f); // �G�t�F�N�g���\���ɂ���
        canAnswer = true; // ���̎��₪�\�����ꂽ��𓚉\�ɂ���
    }

    void GameClear()
    {
        // ���𓚖�肪�Ȃ��Ȃ����ꍇ�̓N�C�Y�I��
        Debug.Log("All questions answered. Game over.");
        Invoke("ShowResult", 0.5f); // 2.0�b��� ChangeScene ���\�b�h���Ăяo��
        Invoke("ShowLoading", 3.0f); // 2.0�b��� ChangeScene ���\�b�h���Ăяo��
        Invoke("ChangeScene", 6.8f); // 2.0�b��� ChangeScene ���\�b�h���Ăяo��
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