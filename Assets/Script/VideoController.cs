using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class VideoController : MonoBehaviour
{
    private VideoPlayer videoPlayer;

    void Start()
    {
        videoPlayer = GetComponent<VideoPlayer>();
        videoPlayer.loopPointReached += EndReached; // เรียกใช้ฟังก์ชันเมื่อวิดีโอจบ
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (videoPlayer.isPlaying)
            {
                videoPlayer.Pause();
            }
            else
            {
                videoPlayer.Play();
            }
        }
    }

    void EndReached(VideoPlayer vp)
    {
        SceneManager.LoadScene("LV1");
    }
}
