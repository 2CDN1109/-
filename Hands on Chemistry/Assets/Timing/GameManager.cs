using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // TextMeshPro���g�p���邽�߂ɒǉ�
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public List<TimingGame> timingGames;
    private int currentGameIndex = 0;

    public Image clearImage;
    public Transform capsule; // �J�v�Z���I�u�W�F�N�g��Transform��ݒ�
    public TMP_Text gameDescriptionText; // TextMeshPro�̃e�L�X�g
    public TMP_Text countdownText; // �J�E���g�_�E���\���p��TextMeshPro�e�L�X�g

    void Start()
    {
        foreach (var game in timingGames)
        {
            game.OnGameEnded = OnGameEnded;
            game.SetGameObjectsActive(false);
        }
        ShowGameDescription();
    }

    void ShowGameDescription()
    {
        if (gameDescriptionText != null)
        {
            gameDescriptionText.gameObject.SetActive(true);
            gameDescriptionText.text = "�Q�[���̐���...\n�X�y�[�X�L�[�������ăQ�[�����J�n";

            StartCoroutine(WaitForSpaceKey());
        }
        else
        {
            Debug.LogError("Game description text is not set in the GameManager.");
        }
    }

    IEnumerator WaitForSpaceKey()
    {
        while (!Input.GetKeyDown(KeyCode.Space))
        {
            yield return null;
        }

        if (gameDescriptionText != null)
        {
            gameDescriptionText.gameObject.SetActive(false);
        }

        StartCoroutine(CountdownAndStartGame());
    }

    IEnumerator CountdownAndStartGame()
    {
        if (countdownText != null)
        {
            countdownText.gameObject.SetActive(true);
            for (int i = 3; i > 0; i--)
            {
                countdownText.text = i.ToString();
                yield return new WaitForSeconds(1f);
            }
            countdownText.text = "�X�^�[�g�I";
            yield return new WaitForSeconds(1f);
            countdownText.gameObject.SetActive(false);
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
            ShowClearImage();
        }
    }

    void OnGameEnded(bool success)
    {
        Debug.Log(timingGames[currentGameIndex].gameObject.name + " has ended " + (success ? "successfully." : "unsuccessfully."));
        timingGames[currentGameIndex].SetGameObjectsActive(false);

        if (success)
        {
            currentGameIndex++;
            StartCoroutine(AnimateCapsuleAndStartNextGame());
        }
        else
        {
            if (timingGames[currentGameIndex].failureAudioSource != null)
            {
                timingGames[currentGameIndex].failureAudioSource.Play();
            }
            StartCoroutine(RetryCurrentGame());
        }
    }

    IEnumerator RetryCurrentGame()
    {
        Debug.Log("RetryCurrentGame called for game index: " + currentGameIndex);
        yield return new WaitForSeconds(1f);
        timingGames[currentGameIndex].StartGame();
    }

    void ShowClearImage()
    {
        if (clearImage != null)
        {
            clearImage.gameObject.SetActive(true);
        }
        else
        {
            Debug.LogError("Clear image is not set in the GameManager.");
        }
    }

    IEnumerator AnimateCapsuleAndStartNextGame()
    {
        if (capsule != null)
        {
            // �J�v�Z�������E�ɓ������A�j���[�V����
            Vector3 startPosition = capsule.position;
            Vector3 leftPosition = startPosition + Vector3.left * 0.1f;
            Vector3 rightPosition = startPosition + Vector3.right * 1.8f;

            float animationDuration = 1.0f;
            float elapsedTime = 0f;

            while (elapsedTime < animationDuration)
            {
                float t = elapsedTime / animationDuration;
                capsule.position = Vector3.Lerp(leftPosition, rightPosition, Mathf.PingPong(t * 2, 1));
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            capsule.position = startPosition;
        }

        StartNextGame();
    }
}
