using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;

public class CompletionVideoController : MonoBehaviour
{
    public static CompletionVideoController Instance;

    [Header("Assign in Inspector")]
    public VideoPlayer videoPlayer;
    public GameObject videoScreenRawImageObject; // the RawImage GameObject (not the component)
    public AudioSource clapAudioSource;


    private void Awake()
    {
        Instance = this;

        // Hide at start
        if (videoScreenRawImageObject != null)
            videoScreenRawImageObject.SetActive(false);

        if (videoPlayer != null)
            videoPlayer.Stop();
    }

    public void PlayFullScreen()
    {
        // if (videoScreenRawImageObject != null)
        //     videoScreenRawImageObject.SetActive(true);

        // if (videoPlayer != null)
        // {
        //     videoPlayer.Stop(); // restart from beginning
        //     videoPlayer.Play();
        // }
        // else
        // {
        //     Debug.LogWarning("CompletionVideoController: VideoPlayer is not assigned!");
        // }
        // Hide gameplay if you do that
    if (videoScreenRawImageObject != null)
            videoScreenRawImageObject.SetActive(true);

    // Show video
    if (videoScreenRawImageObject != null)
        videoScreenRawImageObject.SetActive(true);

    // 🔊 Play clap in parallel
    if (clapAudioSource != null)
        clapAudioSource.Play();

    // 🎬 Play video
    if (videoPlayer != null)
    {
        videoPlayer.Stop();
        videoPlayer.Play();
    }
    }
}
