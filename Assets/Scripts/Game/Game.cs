using UnityEngine;
using System.Collections;
using TMPro;
using Lean.Localization;
public enum GameState
{
    Running, Paused, Finished
}
public class Game : MonoBehaviour
{
    private const int timerSeconds = 3;
    [HideInInspector] public GameState State { get; private set; } = GameState.Running;
    private int leftPlayerScore = 0, rightPlayerScore = 0, gameUp = 0;
    private TextMeshProUGUI resumeTimerText;
    [SerializeField] private TextMeshProUGUI leftPlayerScoreText, rightPlayerScoreText, gameUpText;
    [SerializeField] private GameObject WithBot, ForTwo, Multiplayer, PausePanel, PauseButton, resumeTimer;
    public static Game Shared { get; private set; }
    private BallController ballController;
    private QuestionDialog questionDialog;
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
        resumeTimerText = resumeTimer.GetComponent<TextMeshProUGUI>();
        gameUp = Preferences.GameUp;
        questionDialog = QuestionDialog.Shared;
        ballController = BallController.Shared;
        gameUpText.text = string.Format(LeanLocalization.GetTranslationText("GameUp"), gameUp);
    }
    private void Update()
    {
#if !UNITY_IOS
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (State == GameState.Running)
            {
                SetActive(false);
            }
            else if (PausePanel.activeSelf)
            {
                Quit();
            }
            else if (questionDialog.IsActive)
            {
                HideQuestionDialog();
            }
        }
#endif
    }
    private void ToggleTime(bool active)
    {
        Time.timeScale = active ? 1f : 0f;
    }
    public void SetActive(bool active)
    {
        if (active)
        {
            IEnumerator ResumeTimer()
            {
                PausePanel.SetActive(false);
                resumeTimer.SetActive(true);
                var delay = new WaitForSecondsRealtime(1f);
                for (int i = 3; i >= 0; i--)
                {
                    resumeTimerText.text = i.ToString();
                    yield return delay;
                }
                resumeTimer.SetActive(false);
                PauseButton.SetActive(true);
                State = GameState.Running;
                ToggleTime(true);
            }
            StartCoroutine(ResumeTimer());
        } else
        {
            State = GameState.Paused;
            ToggleTime(false);
            PauseButton.SetActive(false);
            PausePanel.SetActive(true);
        }
    }
    private void HideQuestionDialog()
    {
        questionDialog.Hide();
        PausePanel.SetActive(true);
    }
    public void Quit()
    {
        PausePanel.SetActive(false);
        questionDialog.Show(LeanLocalization.GetTranslationText("QuitQuestion"), delegate
        {
            ToggleTime(true);
            SceneLoader.LoadScene(Scene.Lobby);
        }, delegate
        {
            HideQuestionDialog();
        });
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

