using UnityEngine;
using TMPro;
using Lean.Localization;
public class Game : MonoBehaviour
{
    private int leftPlayerScore = 0, rightPlayerScore = 0, gameUp = 0;
    [SerializeField] private TextMeshProUGUI leftPlayerScoreText, rightPlayerScoreText, gameUpText;
    [SerializeField] private GameObject WithBot, ForTwo, Multiplayer;
    public static Game Shared { get; private set; }
    private BallController ballController;
    private void Awake()
    {
        Shared = this;
    }
    private void Start()
    {
        switch (Preferences.gameMode)
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
        gameUp = Preferences.GameUp;
        ballController = BallController.Shared;
        gameUpText.text = string.Format(LeanLocalization.GetTranslationText("GameUp"), gameUp);
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
            if (leftPlayerScore % 10 == 0)
                ballController.IncreaseSpeed();
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
            if (rightPlayerScore % 10 == 0)
                ballController.IncreaseSpeed();
            rightPlayerScoreText.text = rightPlayerScore.ToString();
        }
    }
}

