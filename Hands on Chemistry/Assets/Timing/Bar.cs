using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimingGame : MonoBehaviour
{
    private float moveSpeed = 2.0f;
    private int direction = 1;
    private bool isGameActive = false;
    private Vector3 initialPosition;

    public delegate void GameEndedCallback(bool success);
    public GameEndedCallback OnGameEnded;

    // 追加：関連するオブジェクト
    public GameObject meter;
    public GameObject hitZone;

    // 追加：成功音のオーディオソース
    public AudioSource successAudioSource;

    // Game2とGame3用のフラグ
    public bool isGame2 = false;
    public bool isGame3 = false;

    // ヒット判定の範囲（例として初期値を -0.7f から 0.7f に設定）
    private float hitRangeMin = -0.7f;
    private float hitRangeMax = 0.7f;

    void Start()
    {
        initialPosition = transform.position; // 初期位置を記憶

        // Game2の場合、移動速度と方向を調整
        if (isGame2)
        {
            moveSpeed = 3.0f; // 例として移動速度を3.0に設定
            direction = -1; // 例として逆方向に移動
            // Game2用のヒット判定範囲を設定する場合
            hitRangeMin = -2.4f;
            hitRangeMax = -1.0f;
        }
        // Game3の場合、移動速度とヒット判定範囲を調整
        else if (isGame3)
        {
            moveSpeed = 1.5f; // 例として移動速度を1.5に設定
            direction = 1; // 例として通常方向に移動
            // Game3用のヒット判定範囲を設定する場合
            hitRangeMin = 1.0f;
            hitRangeMax = 2.0f;
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
        if (isGameActive && Input.GetKeyUp(KeyCode.Space))
        {
            if (transform.position.y >= hitRangeMin && transform.position.y <= hitRangeMax)
            {
                Debug.Log("Hit");
                successAudioSource.Play(); // 成功音を再生
                EndGame(true);
            }
            else
            {
                Debug.Log("Miss");
                EndGame(false);
            }
        }
    }

    public void StartGame()
    {
        isGameActive = true;
        transform.position = initialPosition; // 初期位置にリセット
        SetGameObjectsActive(true); // ゲーム関連のオブジェクトをアクティブに
        Debug.Log(gameObject.name + " has started.");
    }

    private void EndGame(bool success)
    {
        isGameActive = false;
        Debug.Log(gameObject.name + " is ending.");
        SetGameObjectsActive(false); // ゲーム関連のオブジェクトを非表示に
        OnGameEnded?.Invoke(success);
    }

    private void RetryGame()
    {
        // ゲームをリトライする処理
        StartGame(); // ゲームを再開する
    }

    public void SetGameObjectsActive(bool isActive) // メソッドをpublicに変更
    {
        gameObject.SetActive(isActive);
        if (meter != null) meter.SetActive(isActive);
        if (hitZone != null) hitZone.SetActive(isActive);
    }
}
