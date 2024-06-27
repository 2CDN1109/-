using UnityEngine;
using UnityEditor;

public class AudioListenerFinder : MonoBehaviour
{
    // エディターのメニューにオプションを追加
    [MenuItem("Tools/Find Audio Listeners")]
    public static void FindAudioListeners()
    {
        // シーン内のすべてのAudio Listenerを取得
        AudioListener[] audioListeners = FindObjectsOfType<AudioListener>();

        // 各Audio Listenerがアタッチされているオブジェクトを表示
        foreach (AudioListener listener in audioListeners)
        {
            Debug.Log("Audio Listener found on: " + listener.gameObject.name, listener.gameObject);
        }
    }
}
