using UnityEngine;
public class FlatController : Flat
{
#if !UNITY_STANDALONE && !UNITY_EDITOR
    private const float defaultSensitivity = 0.000875f;
#endif
#if UNITY_STANDALONE || UNITY_EDITOR
    private const float defaultSensitivity = 0.01f;
#endif
    protected float sensitivity;
    [SerializeField] private ScreenSide side;
    [SerializeField] private bool useFullScreenForControl;
    protected override void Awake()
    {
        var gameMode = Preferences.GameMode;
        if (gameMode == GameMode.WithBot)
        {
            screenSide = Preferences.PlayerSide;
            if (screenSide == ScreenSide.Right)
                transform.eulerAngles = new Vector3(0f, 0f, 180f);
        }
        else
            screenSide = side;
        sensitivity = defaultSensitivity * Preferences.ScreenInch * Preferences.SensitivityCoefficient;
        base.Awake();
    }
    private void FixedUpdate()
    {
        Vector2 nextPosition;
#if !UNITY_STANDALONE
        if (Input.touchCount > 0)
        {
            foreach (var touch in Input.touches)
            {
                if (useFullScreenForControl || touch.position.x > Screen.width / 2 && side == ScreenSide.Right || touch.position.x < Screen.width / 2 && side == ScreenSide.Left)
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
        if (side == ScreenSide.Left || useFullScreenForControl)
        {
            if (Input.GetKey(KeyCode.W))
                direction = 1f;
            else if (Input.GetKey(KeyCode.S))
                direction = -1f;
        }
        if (side == ScreenSide.Right || useFullScreenForControl)
        {
            if (Input.GetKey(KeyCode.UpArrow))
                direction = 1f;
            else if (Input.GetKey(KeyCode.DownArrow))
                direction = -1f;
        }
        nextPosition = body.position + direction * sensitivity * Vector2.up;
        if (nextPosition.y < yBottomCoordinate || nextPosition.y > yTopCoordinate) return;
        body.MovePosition(nextPosition);
#endif
    }

}
