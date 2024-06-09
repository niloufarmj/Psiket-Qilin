using UnityEngine;
using UnityEngine.Video;

public class VideoBackground : MonoBehaviour
{
    public string videoFileName;

    private void Start()
    {
        PlayVideo();
    }

    public void PlayVideo()
    {
        VideoPlayer vp = GetComponent<VideoPlayer>();

        if (vp)
        {
            string videoPath = System.IO.Path.Combine(Application.streamingAssetsPath, videoFileName);
            vp.url = videoPath;
            vp.Play();
        }
    }

}
