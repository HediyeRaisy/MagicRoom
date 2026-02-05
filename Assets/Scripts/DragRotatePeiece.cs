// using UnityEngine;
// using UnityEngine.EventSystems;

// public class DragRotatePeiece : MonoBehaviour,
//     IPointerDownHandler, IPointerUpHandler, IDragHandler
// {
//     public float rotationStep = 45f;   // degrees per click or key
//     private RectTransform rect;
//     private bool isSelected = false;
//     public UISnapToPoint snap;

//     private void Awake()
//     {
//         rect = GetComponent<RectTransform>();
//         snap = GetComponent<UISnapToPoint>();
//     }

//     public void OnPointerDown(PointerEventData eventData)
//     {
//         if (snap != null && snap.snapped) return;
       
//         isSelected = true;
//     }

//     public void OnPointerUp(PointerEventData eventData)
//     {
//         isSelected = false;
//     }

//     public void OnDrag(PointerEventData eventData)
//     {
//         if (snap != null && snap.snapped) return;
//         rect.position = eventData.position;
//     }

//     // you can keep this or delete it if you don't want Q/E anymore
//     private void Update()
//     {
//         // Debug.Log(snap.snapped + "Snap.snapped");
//         if (snap != null && snap.snapped) return;
//         if (!isSelected) return;

//         if (Input.GetKeyDown(KeyCode.Q))
//             RotateLeft();

//         if (Input.GetKeyDown(KeyCode.E))
//             RotateRight();
//     }

//     public void RotateLeft()
//     {
//         if (!isSelected) return;
//         if (snap != null && snap.snapped) return;

//         rect.Rotate(0f, 0f, rotationStep);
//     }

//     public void RotateRight()
//     {
//         if (!isSelected) return;
//         if (snap != null && snap.snapped) return;

//         rect.Rotate(0f, 0f, -rotationStep);
//     }

//     public void FlipHorizontal()
//     {
//         if (!isSelected) return;
//         if (snap != null && snap.snapped) return;

//         Vector3 s = rect.localScale;
//         s.x *= -1;
//         rect.localScale = s;
//     }
// }

using UnityEngine;
using UnityEngine.EventSystems;

public class DragRotatePiece : MonoBehaviour,
    IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public float rotationStep = 45f;

    public UISnapToPoint snap;
    public AudioClip rotateSound;
    public RectTransform rect;

    public static DragRotatePiece selectedPiece;

    private bool isDragging = false;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        if (snap == null)
            snap = GetComponent<UISnapToPoint>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (snap != null && snap.snapped) return;

        selectedPiece = this;
        isDragging = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isDragging = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isDragging) return;
        if (snap != null && snap.snapped) return;

        rect.position = eventData.position;
    }

    public void RotateLeft()
    {
        if (snap != null && snap.snapped) return;
        rect.Rotate(0, 0, rotationStep);
        if (rotateSound != null)
        {
            AudioSource.PlayClipAtPoint(rotateSound, Camera.main.transform.position);
        }
    }

    public void RotateRight()
    {
        if (snap != null && snap.snapped) return;
        rect.Rotate(0, 0, -rotationStep);
        if (rotateSound != null)
        {
            AudioSource.PlayClipAtPoint(rotateSound, Camera.main.transform.position);
        }
    }

    public void FlipVertical()
    {
        if (snap != null && snap.snapped) return;

        Vector3 s = rect.localScale;
        s.y *= -1;
        rect.localScale = s;
        if (rotateSound != null)
        {
            AudioSource.PlayClipAtPoint(rotateSound, Camera.main.transform.position);
        }
    }
}