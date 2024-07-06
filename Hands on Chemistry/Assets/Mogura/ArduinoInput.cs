using System;
using System.IO.Ports;
using UniRx;
using UnityEngine;

public class ArduinoInput : MonoBehaviour
{
    private SerialPort serialPort = new SerialPort("COM3", 9600); // �K�؂�COM�|�[�g�ɒu�������Ă�������
    public QuizManager quizManager;
    public QuizUIManager quizUIManager;

    private readonly Subject<string> _inputSubject = new Subject<string>();
//    public float inputCooldown = 0.005f; // ���͂̃N�[���_�E�����ԁi�b�j

    void Start()
    {
        serialPort.Open();
        serialPort.ReadTimeout = 10000; // �^�C���A�E�g��10�b�ɐݒ�

        // �f�[�^��M�̊Ď�
        Observable.EveryUpdate()
            .Where(_ => serialPort.IsOpen && serialPort.BytesToRead > 0)
            .Select(_ =>
            {
                try
                {
                    return serialPort.ReadLine().Trim();
                }
                catch (Exception ex)
                {
                    Debug.LogError("Error: " + ex.Message);
                    return null;
                }
            })
            .Where(data => !string.IsNullOrEmpty(data))
//            .ThrottleFirst(TimeSpan.FromSeconds(inputCooldown)) // �N�[���_�E����K�p
            .Subscribe(data => _inputSubject.OnNext(data))
            .AddTo(this);

        // ���͏���
        _inputSubject.Subscribe(line =>
        {
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
        }).AddTo(this);
    }

    void OnApplicationQuit()
    {
        if (serialPort != null && serialPort.IsOpen)
        {
            serialPort.Close();
        }
    }
}
