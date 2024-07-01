using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UniRx;
using System.IO.Ports;

public class SceneSwitcher : MonoBehaviour
{
    public AudioClip soundEffect; // 効果音ファイルを設定するための変数
    private AudioSource audioSource;
    private bool soundPlayed = false; // 効果音が再生されたかどうかのフラグ
    private SerialPort serialPort;

    void Start()
    {
        // AudioSourceコンポーネントを取得
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            // AudioSourceがアタッチされていない場合は新しく追加する
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // 効果音ファイルを設定
        audioSource.clip = soundEffect;

        // シリアルポートの初期化
        serialPort = new SerialPort("COM3", 9600); // 適切なポート名とボーレートを設定
        serialPort.Open();
        serialPort.ReadTimeout = 1000;

        // スペースキーの監視
        Observable.EveryUpdate()
            .Where(_ => Input.GetKeyDown(KeyCode.Space) && !soundPlayed)
            .Subscribe(_ => StartCoroutine(PlaySoundAndLoadNextScene()))
            .AddTo(this);

        // Arduinoからの信号を監視
        Observable.EveryUpdate()
            .Where(_ => serialPort.IsOpen)
            .Select(_ => ReadFromSerialPort())
            .Where(signal => (signal == "LEFT" || signal == "RIGHT" || signal == "SPACE") && !soundPlayed)
            .Subscribe(_ => StartCoroutine(PlaySoundAndLoadNextScene()))
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

    IEnumerator PlaySoundAndLoadNextScene()
    {
        // 効果音を再生
        audioSource.Play();
        soundPlayed = true; // 効果音が再生されたことを記録

        // 効果音が再生されている間待機
        while (audioSource.isPlaying)
        {
            yield return null;
        }

        // 現在のシーンのビルドインデックスを取得
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        // 次のシーンのビルドインデックス
        int nextSceneIndex = currentSceneIndex + 1;

        // シーンが存在するか確認してから遷移
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            Debug.Log("次のシーンが存在しません！");
        }
    }

    void OnDestroy()
    {
        if (serialPort != null && serialPort.IsOpen)
        {
            serialPort.Close();
        }
    }
}
