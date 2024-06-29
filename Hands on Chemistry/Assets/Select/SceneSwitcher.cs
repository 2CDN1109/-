using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneSwitcher : MonoBehaviour
{
    public AudioClip soundEffect; // ���ʉ��t�@�C����ݒ肷�邽�߂̕ϐ�
    private AudioSource audioSource;

    private bool soundPlayed = false; // ���ʉ����Đ����ꂽ���ǂ����̃t���O

    void Start()
    {
        // AudioSource�R���|�[�l���g���擾
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            // AudioSource���A�^�b�`����Ă��Ȃ��ꍇ�͐V�����ǉ�����
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // ���ʉ��t�@�C����ݒ�
        audioSource.clip = soundEffect;
    }

    void Update()
    {
        // �X�y�[�X�L�[�������ꂽ�Ƃ��̏���
        if (Input.GetKeyDown(KeyCode.Space) && !soundPlayed)
        {
            // �񓯊��Ō��ʉ����Đ����A���̌�V�[���J�ڂ��J�n
            StartCoroutine(PlaySoundAndLoadNextScene());
        }
    }

    IEnumerator PlaySoundAndLoadNextScene()
    {
        // ���ʉ����Đ�
        audioSource.Play();
        soundPlayed = true; // ���ʉ����Đ����ꂽ���Ƃ��L�^

        // ���ʉ����Đ�����Ă���ԑҋ@
        while (audioSource.isPlaying)
        {
            yield return null;
        }

        // ���݂̃V�[���̃r���h�C���f�b�N�X���擾
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        // ���̃V�[���̃r���h�C���f�b�N�X
        int nextSceneIndex = currentSceneIndex + 1;

        // �V�[�������݂��邩�m�F���Ă���J��
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            Debug.Log("���̃V�[�������݂��܂���I");
        }
    }
}
