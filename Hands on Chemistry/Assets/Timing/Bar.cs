using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class TimingGame : MonoBehaviour
{
    private float moveSpeed = 2.0f;
    private int direction = 1;
    private bool isGameActive = false;
    private Vector3 initialPosition;

    public delegate void GameEndedCallback(bool success);
    public GameEndedCallback OnGameEnded;

    public GameObject meter;
    public GameObject hitZone;
    public AudioSource successAudioSource;
    public AudioSource failureAudioSource;

    public bool isGame2 = false;
    public bool isGame3 = false;

    private float hitRangeMin = -0.7f;
    private float hitRangeMax = 0.7f;

    private SerialHandler serialHandler;
    public JudgeMovieCtrl judgeMovieCtrl;

    void Start()
    {
        initialPosition = transform.position;

        if (successAudioSource == null)
        {
            Debug.LogError("SuccessAudioSource is not set in " + gameObject.name);
        }

        if (isGame2)
        {
            moveSpeed = 4.0f;
            direction = 1;
            hitRangeMin = -2.5f;
            hitRangeMax = -1.2f;
        }
        else if (isGame3)
        {
            moveSpeed = 6.5f;
            direction = 1;
            hitRangeMin = 1.0f;
            hitRangeMax = 1.4f;
        }

        // シリアルハンドラーのセットアップ
        serialHandler = FindObjectOfType<SerialHandler>();
        if (serialHandler != null)
        {
            serialHandler.OnMessageReceived.Subscribe(OnSerialMessageReceived);
        }
        else
        {
            Debug.LogError("SerialHandler not found.");
        }
    }

    void FixedUpdate()
    {
        if (isGameActive)
        {
            if (transform.position.y >= 2.0f)
                direction = -1;
            if (transform.position.y <= -2.0f)
                direction = 1;
            transform.position = new Vector3(initialPosition.x,
                transform.position.y + moveSpeed * Time.fixedDeltaTime * direction, initialPosition.z);
        }
    }

    private void Update()
    {
        if (isGameActive && (Input.GetKeyUp(KeyCode.Space) || InputReceived("SPACE")))
        {
            CheckHit();
        }
    }

    void OnSerialMessageReceived(string message)
    {
        // スイッチが押された（値が0になった）場合にヒットをチェック
        if (isGameActive && (message.Trim() == "LEFT" || message.Trim() == "RIGHT" || message.Trim() == "SPACE" || message.Trim() == "BUTTON_PRESSED"))
        {
            CheckHit();
        }
    }

    private void CheckHit()
    {
        if (transform.position.y >= hitRangeMin - 1.0f && transform.position.y <= hitRangeMax + 1.0f)
        {
            Debug.Log("Hit");
            if (successAudioSource != null)
            {
                successAudioSource.Play();
                judgeMovieCtrl.videoPlayers[0].Play();
                judgeMovieCtrl.JudgmentObj[0].SetActive(true);
            }
            EndGame(true);
        }
        else
        {
            Debug.Log("Miss");
            judgeMovieCtrl.videoPlayers[1].Play();
            judgeMovieCtrl.JudgmentObj[1].SetActive(true);
            EndGame(false);
        }
    }

    public void StartGame()
    {
        isGameActive = true;
        transform.position = initialPosition;
        SetGameObjectsActive(true); // ゲームオブジェクトをアクティブにする
        Debug.Log(gameObject.name + " has started.");
    }

    private void EndGame(bool success)
    {
        isGameActive = false;
        Debug.Log(gameObject.name + " is ending.");
        SetGameObjectsActive(false);
        OnGameEnded?.Invoke(success);
    }

    public void SetGameObjectsActive(bool isActive)
    {
        gameObject.SetActive(isActive);
        if (meter != null) meter.SetActive(isActive);
        if (hitZone != null) hitZone.SetActive(isActive);
    }

    private bool InputReceived(string input)
    {
        return false; // 必要に応じて実装
    }
}
