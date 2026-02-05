using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonClickFeedback : MonoBehaviour,
    IPointerDownHandler, IPointerUpHandler
{
    public float pressedScale = 0.9f;
    public float speed = 12f;

    Vector3 startScale;

    void Awake()
    {
        startScale = transform.localScale;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        transform.localScale = startScale * pressedScale;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        transform.localScale = startScale;
    }
}
