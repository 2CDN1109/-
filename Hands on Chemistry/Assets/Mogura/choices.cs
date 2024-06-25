using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class choices : MonoBehaviour
{
    private TextAsset csvFile;  // CSV�t�@�C��
    private List<string[]> csvData = new List<string[]>();  // CSV�t�@�C���̒��g�����郊�X�g
    int index = 0; //���ԍ�
    int correct; //����
    bool AnswerFlag;    //�𓚔���
    float timelimit = 10f; //��������
    float nowtimer = 0f;   //�o�ߎ���
    public TextMeshProUGUI ProblemLog;  //��蕶
    public TextMeshProUGUI ProblemNum;  //���ԍ�
    public TextMeshProUGUI CorrectNum;   //����
    public TextMeshProUGUI CorrectRate;  //������
    public TextMeshProUGUI time;    //�c�莞�ԕ\��
    public Button[] ChoiceBtns;   //�I����
    public Image[] ChoiceImages;  //�I�����̉摜
    private List<int> numbers = new List<int> { 1, 2, 3, 4 };   //�I�����ԍ�
    private List<int> Problem = new List<int> { }; //��������
    public GameObject[] JudgmentObj;   //����s�����̕\��-�I�u�W�F�N�g
    private VideoPlayer[] videoPlayers; //����s�����̕\��-�r�f�I
    public Slider TimeSlider;   //�c�莞�ԃQ�[�W
//    public Image Character;      //�摜
    public Sprite[] newCharacters;  //�i�[���Ă���摜��ω�������
    public GameObject Result;       //���U���g��ʂ̕\��
    public AudioSource BGM;     //BGM
    public AudioSource ClearSound;
    public AudioSource FailSound;


    public Image ImageCursor; // �J�[�\���Ƃ��Ďg���摜�i�g�j
    private int selectedIndex = 0;


    void Start()
    {

/*
        //BGM
        float Score = BGMsys.score;
        BGM.volume = Score;
        ClearSound.volume = Score;
        FailSound.volume = Score;
*/

        //CSV���[�h
        csvFile = Resources.Load("test") as TextAsset;
        StringReader reader = new StringReader(csvFile.text);
        while (reader.Peek() != -1)
        {
            string line = reader.ReadLine();
            csvData.Add(line.Split(','));
        }

        //CSV�̖������X�g�ɂ����
        Problem = new List<int>(csvData.Count);
        for (int i = 0; i < csvData.Count; i++)
        {
            Problem.Add(i);
        }
        Shuffle(Problem);  //�����V���b�t��
        Debug.Log("Problem: " + string.Join(", ", Problem.Select(x => x.ToString())));

        TimeSlider.value = 1f;  //�������ԃQ�[�W

        Result.SetActive(false);    //���ʔ��\���\��
        JudgmentObj[0].SetActive(false);    //�����A�j���[�V������\��
        JudgmentObj[1].SetActive(false);    //�s�����A�j���[�V������\��

        //�r�f�I��ۑ����Ă���I�u�W�F�N�g���r�f�I�ɂ���
        videoPlayers = new VideoPlayer[JudgmentObj.Length];
        for (int i = 0; i < JudgmentObj.Length; i++)
        {
            videoPlayers[i] = JudgmentObj[i].GetComponentInChildren<VideoPlayer>();
            videoPlayers[i].loopPointReached += OnVideoEnd;  //���[�v���Ȃ��悤��
        }
        // �ŏ��̖���\��
        ShowNextQuestion();
    }

    void Update()
    {
        //���Ԑ���
        nowtimer += Time.deltaTime; // �^�C�}�[
        float t = nowtimer / timelimit;// �X���C�_�[�̒l�[���K��
        TimeSlider.value = Mathf.Lerp(1f, 0f, t);
        float TimeLimit = 15f - nowtimer; //�c�莞��
        TimeLimit = Mathf.Max(TimeLimit, 0f);

        string LimitLog = TimeLimit.ToString("F0");
        time.text = LimitLog + "�b";
        time.color = (TimeLimit > 5.5f) ? Color.green : Color.red;// 5.5�b�ȏ�͗΁A5.5�b�����͐�

        // �^�C���I�[�o�[
        if (nowtimer >= timelimit)
        {
            nowtimer = 0f;     //�^�C�}�[��0�b�ɖ߂�
            JudgmentObj[1].SetActive(true); //�s������\��
            videoPlayers[1].Play(); //����𗬂�
            AnswerFlag = false;
            AnswerCheck();
            StartCoroutine(NextQuestion());
        }

/*
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            selectedIndex = (selectedIndex - 1 + images.Length) % images.Length;
            UpdateCursor();
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            selectedIndex = (selectedIndex + 1) % images.Length;
            UpdateCursor();
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            SelectImage();
        }
*/
    }

/*
    void UpdateCursor()
    {
        cursor.transform.position = images[selectedIndex].transform.position;
    }
*/



    IEnumerator NextQuestion()
    {
        yield return new WaitForSeconds(2.0f);  // 2.0�b�҂�

        ShowNextQuestion();
    }

    void ShowNextQuestion()
    {
//        Character.sprite = newCharacters[0];
        string No = (index + 1).ToString(); //���ԍ�
        ProblemNum.text = "��" + No + "��";

        // ���ƑI������\��
        if (Problem.Count > index)
        {
            if (index < 10) //10��A���ŏo��A0����n�߂Ă��邩�疢��
            {
                Quiz();
            }
            else
            {
                Result.SetActive(true);
                Time.timeScale = 0;
                BGM.Stop();
                Debug.Log("���I��");
                
            }
        }
        else if (Problem.Count == index)
        {

            Debug.Log("�I��!!!!");
            Result.SetActive(true);
            BGM.Stop();
            Invoke("ChangeScene", 2.0f); // 2.0�b��� HideJudgment ���\�b�h���Ăяo��

        }
    }

    void Quiz()
    {
        for (int i = 0; i < numbers.Count; i++)
        {
            int randomrow = Problem[index];
            int randomcol = numbers[i];
            ProblemLog.text = csvData[randomrow][0];

            // �{�^���̃e�L�X�g���\���ɂ��ĉ摜��\��

            string imagePath = csvData[randomrow][randomcol + 4];
            Sprite choiceSprite = Resources.Load<Sprite>(imagePath);
            ChoiceImages[i].sprite = choiceSprite;

            // ���łɃC�x���g���ǉ�����Ă���ꍇ�͈�U�������Ă���ǉ�
            ChoiceBtns[i].onClick.RemoveAllListeners();
            ChoiceBtns[i].onClick.AddListener(() => OnButtonClick(randomcol));
        }

        // �^�C�}�[�����Z�b�g
        ResetTimer();
        Shuffle(numbers);
    }


    void OnButtonClick(int selectedAnswer)
    {
        if (selectedAnswer == 1)
        {
            AnswerFlag = true;
        }
        else
        {
            AnswerFlag = false;
        }

        AnswerCheck();
        // ���̖���\��
        StartCoroutine(NextQuestion());
    }

    void AnswerCheck()
    {
        ResetTimer();
        if (AnswerFlag == true)//����
        {
            JudgmentObj[0].SetActive(true);
            videoPlayers[0].Play();
//            Character.sprite = newCharacters[1];
//            JudgmentObj[0].GetComponentInChildren<TextMeshProUGUI>().color = Color.red;
//            JudgmentObj[0].GetComponentInChildren<TextMeshProUGUI>().text = ("�Z");
            correct += 1;   //����

            Invoke("HideJudgment", 2.0f); // 2.0�b��� HideJudgment ���\�b�h���Ăяo��

        }
        else if (AnswerFlag == false)//�s����
        {
            JudgmentObj[1].SetActive(true);
            videoPlayers[1].Play();
//            Character.sprite = newCharacters[2];
//            JudgmentObj[1].GetComponentInChildren<TextMeshProUGUI>().color = Color.blue;
//            JudgmentObj[1].GetComponentInChildren<TextMeshProUGUI>().text = ("�~");


            Invoke("HideJudgment", 2.0f); // 2.0�b��� HideJudgment ���\�b�h���Ăяo��
        }
        index += 1;//���̖���

        //�����̊���
        float Rate = correct / (float)index * 100;
        string Ratenum = Rate.ToString("F0");
        string correctNum = correct.ToString();
        CorrectRate.text = Ratenum;
        CorrectNum.text = correctNum;
    }


    void ChangeScene()
    {
        SceneManager.LoadScene("TimingGame");
    }


    void HideJudgment()
    {
        JudgmentObj[0].SetActive(false);
        JudgmentObj[1].SetActive(false);
    }

    //�Đ��I����A������ʔ�\��
    void OnVideoEnd(VideoPlayer vp)
    {
        JudgmentObj[0].SetActive(false);
        JudgmentObj[1].SetActive(false);
    }

    void ResetTimer()
    {
        BGM.time = 0;
        nowtimer = 0f;
        Time.timeScale = 1.0f;
    }

    public void Back()
    {
        SceneManager.LoadScene("Title");
    }
    public void RetryClick()
    {
        SceneManager.LoadScene("Play");
    }

    // Fisher-Yates�V���b�t��
    void Shuffle<T>(List<T> list)
    {
        int n = list.Count;
        for (int i = n - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            T temp = list[i];
            list[i] = list[j];
            list[j] = temp;
        }
    }
}
