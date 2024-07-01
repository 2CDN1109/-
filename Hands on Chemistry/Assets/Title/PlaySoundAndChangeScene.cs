using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO.Ports;
using UniRx;
using System;

public class PlaySoundAndChangeScene : MonoBehaviour
{
    public AudioSource audioSource;
    public string sceneName;
    private bool hasPlayed = false;
    private SerialPort serialPort;

    void Start()
    {
        // シリアルポートの初期化
        serialPort = new SerialPort("COM3", 9600); // 適切なポート名とボーレートを設定
        serialPort.Open();
        serialPort.ReadTimeout = 1000;

        // スペースキーの監視
        Observable.EveryUpdate()
            .Where(_ => Input.GetKeyDown(KeyCode.Space) && !hasPlayed)
            .Subscribe(_ => StartCoroutine(PlaySoundAndLoadScene()))
            .AddTo(this);

        // Arduinoからの信号を監視
        Observable.EveryUpdate()
            .Where(_ => serialPort.IsOpen)
            .Select(_ => ReadFromSerialPort())
            .Where(signal => (signal == "LEFT" || signal == "RIGHT" || signal == "SPACE") && !hasPlayed)
            .Subscribe(_ => StartCoroutine(PlaySoundAndLoadScene()))
            .AddTo(this);
    }

    private string ReadFromSerialPort()
    {
        try
        {
            return serialPort.ReadLine();
        }
        catch (System.Exception)
        {
            return string.Empty;
        }
    }

    private IEnumerator PlaySoundAndLoadScene()
    {
        hasPlayed = true;
        audioSource.Play();
        yield return new WaitForSeconds(audioSource.clip.length);
        SceneManager.LoadScene("Select");
    }

    void OnDestroy()
    {
        if (serialPort != null && serialPort.IsOpen)
        {
            serialPort.Close();
        }
    }
}
