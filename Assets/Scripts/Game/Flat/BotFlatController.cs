using UnityEngine;
public class BotFlatController : Flat
{
    private Rigidbody2D ballBody;
    private BallController ballController;
    private (float, float) speedSpread;
    protected override void Awake()
    {
        screenSide = Preferences.PlayerSide == ScreenSide.Left ? ScreenSide.Right : ScreenSide.Left;
        base.Awake();     
    }
    protected override void Start()
    {
        ballController = BallController.Shared;
        ballBody = ballController.GetComponent<Rigidbody2D>();
        speedSpread = Preferences.SpeedSpread;
        if (screenSide == ScreenSide.Right)
            transform.eulerAngles = new Vector3(0f, 0f, 180f);
        base.Start();
    }
    private void FixedUpdate()
    {
        if (!ballController.IsOnTheField) return;
        var newPosition = body.position + new Vector2(0f, (ballBody.position.y < body.position.y ? -1f : 1f) 
          * Random.Range(speedSpread.Item1 * ballController.SpeedCoefficient,
          speedSpread.Item2 * ballController.SpeedCoefficient) * Mathf.Abs(ballBody.velocity.y));
        if (newPosition.y < yBottomCoordinate || newPosition.y > yTopCoordinate) return;
        body.MovePosition(newPosition);
    }

}
