using UnityEngine;
using UnityEditor;

public class AudioListenerFinder : MonoBehaviour
{
    // �G�f�B�^�[�̃��j���[�ɃI�v�V������ǉ�
    [MenuItem("Tools/Find Audio Listeners")]
    public static void FindAudioListeners()
    {
        // �V�[�����̂��ׂĂ�Audio Listener���擾
        AudioListener[] audioListeners = FindObjectsOfType<AudioListener>();

        // �eAudio Listener���A�^�b�`����Ă���I�u�W�F�N�g��\��
        foreach (AudioListener listener in audioListeners)
        {
            Debug.Log("Audio Listener found on: " + listener.gameObject.name, listener.gameObject);
        }
    }
}
