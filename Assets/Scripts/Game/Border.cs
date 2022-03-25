using UnityEngine;
public enum Player
{
    Left, Right
}
public class Border : MonoBehaviour
{
    public Player player;
    private Game game;
    private void Start()
    {
        game = Game.Shared;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Ball"))
        {
            if (player == Player.Left)
            {
                game.LeftPlayerScore++;
            }
            else if(player == Player.Right)
            {
                game.RightPlayerScore++;
            }
        }
    }
}
