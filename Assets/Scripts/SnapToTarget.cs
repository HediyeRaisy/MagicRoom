using UnityEngine;
using UnityEngine.Video;

public class UISnapToPoint : MonoBehaviour
{
    [Header("Target position for this piece")]
    public RectTransform snapTarget;    // assign in Inspector

    [Header("How close before snapping?")]
    public float snapDistance = 30f;

    [Header("Floor visual to hide when snapped")]
    public GameObject floorVisualToHide;
    //
    public static int snappedCount = 0;     // counts across all pieces
    public static bool videoPlayed = false; // prevents multiple triggers

    [Header("Completion Video")]
    public VideoPlayer completionVideo;     // assign in Inspector OR auto-find

    private bool countedThisPiece = false;
    //





    // [HideInInspector] 
    public bool snapped = false;        // other scripts can read this
    public AudioClip snapSuccessSound;
    public System.Action OnSnapped;
    private RectTransform rect;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    private void Update()
    {
        if (snapped) return;
        if (snapTarget == null) return;

        // distance between piece and target (in UI space)
        float dist = Vector2.Distance(rect.position, snapTarget.position);
        // Debug.Log(dist + " distance to target" + gameObject.name);
    //    Debug.Log(snapTarget.eulerAngles + " target rotation");        
        float angle1 = rect.eulerAngles.z;
        float angle2 = snapTarget.transform.GetChild(0).eulerAngles.z;
        float delta = Mathf.Abs(Mathf.DeltaAngle(angle1, angle2));
        if (dist <= snapDistance &&  delta < 10f)
        {
            
            rect.position = snapTarget.position;
            rect.eulerAngles = snapTarget.transform.GetChild(0).eulerAngles;
            snapped = true;
            //
            // ✅ COUNT + PLAY VIDEO WHEN DONE
        // if (!countedThisPiece)
        // {
        //     countedThisPiece = true;
        //     snappedCount++;

        //     if (!videoPlayed && snappedCount >= 7)
        //     {
        //         videoPlayed = true;

        //         if (completionVideo == null)
        //         {
        //             // optional: auto-find if you didn't assign it
        //             completionVideo = GameObject.Find("CompletionVideoPlayer")?.GetComponent<VideoPlayer>();

        //         }

        //         if (completionVideo != null)
        //             completionVideo.Play();
        //         else
        //             Debug.LogWarning("Completion VideoPlayer not found/assigned!");
        //     }
        // }
                    if (!countedThisPiece)
            {
                countedThisPiece = true;
                snappedCount++;

                if (!videoPlayed && snappedCount >= 7)
                {
                    Transform frontCanvas = GameObject.Find("Front").transform;

                    foreach (Transform child in frontCanvas)
                    {
                        if (child.name.EndsWith("(Clone)"))
                        {
                            Destroy(child.gameObject);
                        }
                    }

                    videoPlayed = true;

                    if (CompletionVideoController.Instance != null)
                        CompletionVideoController.Instance.PlayFullScreen();
                    else
                        Debug.LogWarning("CompletionVideoController.Instance not found in scene!");
                }
            }

            //
            if (floorVisualToHide != null)
            {
                foreach (var r in floorVisualToHide.GetComponentsInChildren<Renderer>(true))
                    r.enabled = false;
            }

            if (snapSuccessSound != null)
            {
                AudioSource.PlayClipAtPoint(snapSuccessSound, Camera.main.transform.position);
            }
            OnSnapped?.Invoke();
            Debug.Log("SNAPPED: " + gameObject.name);
        }

    }
}

// using UnityEngine;

// public class UISnapToPoint : MonoBehaviour
// {
//     [Header("Target position for this piece (UI snap point)")]
//     public RectTransform snapTarget;

//     [Header("How close before snapping? (pixels)")]
//     public float snapDistance = 30f;

//     [Header("Rotation tolerance (degrees)")]
//     public float angleTolerance = 10f;

//     public bool snapped = false;

//     [Header("Sounds")]
//     public AudioClip snapSuccessSound;

//     // IMPORTANT: callback to floor piece
//     public System.Action OnSnapped;

//     private RectTransform rect;

//     private void Awake()
//     {
//         rect = GetComponent<RectTransform>();
//     }

//     private void Update()
//     {
//         if (snapped) return;
//         if (snapTarget == null) return;

//         float dist = Vector2.Distance(rect.position, snapTarget.position);

//         float angle1 = rect.eulerAngles.z;
//         float angle2 = snapTarget.eulerAngles.z; // ✅ no GetChild
//         float delta = Mathf.Abs(Mathf.DeltaAngle(angle1, angle2));

//         if (dist <= snapDistance && delta <= angleTolerance)
//         {
//             rect.position = snapTarget.position;
//             rect.eulerAngles = new Vector3(0f, 0f, angle2);

//             snapped = true;

//             if (snapSuccessSound != null && Camera.main != null)
//                 AudioSource.PlayClipAtPoint(snapSuccessSound, Camera.main.transform.position);

//             OnSnapped?.Invoke();
//         }
//     }
// }


