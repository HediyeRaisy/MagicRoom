using UnityEngine;

public class PuzzleCompleteManager : MonoBehaviour
{
    public UISnapToPoint[] pieces;
    public AudioSource sfxSource;
    public AudioClip cheerClip;

    private int snappedCount = 0;
    private bool done = false;

    private void Start()
    {
        for (int i = 0; i < pieces.Length; i++)
        {
            if (pieces[i] == null) continue;

            if (pieces[i].snapped) snappedCount++;
            pieces[i].OnSnapped += OnPieceSnapped;
        }

        CheckComplete();
    }

    private void OnPieceSnapped()
    {
        if (done) return;

        snappedCount++;
        Debug.Log($"SNAPPED COUNT: {snappedCount}/{pieces.Length}");
        CheckComplete();
    }

    private void CheckComplete()
    {
        if (done) return;

        if (snappedCount >= pieces.Length)
        {
            done = true;
            Debug.Log("PUZZLE COMPLETE!");

            if (sfxSource != null && cheerClip != null)
                sfxSource.PlayOneShot(cheerClip);
        }
    }
}