using UnityEngine;

public class PieceButtonsUI : MonoBehaviour
{
    [Header("Follow selected piece")]
    public Vector2 offset = new Vector2(150f, 0f); // فاصله‌ی دکمه‌ها از قطعه

    private RectTransform panelRect;

    private void Awake()
    {
        panelRect = GetComponent<RectTransform>();
    }
    // private void Awake()
    // {
    // panelRect = GetComponent<RectTransform>();
    // panelRect.gameObject.SetActive(false);  
    // }

    private void LateUpdate()
    {
        if (DragRotatePiece.selectedPiece == null)
        {
            panelRect.gameObject.SetActive(false);
            return;
        }

        panelRect.gameObject.SetActive(true);

        RectTransform pieceRect = DragRotatePiece.selectedPiece.rect;

        panelRect.position = pieceRect.position + (Vector3)offset;

        panelRect.rotation = Quaternion.identity;
    }

    

    public void OnRotateLeftClicked()
    {
        if (DragRotatePiece.selectedPiece == null) return;
        DragRotatePiece.selectedPiece.RotateLeft();
    }

    public void OnRotateRightClicked()
    {
        if (DragRotatePiece.selectedPiece == null) return;
        DragRotatePiece.selectedPiece.RotateRight();
    }

    public void OnFlipClicked()
    {
        if (DragRotatePiece.selectedPiece == null) return;
        DragRotatePiece.selectedPiece.FlipVertical();
    }
}
