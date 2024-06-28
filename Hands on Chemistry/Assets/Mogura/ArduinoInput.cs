using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;

public class ArduinoInput : MonoBehaviour
{
    SerialPort serialPort = new SerialPort("COM3", 9600); // �K�؂�COM�|�[�g�ɒu�������Ă�������

    public QuizManager quizManager;
    public QuizUIManager quizUIManager;

    private float lastInputTime = 0f; // �Ō�ɓ��͂��󂯕t��������
    public float inputCooldown = 0.2f; // ���͂̃N�[���_�E�����ԁi�b�j

    void Start()
    {
        serialPort.Open();
        serialPort.ReadTimeout = 10000; // �^�C���A�E�g��1�b�ɐݒ�
    }

    void Update()
    {
        if (serialPort.IsOpen)
        {
            try
            {
                string line = serialPort.ReadLine().Trim(); // ��M������������擾���A�]���ȋ󔒂��g��������

                // �N�[���_�E�����͏������Ȃ�
                if (Time.time - lastInputTime < inputCooldown)
                {
                    return;
                }

                // LEFT, RIGHT, SPACE �̏���
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

                // �Ō�̓��͎��Ԃ��X�V
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
