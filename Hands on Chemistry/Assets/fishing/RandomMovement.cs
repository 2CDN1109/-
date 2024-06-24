using UnityEngine;

public class SmoothRandomMovement : MonoBehaviour
{
    public float minX = 4.75f;          // X座標の最小値
    public float maxX = 5.65f;          // X座標の最大値
    public float moveDuration = 2.0f;   // 移動にかける時間（秒）

    private Vector3 startPosition;      // 移動開始位置
    private Vector3 targetPosition;     // 移動目標位置
    private float startTime;            // 移動開始時間

    void Start()
    {
        // 初期位置の設定
        startPosition = transform.position;
        // 移動開始時刻の記録
        startTime = Time.time;
        // ランダムな目標位置の設定
        float randomX = Random.Range(minX, maxX);
        targetPosition = new Vector3(randomX, transform.position.y, transform.position.z);
    }

    void Update()
    {
        // 現在の時間に対する移動の進捗度合い（0から1の範囲）
        float progress = (Time.time - startTime) / moveDuration;
        // 移動が完了していない場合
        if (progress < 1.0f)
        {
            // 現在の位置を補間して計算
            transform.position = Vector3.Lerp(startPosition, targetPosition, progress);
        }
        else
        {
            // 移動が完了したら次の目標位置を設定して再度移動を開始する
            float randomX = Random.Range(minX, maxX);
            startPosition = transform.position;
            targetPosition = new Vector3(randomX, transform.position.y, transform.position.z);
            startTime = Time.time;
        }
    }
}
