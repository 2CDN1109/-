using System;
using System.IO.Ports;
using UniRx;
using UnityEngine;

public class SerialHandler : MonoBehaviour
{
    public string portName = "COM4"; // 使用するポートを設定
    public int baudRate = 9600; // Arduinoのボーレートを設定

    private SerialPort serialPort;
    private Subject<string> messageSubject = new Subject<string>();

    public IObservable<string> OnMessageReceived => messageSubject;

    void Start()
    {
        Open();
        Observable.EveryUpdate().Subscribe(_ => ReadSerial());
    }

    void OnDestroy()
    {
        Close();
    }

    private void Open()
    {
        try
        {
            serialPort = new SerialPort(portName, baudRate);
            serialPort.Open();
            Debug.Log("Serial port opened.");
        }
        catch (Exception e)
        {
            Debug.LogError("Failed to open serial port: " + e.Message);
        }
    }

    private void Close()
    {
        if (serialPort != null && serialPort.IsOpen)
        {
            serialPort.Close();
            serialPort = null;
            Debug.Log("Serial port closed.");
        }
    }

    private void ReadSerial()
    {
        if (serialPort != null && serialPort.IsOpen && serialPort.BytesToRead > 0)
        {
            try
            {
                string message = serialPort.ReadLine();
                messageSubject.OnNext(message);
            }
            catch (Exception e)
            {
                Debug.LogError("Error reading serial port: " + e.Message);
            }
        }
    }
}
