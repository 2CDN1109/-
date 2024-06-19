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
            game.OnGameEnded = OnGameEnded; // �f���Q�[�g�Ɉ�v���郁�\�b�h�����蓖��
            game.SetGameObjectsActive(false); // �ŏ��͑S�ẴQ�[���I�u�W�F�N�g���\����
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
        timingGames[currentGameIndex].SetGameObjectsActive(false); // ���݂̃Q�[�����\����

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
        yield return new WaitUntil(() => Input.GetKeyUp(KeyCode.Space)); // �X�y�[�X�L�[���������܂őҋ@
        timingGames[currentGameIndex].SetGameObjectsActive(true); // ���݂̃Q�[�����ĕ\��
        timingGames[currentGameIndex].StartGame(); // �Q�[�����ĊJ
    }
}
