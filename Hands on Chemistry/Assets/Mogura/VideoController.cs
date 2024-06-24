using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;

public class VideoController : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public RawImage rawImage;

    void Start()
    {
        // Ensure the render texture is assigned to the RawImage
        rawImage.texture = videoPlayer.targetTexture;

        // Start video playback
        if (videoPlayer != null)
        {
            videoPlayer.Play();
        }
        else
        {
            Debug.LogError("VideoPlayer component is not assigned.");
        }
    }
}

