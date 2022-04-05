using UnityEngine;
public class BotFlatController : Flat
{
    private Rigidbody2D ballBody;
    private BallController ballController;
    private (float, float) speedSpread = (0.035f, 0.095f);
    protected override void Awake()
    {
        screenSide = Preferences.PlayerSide == ScreenSide.Left ? ScreenSide.Right : ScreenSide.Left;
        base.Awake();     
    }
    protected override void Start()
    {
        ballController = BallController.Shared;
        ballBody = ballController.GetComponent<Rigidbody2D>();
        var aspectRatio = Preferences.DistanceBetweenFlats / Preferences.ScreenWorldHeight;
        speedSpread.Item1 *= aspectRatio;
        speedSpread.Item2 *= aspectRatio;
        if (screenSide == ScreenSide.Right)
            transform.eulerAngles = new Vector3(0f, 0f, 180f);
        var speed = ballController.Speed;
        speedSpread.Item1 /= speed;
        speedSpread.Item2 /= speed;
        base.Start();
    }
    private void FixedUpdate()
    {
        if (!ballController.IsOnTheField) return;
        var newPosition = body.position + new Vector2(0f, (ballBody.position.y < body.position.y ? -1f : 1f) 
          * Random.Range(speedSpread.Item1, speedSpread.Item2) * ballController.Speed);
        if (newPosition.y < yBottomCoordinate || newPosition.y > yTopCoordinate) return;
        body.MovePosition(newPosition);
    }

}
