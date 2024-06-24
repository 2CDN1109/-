using UnityEngine;
using UnityEngine.UI;
using System.IO.Ports;

public class ArduinoController : MonoBehaviour
{
    public Image gaugeFill;         // �Q�[�W��UI Image
    public Text instructionText;    // �w����UI Text

    private SerialPort serialPort;
    private float gaugeValue = 0f;

    void Start()
    {
        // �V���A���|�[�g�̐ݒ�
        serialPort = new SerialPort("COM3", 9600); // �K�v�ɉ����ă|�[�g�ԍ���ύX
        serialPort.Open();
    }

    void Update()
    {
        if (serialPort.IsOpen)
        {
            try
            {
                // �V���A���|�[�g����f�[�^��ǂݎ��
                string data = serialPort.ReadLine();
                float value;
                if (float.TryParse(data, out value))
                {
                    gaugeValue = Mathf.Clamp01(value / 1023f); // 0-1023 �͈̔͂� 0-1 �ɐ��K��
                    gaugeFill.fillAmount = gaugeValue;
                }
            }
            catch (System.Exception)
            {
                // �ǂݎ�莸�s���̏���
            }
        }

        // �C���X�g���N�V�����e�L�X�g�̍X�V
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
