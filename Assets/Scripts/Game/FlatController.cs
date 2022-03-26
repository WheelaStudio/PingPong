using UnityEngine;
public class FlatController : MonoBehaviour
{
    private Rigidbody2D body;
    [SerializeField] private bool isRight;
    private const float sensitivity = 0.005f;
    private float yTopCoordinate, yBottomCoordinate;
    private void Start()
    {
        body = GetComponent<Rigidbody2D>();
        var camera = Camera.main;
        var xCoordinate = isRight ? camera.ScreenToWorldPoint(new Vector3(Screen.width,0f)).x : camera.ScreenToWorldPoint(Vector3.zero).x;
        var localScale = transform.localScale;
        yTopCoordinate = camera.ScreenToWorldPoint(new Vector3(0f, Screen.height)).y - localScale.y / 2f;
        yBottomCoordinate = -yTopCoordinate;
        transform.position = new Vector2(isRight ? xCoordinate - localScale.x / 2f : xCoordinate + localScale.x / 2f, 0f);
    }
    private void FixedUpdate()
    {
        if (Input.touchCount > 0)
        {
            foreach (var touch in Input.touches)
            {
                if (touch.position.x > Screen.width / 2 && isRight || touch.position.x < Screen.width / 2 && !isRight)
                {
                    var nextPosition = body.position + touch.deltaPosition.y * sensitivity * Vector2.up;
                    if (nextPosition.y < yBottomCoordinate || nextPosition.y > yTopCoordinate) return;
                    body.MovePosition(nextPosition);
                    break;
                }
            }
        }
    }
}
