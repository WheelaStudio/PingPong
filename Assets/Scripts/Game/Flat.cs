using UnityEngine;
public class Flat : MonoBehaviour
{
#if !UNITY_STANDALONE && !UNITY_EDITOR
    private const float defaultSensitivity = 0.000875f;
#endif
    protected float sensitivity;
#if UNITY_STANDALONE || UNITY_EDITOR
    private const float PCsensitivity = 0.02f;
#endif
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
#if !UNITY_STANDALONE && !UNITY_EDITOR
        sensitivity = defaultSensitivity * Preferences.ScreenInch * Preferences.SensitivityCoefficient;
#endif
#if UNITY_STANDALONE || UNITY_EDITOR
        sensitivity = PCsensitivity * Preferences.ScreenInch * Preferences.SensitivityCoefficient;
#endif
        var width = Screen.safeArea.width;
        var screenCoefficient = (defaultScreenCoefficient * (Screen.width / (float)Screen.height)) - defaultScreenCoefficient;
        var localScale = transform.localScale;
        var localScaleXHalf = localScale.x / 2f;
        var pauseButtonWidth = defaultPauseButtonWidth * (Screen.width / defaultScreenWidth);
        var xCoordinate = screenSide == ScreenSide.Right ? mainCamera.ScreenToWorldPoint(new Vector3(width - width / screenCoefficient - pauseButtonWidth, 0f)).x :
            mainCamera.ScreenToWorldPoint(new Vector3(width / screenCoefficient + (Screen.width - width + pauseButtonWidth), 0f)).x;
        if (xCoordinate > 0f)
            BallController.Shared.distanceBetweenFlats = xCoordinate * 2f;
        transform.position = new Vector2(screenSide == ScreenSide.Right ? xCoordinate - localScaleXHalf : xCoordinate + localScaleXHalf, 0f);
        yTopCoordinate = mainCamera.ScreenToWorldPoint(new Vector3(0f, Screen.height)).y - localScale.y / 2f;
        yBottomCoordinate = -yTopCoordinate;
    }
}
