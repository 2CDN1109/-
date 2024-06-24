using UnityEngine;
using System.IO.Ports; // �V���A���ʐM�p�̃��C�u������ǉ�

public class MoveObjectWithArduino : MonoBehaviour
{
    public float minX = 4.75f;  // X���W�̍ŏ��l
    public float maxX = 5.65f;  // X���W�̍ő�l
    public int serialPortNumber = 3;  // Arduino�̃V���A���|�[�g�ԍ�
    public int baudRate = 9600;  // Arduino�̃{�[���[�g

    private SerialPort serialPort;

    void Start()
    {
        // Arduino�Ƃ̃V���A���|�[�g��ݒ�
        string portName = "COM" + serialPortNumber;
        serialPort = new SerialPort(portName, baudRate);
        serialPort.Open(); // �V���A���|�[�g���J��
    }

    void Update()
    {
        if (serialPort.IsOpen)
        {
            try
            {
                // Arduino����l��ǂݎ��
                int sensorValue = int.Parse(serialPort.ReadLine());

                // �l��X���W�Ƀ}�b�s���O����myBox���ړ�������
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
        // �V���A���|�[�g�����
        if (serialPort != null && serialPort.IsOpen)
        {
            serialPort.Close();
        }
    }

    // �l���w�肳�ꂽ�͈͂Ƀ}�b�s���O����֐�
    float Map(float value, float fromSource, float toSource, float fromTarget, float toTarget)
    {
        return (value - fromSource) / (toSource - fromSource) * (toTarget - fromTarget) + fromTarget;
    }
}
