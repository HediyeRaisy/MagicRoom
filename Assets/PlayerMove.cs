using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float speed = 3f;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }


    private void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 dir = new Vector3(h, 0f, v);
        rb.MovePosition(transform.position + dir * speed * Time.deltaTime);

        // Debug.Log($"PlayerMove h={h} v={v}");
    }

}
