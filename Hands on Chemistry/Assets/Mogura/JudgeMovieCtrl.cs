using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class JudgeMovieCtrl : MonoBehaviour
{
    public VideoPlayer[] videoPlayers; // 正解不正解の表示-ビデオ
    public GameObject[] JudgmentObj;   // 正解不正解の表示-オブジェクト

    void Start()
    {
        videoPlayers = new VideoPlayer[JudgmentObj.Length];
        for (int i = 0; i < JudgmentObj.Length; i++)
        {
            videoPlayers[i] = JudgmentObj[i].GetComponentInChildren<VideoPlayer>();
            videoPlayers[i].loopPointReached += OnVideoEnd;  // ループしないように
        }
        JudgmentObj[0].SetActive(false);    // 正解アニメーション非表示
        JudgmentObj[1].SetActive(false);    // 不正解アニメーション非表示
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
