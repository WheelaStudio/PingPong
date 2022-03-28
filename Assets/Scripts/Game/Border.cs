using UnityEngine;
public enum ScreenSide
{
    Left, Right
}
public class Border : MonoBehaviour
{
    public ScreenSide side;
    private Game game;
    private void Start()
    {
        game = Game.Shared;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Ball"))
        {
            if (side == ScreenSide.Left)
            {
                game.LeftPlayerScore++;
            }
            else if(side == ScreenSide.Right)
            {
                game.RightPlayerScore++;
            }
        }
    }
}
