using UnityEngine;
using UnityEngine.UI;

public class GameClearCheckerWithGaugeAndRotation : MonoBehaviour
{
    public GameObject myBox; // 自分が動かすオブジェクト
    public GameObject point; // ゲームクリアの目標となるオブジェクト
    public GameObject forceps; // Y回転を増加させるオブジェクト
    public JudgeMovieCtrl judgeMovieCtrl;
    public Slider progressSlider; // ゲージ用のスライダー
    public float overlapTimeNeeded = 10f; // ゲームクリアに必要な重なり時間（秒）
    public float rotationSpeed = 10f; // Y回転の増加速度

    private float overlapTime = 0f;

    void Update()
    {
        // myBoxとpointの重なりを判定
        if (myBox != null && point != null)
        {
            if (myBox.GetComponent<Renderer>().bounds.Intersects(point.GetComponent<Renderer>().bounds))
            {
                // 重なっている場合
                overlapTime += Time.deltaTime;

                // forcepsのY回転を増加させる
                RotateForceps();

                if (overlapTime >= overlapTimeNeeded)
                {
                    // ゲームクリア処理
                    Debug.Log("Game Clear!");
                    // ゲームクリア時の処理をここに追加
                    judgeMovieCtrl.videoPlayers[0].Play();
                    Invoke("ShowClear", 0.1f);
                    Invoke("ShowFinish", 2.0f);
                }
            }
            else
            {
                // 重なっていない場合
                if (overlapTime > 0f)
                {
                    overlapTime -= Time.deltaTime * 3;
                }
                else
                {
                    overlapTime = 0f;
                }

            }

            // ゲージを更新
            UpdateGauge();
        }
    }

    // ゲージを更新するメソッド
    void UpdateGauge()
    {
        if (progressSlider != null)
        {
            progressSlider.value = overlapTime / overlapTimeNeeded;
        }
    }

    // forcepsのY回転を増加させるメソッド
    void RotateForceps()
    {
        if (forceps != null)
        {
            Vector3 rotation = forceps.transform.rotation.eulerAngles;
            rotation.y += rotationSpeed * Time.deltaTime;
            forceps.transform.rotation = Quaternion.Euler(rotation);
        }
    }

    void ShowClear()
    {
        judgeMovieCtrl.JudgmentObj[0].SetActive(true);
    }
    void ShowFinish()
    {
        judgeMovieCtrl.JudgmentObj[1].SetActive(true);
        judgeMovieCtrl.videoPlayers[1].Play();
    }
}
