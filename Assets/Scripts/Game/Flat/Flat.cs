using UnityEngine;
public class Flat : MonoBehaviour
{
    private const float defaultPauseButtonWidth = 125f;
    private const float defaultScreenWidth = 1920f;
    protected float yTopCoordinate, yBottomCoordinate;
    protected Camera mainCamera;
    private const float defaultScreenCoefficient = 62.5f;
    protected ScreenSide screenSide;
    protected Rigidbody2D body;
    protected virtual void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        mainCamera = Camera.main;
        var width = Screen.safeArea.width;
        var screenCoefficient = (defaultScreenCoefficient * (Screen.width / (float)Screen.height)) - defaultScreenCoefficient;
        var localScale = transform.localScale;
        var localScaleXHalf = localScale.x / 2f;
        var pauseButtonWidth = defaultPauseButtonWidth * (Screen.width / defaultScreenWidth);
        var xCoordinate = screenSide == ScreenSide.Right ? mainCamera.ScreenToWorldPoint(new Vector3(width - width / screenCoefficient - pauseButtonWidth, 0f)).x :
            mainCamera.ScreenToWorldPoint(new Vector3(width / screenCoefficient + (Screen.width - width + pauseButtonWidth), 0f)).x;
        if (xCoordinate > 0f)
            Preferences.DistanceBetweenFlats = xCoordinate * 2f;
        transform.position = new Vector2(screenSide == ScreenSide.Right ? xCoordinate - localScaleXHalf : xCoordinate + localScaleXHalf, 0f);
        yTopCoordinate = mainCamera.ScreenToWorldPoint(new Vector3(0f, Screen.height)).y - localScale.y / 2f;
        yBottomCoordinate = -yTopCoordinate;
    }
    protected virtual void Start()
    {
        BallController.Shared.OnExitFromTheField += delegate
        {
            var position = transform.position;
            position.y = 0f;
            transform.position = position;
        };
    }
}
