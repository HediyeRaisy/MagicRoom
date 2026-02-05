using UnityEngine;
using UnityEngine.Video;

public class EndVideoController : MonoBehaviour
{
    public GameObject endVideoPanel;   // پنل/RawImage روی Canvas
    public VideoPlayer player;         // VideoPlayer

    public void PlayVideo()
    {
        endVideoPanel.SetActive(true);
        player.Play();
    }
}