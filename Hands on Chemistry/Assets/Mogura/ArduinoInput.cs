using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;

public class ArduinoInput : MonoBehaviour
{
    SerialPort serialPort = new SerialPort("COM3", 9600); // 適切なCOMポートに置き換えてください

    public QuizManager quizManager;
    public QuizUIManager quizUIManager;

    private float lastInputTime = 0f; // 最後に入力を受け付けた時間
    public float inputCooldown = 0.2f; // 入力のクールダウン時間（秒）

    void Start()
    {
        serialPort.Open();
        serialPort.ReadTimeout = 10000; // タイムアウトを1秒に設定
    }

    void Update()
    {
        if (serialPort.IsOpen)
        {
            try
            {
                string line = serialPort.ReadLine().Trim(); // 受信した文字列を取得し、余分な空白をトリムする

                // クールダウン中は処理しない
                if (Time.time - lastInputTime < inputCooldown)
                {
                    return;
                }

                // LEFT, RIGHT, SPACE の処理
                if (line == "LEFT")
                {
                    quizUIManager.SelectPreviousChoice();
                    Debug.Log("Left key pressed");
                }
                else if (line == "RIGHT")
                {
                    quizUIManager.SelectNextChoice();
                    Debug.Log("Right key pressed");
                }
                else if (line == "SPACE")
                {
                    int selectedChoiceIndex = quizUIManager.GetSelectedChoiceIndex();
                    quizManager.CheckAnswer(selectedChoiceIndex);
                    Debug.Log("Space key pressed");
                }
                else
                {
                    Debug.LogWarning("Received unrecognized input: " + line);
                }

                // 最後の入力時間を更新
                lastInputTime = Time.time;
            }
            catch (System.Exception ex)
            {
                Debug.LogError("Error: " + ex.Message);
            }
        }
    }

    void OnApplicationQuit()
    {
        serialPort.Close();
    }
}
