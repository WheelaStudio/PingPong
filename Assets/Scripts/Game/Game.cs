using UnityEngine;
using System.Collections;
using TMPro;
using Lean.Localization;

public class Game : MonoBehaviour
{
    private const int timerSeconds = 3;
    [HideInInspector] public GameState State { get; private set; } = GameState.Running;
    [HideInInspector] public GameMode Mode { get; private set; }
    private int leftPlayerScore = 0, rightPlayerScore = 0, gameUp = 0;
    private TextMeshProUGUI resumeTimerText;
    [SerializeField] private TextMeshProUGUI leftPlayerScoreText, rightPlayerScoreText, gameUpText, scoreText;
    [SerializeField] private GameObject WithBot, ForTwo, Multiplayer, pausePanel, pauseButton, resumeTimer, gameOverPanel;
    public static Game Shared { get; private set; }
    private BallController ballController;
    private QuestionDialog questionDialog;
    private void Awake()
    {
        Shared = this;
    }
    private void Start()
    {
        Mode = Preferences.GameMode;
        switch (Mode)
        {
            case GameMode.WithBot:
                Instantiate(WithBot);
                break;
            case GameMode.ForTwo:
                Instantiate(ForTwo);
                break;
        }
        resumeTimerText = resumeTimer.GetComponent<TextMeshProUGUI>();
        gameUp = Preferences.GameUp;
        questionDialog = QuestionDialog.Shared;
        ballController = BallController.Shared;
        gameUpText.text = string.Format(LeanLocalization.GetTranslationText("GameUp"), gameUp);
    }
    private void OnApplicationFocus(bool focus)
    {
        if (!focus && State == GameState.Running)
            SetActive(false);
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
            else if (pausePanel.activeSelf)
            {
                OpenQuitDialog();
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
    private IEnumerator GameOver()
    {
        yield return new WaitForSeconds(0.5f);
        State = GameState.Finished;
        ToggleTime(false);
        pauseButton.SetActive(false);
        gameOverPanel.SetActive(true);
        scoreText.text = string.Format(LeanLocalization.GetTranslationText("Score"), $"{leftPlayerScore}:{rightPlayerScore}");
    }
    public void SetActive(bool active)
    {
        if (active)
        {
            IEnumerator ResumeTimer()
            {
                pausePanel.SetActive(false);
                resumeTimer.SetActive(true);
                var delay = new WaitForSecondsRealtime(1f);
                for (int i = timerSeconds; i > 0; i--)
                {
                    resumeTimerText.text = i.ToString();
                    yield return delay;
                }
                resumeTimer.SetActive(false);
                pauseButton.SetActive(true);
                State = GameState.Running;
                ToggleTime(true);
            }
            StartCoroutine(ResumeTimer());
        }
        else
        {
            State = GameState.Paused;
            ToggleTime(false);
            pauseButton.SetActive(false);
            pausePanel.SetActive(true);
        }
    }
    private void HideQuestionDialog()
    {
        questionDialog.Hide();
        pausePanel.SetActive(true);
    }
    public void OpenQuitDialog()
    {
        pausePanel.SetActive(false);
        questionDialog.Show(LeanLocalization.GetTranslationText("QuitQuestion"), delegate
        {
            Quit();
        }, delegate
        {
            HideQuestionDialog();
        });
    }
    public void OpenRestartDialog()
    {
        pausePanel.SetActive(false);
        questionDialog.Show(LeanLocalization.GetTranslationText("RestartQuestion"), delegate
        {
            Restart();
        }, delegate
        {
            HideQuestionDialog();
        });
    }
    public void Restart()
    {
        ToggleTime(true);
        SceneLoader.LoadScene(Scene.Game);
    }
    public void Quit()
    {
        ToggleTime(true);
        SceneLoader.LoadScene(Scene.Lobby);
    }
    private void CheckScore(int score)
    {
        if (score == gameUp)
            StartCoroutine(GameOver());
        if (score % 10 == 0)
            ballController.IncreaseSpeed();
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
            CheckScore(leftPlayerScore);
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
            CheckScore(rightPlayerScore);
        }
    }
}

