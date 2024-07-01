using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UniRx;
using System.IO.Ports;

public class SceneSwitcher : MonoBehaviour
{
    public AudioClip soundEffect; // ���ʉ��t�@�C����ݒ肷�邽�߂̕ϐ�
    private AudioSource audioSource;
    private bool soundPlayed = false; // ���ʉ����Đ����ꂽ���ǂ����̃t���O
    private SerialPort serialPort;

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

        // �V���A���|�[�g�̏�����
        serialPort = new SerialPort("COM3", 9600); // �K�؂ȃ|�[�g���ƃ{�[���[�g��ݒ�
        serialPort.Open();
        serialPort.ReadTimeout = 1000;

        // �X�y�[�X�L�[�̊Ď�
        Observable.EveryUpdate()
            .Where(_ => Input.GetKeyDown(KeyCode.Space) && !soundPlayed)
            .Subscribe(_ => StartCoroutine(PlaySoundAndLoadNextScene()))
            .AddTo(this);

        // Arduino����̐M�����Ď�
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

    void OnDestroy()
    {
        if (serialPort != null && serialPort.IsOpen)
        {
            serialPort.Close();
        }
    }
}
