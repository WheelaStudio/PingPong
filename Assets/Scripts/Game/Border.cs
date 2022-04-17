using UnityEngine;
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
                game.RightPlayerScore++;
            }
            else if(side == ScreenSide.Right)
            {
                game.LeftPlayerScore++;
            }
        }
    }
}
