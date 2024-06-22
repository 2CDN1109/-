using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // 追加: UIを操作するために必要

public class GameManager : MonoBehaviour
{
    public List<TimingGame> timingGames;
    private int currentGameIndex = 0;

    // 追加: クリア画像を表示するためのImageオブジェクト
    public Image clearImage;

    void Start()
    {
        foreach (var game in timingGames)
        {
            game.OnGameEnded = OnGameEnded;
            game.SetGameObjectsActive(false);
        }
        StartNextGame();
    }

    void StartNextGame()
    {
        if (currentGameIndex < timingGames.Count)
        {
            timingGames[currentGameIndex].SetGameObjectsActive(true);
            timingGames[currentGameIndex].StartGame();
        }
        else
        {
            Debug.Log("All games completed!");
            ShowClearImage(); // 追加: すべてのゲームが完了したらクリア画像を表示
        }
    }

    void OnGameEnded(bool success)
    {
        Debug.Log(timingGames[currentGameIndex].gameObject.name + " has ended " + (success ? "successfully." : "unsuccessfully."));
        timingGames[currentGameIndex].SetGameObjectsActive(false);

        if (success)
        {
            currentGameIndex++;
            StartNextGame();
        }
        else
        {
            StartCoroutine(RetryCurrentGame());
        }
    }

    IEnumerator RetryCurrentGame()
    {
        Debug.Log("RetryCurrentGame called for game index: " + currentGameIndex);

        // ここで任意の待機時間を設定します（例: 3秒）
        yield return new WaitForSeconds(1f);

        // 現在のゲームをリトライ
        timingGames[currentGameIndex].StartGame();
    }



    void ShowClearImage()
    {
        if (clearImage != null)
        {
            clearImage.gameObject.SetActive(true); // クリア画像を表示
        }
        else
        {
            Debug.LogError("Clear image is not set in the GameManager.");
        }
    }
}
