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
    private TextAsset csvFile;  // CSVファイル
    private List<string[]> csvData = new List<string[]>();  // CSVファイルの中身を入れるリスト
    int index = 0; //問題番号
    int correct; //正解数
    bool AnswerFlag;    //解答判定
    float timelimit = 10f; //制限時間
    float nowtimer = 0f;   //経過時間
    public TextMeshProUGUI ProblemLog;  //問題文
    public TextMeshProUGUI ProblemNum;  //問題番号
    public TextMeshProUGUI CorrectNum;   //正解数
    public TextMeshProUGUI CorrectRate;  //正答率
    public TextMeshProUGUI time;    //残り時間表示
    public Button[] ChoiceBtns;   //選択肢
    public Image[] ChoiceImages;  //選択肢の画像
    private List<int> numbers = new List<int> { 1, 2, 3, 4 };   //選択肢番号
    private List<int> Problem = new List<int> { }; //未正解問題
    public GameObject[] JudgmentObj;   //正解不正解の表示-オブジェクト
    private VideoPlayer[] videoPlayers; //正解不正解の表示-ビデオ
    public Slider TimeSlider;   //残り時間ゲージ
//    public Image Character;      //画像
    public Sprite[] newCharacters;  //格納している画像を変化させる
    public GameObject Result;       //リザルト画面の表示
    public AudioSource BGM;     //BGM
    public AudioSource ClearSound;
    public AudioSource FailSound;


    public Image ImageCursor; // カーソルとして使う画像（枠）
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

        //CSVロード
        csvFile = Resources.Load("test") as TextAsset;
        StringReader reader = new StringReader(csvFile.text);
        while (reader.Peek() != -1)
        {
            string line = reader.ReadLine();
            csvData.Add(line.Split(','));
        }

        //CSVの問題をリストにいれる
        Problem = new List<int>(csvData.Count);
        for (int i = 0; i < csvData.Count; i++)
        {
            Problem.Add(i);
        }
        Shuffle(Problem);  //問題をシャッフル
        Debug.Log("Problem: " + string.Join(", ", Problem.Select(x => x.ToString())));

        TimeSlider.value = 1f;  //制限時間ゲージ

        Result.SetActive(false);    //結果発表を非表示
        JudgmentObj[0].SetActive(false);    //正解アニメーション非表示
        JudgmentObj[1].SetActive(false);    //不正解アニメーション非表示

        //ビデオを保存しているオブジェクトをビデオにする
        videoPlayers = new VideoPlayer[JudgmentObj.Length];
        for (int i = 0; i < JudgmentObj.Length; i++)
        {
            videoPlayers[i] = JudgmentObj[i].GetComponentInChildren<VideoPlayer>();
            videoPlayers[i].loopPointReached += OnVideoEnd;  //ループしないように
        }
        // 最初の問題を表示
        ShowNextQuestion();
    }

    void Update()
    {
        //時間制御
        nowtimer += Time.deltaTime; // タイマー
        float t = nowtimer / timelimit;// スライダーの値ー正規化
        TimeSlider.value = Mathf.Lerp(1f, 0f, t);
        float TimeLimit = 15f - nowtimer; //残り時間
        TimeLimit = Mathf.Max(TimeLimit, 0f);

        string LimitLog = TimeLimit.ToString("F0");
        time.text = LimitLog + "秒";
        time.color = (TimeLimit > 5.5f) ? Color.green : Color.red;// 5.5秒以上は緑、5.5秒未満は赤

        // タイムオーバー
        if (nowtimer >= timelimit)
        {
            nowtimer = 0f;     //タイマーを0秒に戻す
            JudgmentObj[1].SetActive(true); //不正解を表示
            videoPlayers[1].Play(); //動画を流す
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
        yield return new WaitForSeconds(2.0f);  // 2.0秒待つ

        ShowNextQuestion();
    }

    void ShowNextQuestion()
    {
//        Character.sprite = newCharacters[0];
        string No = (index + 1).ToString(); //問題番号
        ProblemNum.text = "第" + No + "問";

        // 問題と選択肢を表示
        if (Problem.Count > index)
        {
            if (index < 10) //10問連続で出題、0から始めているから未満
            {
                Quiz();
            }
            else
            {
                Result.SetActive(true);
                Time.timeScale = 0;
                BGM.Stop();
                Debug.Log("問題終了");
                
            }
        }
        else if (Problem.Count == index)
        {

            Debug.Log("終了!!!!");
            Result.SetActive(true);
            BGM.Stop();
            Invoke("ChangeScene", 2.0f); // 2.0秒後に HideJudgment メソッドを呼び出す

        }
    }

    void Quiz()
    {
        for (int i = 0; i < numbers.Count; i++)
        {
            int randomrow = Problem[index];
            int randomcol = numbers[i];
            ProblemLog.text = csvData[randomrow][0];

            // ボタンのテキストを非表示にして画像を表示

            string imagePath = csvData[randomrow][randomcol + 4];
            Sprite choiceSprite = Resources.Load<Sprite>(imagePath);
            ChoiceImages[i].sprite = choiceSprite;

            // すでにイベントが追加されている場合は一旦解除してから追加
            ChoiceBtns[i].onClick.RemoveAllListeners();
            ChoiceBtns[i].onClick.AddListener(() => OnButtonClick(randomcol));
        }

        // タイマーをリセット
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
        // 次の問題を表示
        StartCoroutine(NextQuestion());
    }

    void AnswerCheck()
    {
        ResetTimer();
        if (AnswerFlag == true)//正解
        {
            JudgmentObj[0].SetActive(true);
            videoPlayers[0].Play();
//            Character.sprite = newCharacters[1];
//            JudgmentObj[0].GetComponentInChildren<TextMeshProUGUI>().color = Color.red;
//            JudgmentObj[0].GetComponentInChildren<TextMeshProUGUI>().text = ("〇");
            correct += 1;   //正解数

            Invoke("HideJudgment", 2.0f); // 2.0秒後に HideJudgment メソッドを呼び出す

        }
        else if (AnswerFlag == false)//不正解
        {
            JudgmentObj[1].SetActive(true);
            videoPlayers[1].Play();
//            Character.sprite = newCharacters[2];
//            JudgmentObj[1].GetComponentInChildren<TextMeshProUGUI>().color = Color.blue;
//            JudgmentObj[1].GetComponentInChildren<TextMeshProUGUI>().text = ("×");


            Invoke("HideJudgment", 2.0f); // 2.0秒後に HideJudgment メソッドを呼び出す
        }
        index += 1;//次の問題へ

        //正解の割合
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

    //再生終了後、正解画面非表示
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

    // Fisher-Yatesシャッフル
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
