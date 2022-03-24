using UnityEngine;
public class FlatController : MonoBehaviour
{
    private Rigidbody2D body;
    [SerializeField] private bool isRight;
    private const float sensitivity = 0.005f;
    private void Start()
    {
        body = GetComponent<Rigidbody2D>();
    }
    private void FixedUpdate()
    {
        if (Input.touchCount > 0)
        {
            var last = Input.touches[Input.touchCount - 1];
            if (last.position.x > Screen.width / 2 && isRight || last.position.x < Screen.width / 2 && !isRight)
                body.MovePosition(body.position + Vector2.up * last.deltaPosition.y * sensitivity);
        }
    }
}
