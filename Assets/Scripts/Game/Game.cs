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
    [SerializeField] private TextMeshProUGUI leftPlayerScoreText, rightPlayerScoreText, gameUpText, scoreText, tutorialText;
    [SerializeField] private GameObject WithBotCommon, ForTwoCommon,
        WithBotAtari, ForTwoAtari, pausePanel, pauseButton, resumeTimer, gameOverPanel, tutorialPanel;
    [SerializeField] private TMP_FontAsset common, atari;
    public static Game Shared { get; private set; }
    private QuestionDialog questionDialog;
    private void Awake()
    {
        Shared = this;
    }
    private void Start()
    {
        Mode = Preferences.GameMode;
        var gameDesign = Preferences.CurrentGameDesign;
        switch (Mode)
        {
            case GameMode.WithBot:
                Instantiate(gameDesign == GameDesign.Common ? WithBotCommon : WithBotAtari);
#if UNITY_STANDALONE 
                tutorialText.text = LeanLocalization.GetTranslationText("pc_WithBotTutorial");
#endif
#if !UNITY_STANDALONE
                tutorialText.text = LeanLocalization.GetTranslationText("m_WithBotTutorial");
#endif
                if(!Preferences.WithBotTutorialIsViewed)
                {
                    DisplayTutorial();
                }
                break;
            case GameMode.ForTwo:
                Instantiate(gameDesign == GameDesign.Common ? ForTwoCommon : ForTwoAtari);
#if UNITY_STANDALONE
                tutorialText.text = LeanLocalization.GetTranslationText("pc_ForTwoTutorial");
#endif
#if !UNITY_STANDALONE
                tutorialText.text = LeanLocalization.GetTranslationText("m_ForTwoTutorial");
#endif
                if (!Preferences.ForTwoTutorialIsViewed)
                {
                    DisplayTutorial();
                }
                break;
        }
        switch(gameDesign)
        {
            case GameDesign.Common:
                leftPlayerScoreText.font = common;
                leftPlayerScoreText.fontMaterial = common.material;
                rightPlayerScoreText.font = common;
                rightPlayerScoreText.fontMaterial = common.material;
                break;
            case GameDesign.Atari:
                leftPlayerScoreText.font = atari;
                leftPlayerScoreText.fontMaterial = atari.material;
                rightPlayerScoreText.font = atari;
                rightPlayerScoreText.fontMaterial = atari.material;
                break;
        }
        resumeTimerText = resumeTimer.GetComponent<TextMeshProUGUI>();
        gameUp = Preferences.GameUp;
        questionDialog = QuestionDialog.Shared;
        gameUpText.text = string.Format(LeanLocalization.GetTranslationText("GameUp"), gameUp);
    }
    public void DisplayTutorial()
    {
        if(State == GameState.Running)
        {
            Time.timeScale = 0f;
            tutorialPanel.SetActive(true);
        } else
        {
            pausePanel.SetActive(false);
            tutorialPanel.SetActive(true);
        }
    }
    public void CloseTutorial()
    {
        if (State == GameState.Running)
        {
            switch (Mode)
            {
                case GameMode.WithBot:
                    Preferences.WithBotTutorialIsViewed = true;
                    break;
                case GameMode.ForTwo:
                    Preferences.ForTwoTutorialIsViewed = true;
                    break;
            }
            tutorialPanel.SetActive(false);
            Time.timeScale = 1f;
        } else
        {
            tutorialPanel.SetActive(false);
            pausePanel.SetActive(true);
        }
    }
    private void OnApplicationFocus(bool focus)
    {
        if (!focus && State == GameState.Running)
            SetActive(false);
    }
#if !UNITY_IOS
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (State == GameState.Running && !tutorialPanel.activeSelf)
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
            else if (tutorialPanel.activeSelf)
                CloseTutorial();
        }
    }
#endif
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
        SceneLoader.LoadScene(Scene.Menu);
    }
    private void CheckScore(int score)
    {
        if (score == gameUp)
            StartCoroutine(GameOver());
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

