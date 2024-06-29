using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class JudgeMovieCtrl : MonoBehaviour
{
    public VideoPlayer[] videoPlayers; // ����s�����̕\��-�r�f�I
    public GameObject[] JudgmentObj;   // ����s�����̕\��-�I�u�W�F�N�g

    void Start()
    {
        videoPlayers = new VideoPlayer[JudgmentObj.Length];
        for (int i = 0; i < JudgmentObj.Length; i++)
        {
            videoPlayers[i] = JudgmentObj[i].GetComponentInChildren<VideoPlayer>();
            videoPlayers[i].loopPointReached += OnVideoEnd;  // ���[�v���Ȃ��悤��
        }
        JudgmentObj[0].SetActive(false);    // �����A�j���[�V������\��
        JudgmentObj[1].SetActive(false);    // �s�����A�j���[�V������\��
    }




    void OnVideoEnd(VideoPlayer vp)
    {
        HideJudgment();
    }

    public void HideJudgment()
    {
        JudgmentObj[0].SetActive(false);
        JudgmentObj[1].SetActive(false);
    }
}
