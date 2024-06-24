using UnityEngine;
using System.IO.Ports; // シリアル通信用のライブラリを追加

public class MoveObjectWithArduino : MonoBehaviour
{
    public float minX = 4.75f;  // X座標の最小値
    public float maxX = 5.65f;  // X座標の最大値
    public int serialPortNumber = 3;  // Arduinoのシリアルポート番号
    public int baudRate = 9600;  // Arduinoのボーレート

    private SerialPort serialPort;

    void Start()
    {
        // Arduinoとのシリアルポートを設定
        string portName = "COM" + serialPortNumber;
        serialPort = new SerialPort(portName, baudRate);
        serialPort.Open(); // シリアルポートを開く
    }

    void Update()
    {
        if (serialPort.IsOpen)
        {
            try
            {
                // Arduinoから値を読み取る
                int sensorValue = int.Parse(serialPort.ReadLine());

                // 値をX座標にマッピングしてmyBoxを移動させる
                float mappedX = Map(sensorValue, 0, 1023, minX, maxX);
                Vector3 newPosition = transform.position;
                newPosition.x = mappedX;
                transform.position = newPosition;
            }
            catch (System.Exception e)
            {
                Debug.LogWarning(e.Message);
            }
        }
    }

    void OnDestroy()
    {
        // シリアルポートを閉じる
        if (serialPort != null && serialPort.IsOpen)
        {
            serialPort.Close();
        }
    }

    // 値を指定された範囲にマッピングする関数
    float Map(float value, float fromSource, float toSource, float fromTarget, float toTarget)
    {
        return (value - fromSource) / (toSource - fromSource) * (toTarget - fromTarget) + fromTarget;
    }
}
