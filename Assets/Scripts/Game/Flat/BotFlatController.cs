using UnityEngine;
public class BotFlatController : Flat
{
    private Rigidbody2D ballBody;
    private BallController ballController;
    private (float, float) speedSpread = (0.007f, 0.02885f);
    protected override void Awake()
    {
        screenSide = Preferences.PlayerSide == ScreenSide.Left ? ScreenSide.Right : ScreenSide.Left;
        base.Awake();     
    }
    protected override void Start()
    {
        ballController = BallController.Shared;
        ballBody = ballController.GetComponent<Rigidbody2D>();
        if (screenSide == ScreenSide.Right)
            transform.eulerAngles = new Vector3(0f, 0f, 180f);
        base.Start();
    }
    private void FixedUpdate()
    {
        if (!ballController.IsOnTheField) return;
        var newPosition = body.position + new Vector2(0f, (ballBody.position.y < body.position.y ? -1f : 1f) 
          * Random.Range(speedSpread.Item1, speedSpread.Item2) * Mathf.Abs(ballBody.velocity.y));
        if (newPosition.y < yBottomCoordinate || newPosition.y > yTopCoordinate) return;
        body.MovePosition(newPosition);
    }

}
