using UnityEngine;

public class CheerSound : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip cheerClip;

    public void PlayCheer()
    {
        audioSource.PlayOneShot(cheerClip);
        Debug.Log("CHEER PLAYED");
        audioSource.PlayOneShot(cheerClip);
    }
    
}