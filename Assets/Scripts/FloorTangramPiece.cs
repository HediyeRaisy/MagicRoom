using UnityEngine;

public class FloorTangramPiece : MonoBehaviour
{
    [Tooltip("Name of this piece, e.g. Triangle1")]
    public string pieceId;

    [Header("Visible shape on the floor (e.g. square)")]
    public GameObject floorVisual;

    [Tooltip("UI prefab of this piece")]
    public GameObject uiPiecePrefab;

    [Tooltip("Parent transform (Canvas) for UI piece")]
    public Transform uiParent;

    [Tooltip("Seconds player must stay on this piece")]
    public float requiredTime = 5f;

    private float timer = 0f;
    private bool spawned = false;
    public AudioClip appearSound;
    private void OnTriggerStay(Collider other)
    {
        if (spawned) return;

        if (other.CompareTag("Player"))
        {
            timer += Time.deltaTime;

            if (timer >= requiredTime)
            {
                SpawnUIPiece();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            timer = 0f;
        }
    }

    private void SpawnUIPiece()
    {
        spawned = true;

        if (uiPiecePrefab != null && uiParent != null)
        {
            GameObject uiPiece = Instantiate(uiPiecePrefab, uiParent);
            var snap = uiPiece.GetComponent<UISnapToPoint>();
            
            if (snap != null)
            {
                snap.floorVisualToHide = floorVisual;   // ✅ این قطعه‌ی زمین مربوط به همین UI است
            }
            
            
            GameObject[] pieces = GameObject.FindGameObjectsWithTag("map");
            GameObject target = null;

            foreach (GameObject obj in pieces)
            {
                
                if (obj.name == uiPiece.name.Replace("(Clone)", "").Trim())
                {
                    target = obj;
                    break;
                }
            }
            if (appearSound != null)
            {
                AudioSource.PlayClipAtPoint(appearSound, Camera.main.transform.position);
            }

            uiPiece.GetComponent<UISnapToPoint>().snapTarget = target.GetComponent<RectTransform>();
            if (snap != null)
            {
                snap.OnSnapped += HideOnlyThisFloorPiece;
            }
            // HideFloorPiece();

            // RectTransform rt = uiPiece.GetComponent<RectTransform>();
            // if (rt != null)
            // {
            //     rt.anchoredPosition = Vector2.zero; // center of canvas
            // }
        }
        else
        {
            Debug.LogWarning("FloorTangramPiece: uiPiecePrefab or uiParent not set!");
        }
    }
    private void HideOnlyThisFloorPiece()
    {
        // خاموش کردن همه رندررها (MeshRenderer, SpriteRenderer, ...)
        foreach (var rend in GetComponentsInChildren<Renderer>(true))
            rend.enabled = false;

        // خاموش کردن collider های 3D
        foreach (var col in GetComponentsInChildren<Collider>(true))
            col.enabled = false;

        // اگر از Collider2D استفاده می‌کنی
        foreach (var col2d in GetComponentsInChildren<Collider2D>(true))
            col2d.enabled = false;

        Debug.Log("Floor piece hidden: " + gameObject.name);
    }
}
// using UnityEngine;

// public class FloorTangramPiece : MonoBehaviour
// {
//     public GameObject uiPiecePrefab;
//     public Transform uiParent;
//     public float requiredTime = 5f;

//     [Header("THIS is the floor object you want to disappear (the visible shape)")]
//     public GameObject floorVisualToHide;   // <-- drag "square" here in Inspector

//     public AudioClip appearSound;

//     private float timer = 0f;
//     private bool spawned = false;

//     private void OnTriggerStay(Collider other)
//     {
//         if (spawned) return;

//         if (other.CompareTag("Player"))
//         {
//             timer += Time.deltaTime;
//             if (timer >= requiredTime)
//                 SpawnUIPiece();
//         }
//     }

//     private void OnTriggerExit(Collider other)
//     {
//         if (other.CompareTag("Player"))
//             timer = 0f;
//     }

//     private void SpawnUIPiece()
//     {
//         spawned = true;

//         if (uiPiecePrefab == null || uiParent == null)
//         {
//             Debug.LogWarning("FloorTangramPiece: uiPiecePrefab or uiParent not set!");
//             return;
//         }

//         GameObject uiPiece = Instantiate(uiPiecePrefab, uiParent);

//         // 1) set snap target for this UI piece (your existing logic)
//         var snap = uiPiece.GetComponent<UISnapToPoint>();
//         if (snap == null)
//         {
//             Debug.LogWarning("UI piece has no UISnapToPoint component!");
//             return;
//         }

//         // Find the matching snap point on wall by name:
//         // UI prefab name is snap_square, snap_triangle_yellow, etc.
//         // The snapTarget should be the object with tag "map" and SAME name as the UI object (without (Clone)).
//         GameObject[] targets = GameObject.FindGameObjectsWithTag("map");
//         GameObject target = null;

//         string uiName = uiPiece.name.Replace("(Clone)", "").Trim();

//         foreach (GameObject obj in targets)
//         {
//             if (obj.name.Trim() == uiName)
//             {
//                 target = obj;
//                 break;
//             }
//         }

//         if (target == null)
//         {
//             Debug.LogWarning("Could not find snap target with name: " + uiName + " (tag=map)");
//             return;
//         }

//         snap.snapTarget = target.GetComponent<RectTransform>();

//         // 2) play appear sound
//         if (appearSound != null && Camera.main != null)
//             AudioSource.PlayClipAtPoint(appearSound, Camera.main.transform.position);

//         // 3) IMPORTANT: when UI snaps, hide ONLY this floor piece's visual
//         snap.OnSnapped += HideFloorVisualOnly;
//     }

//     private void HideFloorVisualOnly()
//     {
//         if (floorVisualToHide == null)
//         {
//             Debug.LogWarning("floorVisualToHide is not assigned on: " + gameObject.name);
//             return;
//         }

//         // Hide renderers (MeshRenderer, SpriteRenderer, etc.)
//         foreach (var rend in floorVisualToHide.GetComponentsInChildren<Renderer>(true))
//             rend.enabled = false;

//         // If your floor visual is UI/Canvas (rare) you can also disable it by SetActive(false)
//         // floorVisualToHide.SetActive(false);

//         Debug.Log("Hidden floor visual: " + floorVisualToHide.name);
//     }
// }

