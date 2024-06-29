using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public AudioClip soundEffect; // ���ʉ��̃I�[�f�B�I�N���b�v
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
        SceneManager.LoadScene("Re-Mogura"); // �J�ڂ���V�[���̖��O���w��
    }
}
