using UnityEngine;

public class GameClearChecker : MonoBehaviour
{
    public GameObject myBox; // 自分が動かすオブジェクト
    public GameObject point; // ゲームクリアの目標となるオブジェクト
    public float overlapTimeNeeded = 5f; // ゲームクリアに必要な重なり時間（秒）

    private float overlapTime = 0f;
    private bool isOverlapping = false;

    void Update()
    {
        // myBoxとpointの重なりを判定
        if (myBox != null && point != null)
        {
            if (myBox.GetComponent<Renderer>().bounds.Intersects(point.GetComponent<Renderer>().bounds))
            {
                // 重なっている場合
                overlapTime += Time.deltaTime;
                if (overlapTime >= overlapTimeNeeded && !isOverlapping)
                {
                    // ゲームクリア処理
                    Debug.Log("Game Clear!");
                    isOverlapping = true;
                    // ここにゲームクリア時の処理を追加する（例：次のシーンに遷移する、メッセージを表示するなど）
                }
            }
            else
            {
                // 重なっていない場合
                overlapTime = 0f;
                isOverlapping = false;
            }
        }
    }
}
