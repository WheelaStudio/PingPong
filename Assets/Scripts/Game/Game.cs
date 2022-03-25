using UnityEngine;
using TMPro;
public class Game : MonoBehaviour
{
    private int leftPlayerScore = 0, rightPlayerScore = 0;
    [SerializeField] private TextMeshProUGUI leftPlayerScoreText, rightPlayerScoreText;
    [SerializeField] private GameObject WithBot, ForTwo, Multiplayer;
    public static Game Shared { get; private set; }
    private void Awake()
    {
        Shared = this;
    }
    private void Start()
    {
        switch(Preferences.gameMode)
        {
            case GameMode.WithBot:
                Instantiate(WithBot);
                break;
            case GameMode.ForTwo:
                Instantiate(ForTwo);
                break;
            case GameMode.Multiplayer:
                Instantiate(Multiplayer);
                break;
        }
   }
    public int LeftPlayerScore
    {
        get
        {
            return leftPlayerScore;
        }
        set
        {
            leftPlayerScore = value;
            leftPlayerScoreText.text = leftPlayerScore.ToString();
        }
    }
    public int RightPlayerScore
    {
        get
        {
            return rightPlayerScore;
        }
        set
        {
            rightPlayerScore = value;
            rightPlayerScoreText.text = rightPlayerScore.ToString();
        }
    }
}

