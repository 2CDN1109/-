using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // �ǉ�: UI�𑀍삷�邽�߂ɕK�v

public class GameManager : MonoBehaviour
{
    public List<TimingGame> timingGames;
    private int currentGameIndex = 0;

    // �ǉ�: �N���A�摜��\�����邽�߂�Image�I�u�W�F�N�g
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
            ShowClearImage(); // �ǉ�: ���ׂẴQ�[��������������N���A�摜��\��
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

        // �����ŔC�ӂ̑ҋ@���Ԃ�ݒ肵�܂��i��: 3�b�j
        yield return new WaitForSeconds(1f);

        // ���݂̃Q�[�������g���C
        timingGames[currentGameIndex].StartGame();
    }



    void ShowClearImage()
    {
        if (clearImage != null)
        {
            clearImage.gameObject.SetActive(true); // �N���A�摜��\��
        }
        else
        {
            Debug.LogError("Clear image is not set in the GameManager.");
        }
    }
}
