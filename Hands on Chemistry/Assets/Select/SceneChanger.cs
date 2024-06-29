using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public AudioClip soundEffect; // 効果音のオーディオクリップ
    private AudioSource audioSource;

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            PlaySoundAndChangeScene();
        }
    }

    void PlaySoundAndChangeScene()
    {
        audioSource.PlayOneShot(soundEffect);
        Invoke("ChangeScene", soundEffect.length);
    }

    void ChangeScene()
    {
        SceneManager.LoadScene("Re-Mogura"); // 遷移するシーンの名前を指定
    }
}
