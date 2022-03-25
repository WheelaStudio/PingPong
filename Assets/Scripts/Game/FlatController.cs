using UnityEngine;
public class FlatController : MonoBehaviour
{
    private Rigidbody2D body;
    [SerializeField] private bool isRight;
    private const float sensitivity = 0.005f;
    private void Start()
    {
        body = GetComponent<Rigidbody2D>();
        var xCoordinate = isRight ? Camera.main.ScreenToWorldPoint(new Vector3(Screen.width,0f)).x : Camera.main.ScreenToWorldPoint(Vector3.zero).x;
        transform.position = new Vector2(isRight ? xCoordinate - 0.175f : xCoordinate + 0.175f, 0f);
    }
    private void FixedUpdate()
    {
        if (Input.touchCount > 0)
        {
            foreach (var touch in Input.touches)
            {
                if (touch.position.x > Screen.width / 2 && isRight || touch.position.x < Screen.width / 2 && !isRight)
                {
                    body.MovePosition(body.position + touch.deltaPosition.y * sensitivity * Vector2.up);
                    break;
                }
            }
        }
    }
}
