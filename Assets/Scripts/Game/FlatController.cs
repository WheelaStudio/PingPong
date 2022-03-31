using UnityEngine;
public class FlatController : MonoBehaviour
{
    private Rigidbody2D body;
    [SerializeField] private ScreenSide side;
#if !UNITY_STANDALONE
    private const float defaultSensitivity = 0.000875f;
    private float sensitivity;
#endif
#if UNITY_STANDALONE || UNITY_EDITOR
    private const float PCsensitivity = 0.2f;
#endif
    private const float defaultPauseButtonWidth = 125f;
    private const float defaultScreenWidth = 1920f;
    private const float defaultScreenCoefficient = 62.5f;
    private float yTopCoordinate, yBottomCoordinate;
    private Camera mainCamera;
    private void Start()
    {
        body = GetComponent<Rigidbody2D>();
        mainCamera = Camera.main;
#if! UNITY_STANDALONE
        sensitivity = defaultSensitivity * Preferences.ScreenInch * Preferences.SensitivityCoefficient;
#endif
#if UNITY_STANDALONE || UNITY_EDITOR
        // sensitivity = PCsensitivity * Preferences.ScreenInch * Preferences.SensitivityCoefficient;
#endif
        var width = Screen.safeArea.width;
        var screenCoefficient = (defaultScreenCoefficient * (Screen.width / (float)Screen.height)) - defaultScreenCoefficient;
        var localScale = transform.localScale;
        var localScaleXHalf = localScale.x / 2f;
        var pauseButtonWidth = defaultPauseButtonWidth * (Screen.width / defaultScreenWidth);
        var xCoordinate = side == ScreenSide.Right ? mainCamera.ScreenToWorldPoint(new Vector3(width - width / screenCoefficient - pauseButtonWidth, 0f)).x :
            mainCamera.ScreenToWorldPoint(new Vector3(width / screenCoefficient + (Screen.width - width + pauseButtonWidth), 0f)).x;
        transform.position = new Vector2(side == ScreenSide.Right ? xCoordinate - localScaleXHalf : xCoordinate + localScaleXHalf, 0f);
        yTopCoordinate = mainCamera.ScreenToWorldPoint(new Vector3(0f, Screen.height)).y - localScale.y / 2f;
        yBottomCoordinate = -yTopCoordinate;
    }
    private void FixedUpdate()
    {
        Vector2 nextPosition;
#if !UNITY_STANDALONE
        if (Input.touchCount > 0)
        {
            foreach (var touch in Input.touches)
            {
                if (touch.position.x > Screen.width / 2 && side == ScreenSide.Right || touch.position.x < Screen.width / 2 && side == ScreenSide.Left)
                {
                    nextPosition = body.position + touch.deltaPosition.y * sensitivity * Vector2.up;
                    if (nextPosition.y < yBottomCoordinate || nextPosition.y > yTopCoordinate) return;
                    body.MovePosition(nextPosition);
                    break;
                }
            }
        }
#endif
#if UNITY_STANDALONE || UNITY_EDITOR
        var direction = 0f;
        if (side == ScreenSide.Left)
        {
            if (Input.GetKey(KeyCode.W))
                direction = 1f;
            else if (Input.GetKey(KeyCode.S))
                direction = -1f;
        }
        else if (side == ScreenSide.Right)
        {
            if (Input.GetKey(KeyCode.UpArrow))
                direction = 1f;
            else if (Input.GetKey(KeyCode.DownArrow))
                direction = -1f;
        }
        nextPosition = body.position + direction * PCsensitivity * Vector2.up;
        if (nextPosition.y < yBottomCoordinate || nextPosition.y > yTopCoordinate) return;
        body.MovePosition(nextPosition);
#endif
    }

}
