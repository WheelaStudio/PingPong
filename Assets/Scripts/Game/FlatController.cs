using UnityEngine;
public class FlatController : Flat
{
    [SerializeField] private ScreenSide side;
    protected override void Awake()
    {
        screenSide = side;
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
        nextPosition = body.position + direction * sensitivity * Vector2.up;
        if (nextPosition.y < yBottomCoordinate || nextPosition.y > yTopCoordinate) return;
        body.MovePosition(nextPosition);
#endif
    }

}
