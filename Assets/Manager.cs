using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    [Header("All snap scripts (7 pieces)")]
    public UISnapToPoint[] pieces;

    [Header("How many pieces to finish")]
    public int totalPieces = 7;

    [Header("End video controller")]
    public EndVideoController endVideo;

    private int snappedCount = 0;
    private bool finished = false;

    private void OnEnable()
    {
        // وصل شدن به 이벤트 OnSnapped هر قطعه
        if (pieces == null) return;

        foreach (var p in pieces)
        {
            if (p == null) continue;
            p.OnSnapped += HandlePieceSnapped;
        }
    }

    private void OnDisable()
    {
        // جدا کردن 이벤트 ها (مهم برای جلوگیری از دوبار شمارش)
        if (pieces == null) return;

        foreach (var p in pieces)
        {
            if (p == null) continue;
            p.OnSnapped -= HandlePieceSnapped;
        }
    }

    private void Start()
    {
        // اگر بعضی قطعه‌ها از قبل snapped بودند (مثلاً تست/لود)، بشمار
        snappedCount = 0;
        if (pieces != null)
        {
            foreach (var p in pieces)
                if (p != null && p.snapped) snappedCount++;
        }

        if (snappedCount >= totalPieces && !finished)
            FinishGame();
    }

    private void HandlePieceSnapped()
    {
        if (finished) return;

        snappedCount++;

        if (snappedCount >= totalPieces)
            FinishGame();
    }

    private void FinishGame()
    {
        finished = true;
        if (endVideo != null)
            endVideo.PlayVideo();
        else
            Debug.LogWarning("EndVideoController is not assigned!");
    }
}