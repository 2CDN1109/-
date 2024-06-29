using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneSwitcher : MonoBehaviour
{
    public AudioClip soundEffect; // 効果音ファイルを設定するための変数
    private AudioSource audioSource;

    private bool soundPlayed = false; // 効果音が再生されたかどうかのフラグ

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
    }

    void Update()
    {
        // スペースキーが押されたときの処理
        if (Input.GetKeyDown(KeyCode.Space) && !soundPlayed)
        {
            // 非同期で効果音を再生し、その後シーン遷移を開始
            StartCoroutine(PlaySoundAndLoadNextScene());
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
}
