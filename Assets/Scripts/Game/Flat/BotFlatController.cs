using UnityEngine;
public class BotFlatController : Flat
{
    private Rigidbody2D ballBody;
    private BallController ballController;
#if UNITY_STANDALONE || UNITY_EDITOR
    private readonly (float, float) speedSpread = (0.325f, 0.75f);
#endif
#if !UNITY_STANDALONE && !UNITY_EDITOR
    private readonly (float, float) speedSpread = (7.42f, 17.14f);
#endif
    protected override void Awake()
    {
        screenSide = ScreenSide.Right;
        base.Awake();
    }
    private void Start()
    {
        ballController = BallController.Shared;
        ballBody = ballController.GetComponent<Rigidbody2D>();
    }
    private void FixedUpdate()
    {
        if (!ballController.IsVisible) return;
        var newPosition = body.position + new Vector2(0f, (ballBody.position.y < body.position.y ? -1f : 1f)
             * sensitivity * Random.Range(speedSpread.Item1, speedSpread.Item2));
        if (newPosition.y < yBottomCoordinate || newPosition.y > yTopCoordinate) return;
        body.MovePosition(newPosition);
    }

}
