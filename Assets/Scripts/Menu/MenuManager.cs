using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Lean.Localization;
public class MenuManager : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private Slider gameUpSlider;
    [SerializeField] private TextMeshProUGUI gameUpText;
    [Header("Bot mode settings")]
    [SerializeField] private Image[] sideButtonsBorders;
    [SerializeField] private Image[] complexityButtonsBorders;
    [Header("Panels")]
    [SerializeField] private GameObject modeChooser;
    [SerializeField] private GameObject botSettingsPanel;
    [SerializeField] private GameObject settingsPanel;
    private void Awake()
    {
        Preferences.Init();
    }
    private void Start()
    {
        gameUpSlider.value = Preferences.GameUp;
        DisplayGameUp(Preferences.GameUp);
        sideButtonsBorders[(int)Preferences.PlayerSide].color = Preferences.enabledColor;
        complexityButtonsBorders[(int)Preferences.PlayerComplexity].color = Preferences.enabledColor;
    }
    public void SetActiveSettings(bool value)
    {
        if (!modeChooser.activeSelf && !botSettingsPanel.activeSelf)
        {
            gameUpSlider.enabled = !value;
            settingsPanel.SetActive(value);
        }
    }
    public void SetActiveModeChooser(bool active)
    {
        if (!settingsPanel.activeSelf && !botSettingsPanel.activeSelf)
        {
            gameUpSlider.enabled = !active;
            modeChooser.SetActive(active);
        }
    }
    public void SetActiveBotSettingsPanel(bool active)
    {
        botSettingsPanel.SetActive(active);
        modeChooser.SetActive(!active);
    }
    public void DisplayGameUp(float value)
    {
        gameUpText.text = string.Format(LeanLocalization.GetTranslationText("GameUp"), Mathf.RoundToInt(value));
    }
    public void SaveGameUp()
    {
        Preferences.GameUp = Mathf.RoundToInt(gameUpSlider.value);
    }
    public void SetSide(int side)
    {
        sideButtonsBorders[(int)Preferences.PlayerSide].color = Preferences.disabledColor;
        Preferences.PlayerSide = (ScreenSide)side;
        sideButtonsBorders[side].color = Preferences.enabledColor;
    }
    public void SetComplexity(int complexity)
    {
        complexityButtonsBorders[(int)Preferences.PlayerComplexity].color = Preferences.disabledColor;
        Preferences.PlayerComplexity = (Complexity)complexity;
        complexityButtonsBorders[complexity].color = Preferences.enabledColor;
    }
    public void StartGame(int gameMode)
    {
        Preferences.GameMode = (GameMode)gameMode;
        SceneLoader.LoadScene(Scene.Game);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (modeChooser.activeSelf)
                SetActiveModeChooser(false);
            if (settingsPanel.activeSelf)
                SetActiveSettings(false);
            if (botSettingsPanel.activeSelf)
                SetActiveBotSettingsPanel(false);
        }
    }
}
