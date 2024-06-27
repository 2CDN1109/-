using UnityEngine;
using UnityEngine.UI;
using System.IO.Ports;

public class ArduinoController : MonoBehaviour
{
    public Image gaugeFill;         // ゲージのUI Image
    public Text instructionText;    // 指示のUI Text

    private SerialPort serialPort;
    private float gaugeValue = 0f;

    void Start()
    {
        // シリアルポートの設定
        serialPort = new SerialPort("COM3", 9600); // 必要に応じてポート番号を変更
        serialPort.Open();
    }

    void Update()
    {
        if (serialPort.IsOpen)
        {
            try
            {
                // シリアルポートからデータを読み取る
                string data = serialPort.ReadLine();
                float value;
                if (float.TryParse(data, out value))
                {
                    gaugeValue = Mathf.Clamp01(value / 1023f); // 0-1023 の範囲を 0-1 に正規化
                    gaugeFill.fillAmount = gaugeValue;
                }
            }
            catch (System.Exception)
            {
                // 読み取り失敗時の処理
            }
        }

        // インストラクションテキストの更新
        instructionText.text = "Adjust the gauge with the potentiometer.";
    }

    void OnApplicationQuit()
    {
        if (serialPort.IsOpen)
        {
            serialPort.Close();
        }
    }
}
