using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public List<TimingGame> timingGames;
    private int currentGameIndex = 0;

    void Start()
    {
        foreach (var game in timingGames)
        {
            game.OnGameEnded = OnGameEnded; // デリゲートに一致するメソッドを割り当て
            game.SetGameObjectsActive(false); // 最初は全てのゲームオブジェクトを非表示に
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
        }
    }

    void OnGameEnded(bool success)
    {
        Debug.Log(timingGames[currentGameIndex].gameObject.name + " has ended " + (success ? "successfully." : "unsuccessfully."));
        timingGames[currentGameIndex].SetGameObjectsActive(false); // 現在のゲームを非表示に

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
        yield return new WaitUntil(() => Input.GetKeyUp(KeyCode.Space)); // スペースキーが押されるまで待機
        timingGames[currentGameIndex].SetGameObjectsActive(true); // 現在のゲームを再表示
        timingGames[currentGameIndex].StartGame(); // ゲームを再開
    }
}
