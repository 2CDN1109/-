using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameClearCheckerWithGaugeAndRotation : MonoBehaviour
{
    public GameObject myBox; // 自分が動かすオブジェクト
    public GameObject point; // ゲームクリアの目標となるオブジェクト
    public GameObject forceps; // Y回転を増加させるオブジェクト
    public GameObject ProgressSlider;
    public Slider progressSlider; // ゲージ用のスライダー
    public float overlapTimeNeeded = 5f; // ゲームクリアに必要な重なり時間（秒）
    public float rotationSpeed = 10f; // Y回転の増加速度

    private float overlapTime = 0f;

    public JudgeMovieCtrl judgeMovieCtrl;
    private bool gameCleared = false; // ゲームクリアのフラグ

    void Start()
    {
        ProgressSlider.SetActive(true);
    }

    void Update()
    {
        // myBoxとpointの重なりを判定
        if (myBox != null && point != null && !gameCleared)
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
                    gameCleared = true; // ゲームクリアのフラグを立てる
                    Debug.Log("Game Clear!");
                    // ゲームクリア時の処理をここに追加
                    judgeMovieCtrl.JudgmentObj[0].SetActive(true);
                    judgeMovieCtrl.videoPlayers[0].Play();
                    Invoke("ShowFinish", 2.5f);
                    Invoke("GoTitle", 21.5f);
                }
            }
            else
            {
                // 重なっていない場合
                if (overlapTime > 0)
                {
                    overlapTime -= Time.deltaTime *2;
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

    void ShowFinish() 
    {
        ProgressSlider.SetActive(false);
        judgeMovieCtrl.JudgmentObj[1].SetActive(true);
        judgeMovieCtrl.videoPlayers[1].Play();
    }

    void GoTitle()
    {
        SceneManager.LoadScene("title");
    }
}
