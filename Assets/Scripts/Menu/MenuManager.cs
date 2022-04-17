using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Lean.Localization;
public class MenuManager : MonoBehaviour
{
    private Color disabledColor = new(0.5f, 0.5f, 0.5f);
    private Color enabledColor = Color.white;
    [SerializeField] private Image[] sideButtonsBorders;
    [SerializeField] private Image[] complexityButtonsBorders;
    [SerializeField] private GameObject modeChooser, botSettingsPanel, settingsPanel;
    [SerializeField] private Slider gameUpSlider;
    [SerializeField] private TextMeshProUGUI gameUpText;
    private void Start()
    {
        gameUpSlider.value = Preferences.GameUp;
        DisplayGameUp(Preferences.GameUp);
        sideButtonsBorders[(int)Preferences.PlayerSide].color = enabledColor;
        complexityButtonsBorders[(int)Preferences.PlayerСomplexity].color = enabledColor;
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
        sideButtonsBorders[(int)Preferences.PlayerSide].color = disabledColor;
        Preferences.PlayerSide = (ScreenSide)side;
        sideButtonsBorders[side].color = enabledColor;
    }
    public void SetComplexity(int complexity)
    {
        complexityButtonsBorders[(int)Preferences.PlayerСomplexity].color = disabledColor;
        Preferences.PlayerСomplexity = (Сomplexity)complexity;
        complexityButtonsBorders[complexity].color = enabledColor;
    }
    public void StartGame(int gameMode)
    {
        if (gameMode == 2)
        {
#if UNITY_ANDROID
            AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject unityActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            if (unityActivity != null)
            {
                AndroidJavaClass toastClass = new AndroidJavaClass("android.widget.Toast");
                unityActivity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
                {
                    AndroidJavaObject toastObject = toastClass.CallStatic<AndroidJavaObject>("makeText", unityActivity, "В разработке", 0);
                    toastObject.Call("show");
                }));
            }
#endif
        }
        else
        {
            Preferences.GameMode = (GameMode)gameMode;
            SceneLoader.LoadScene(Scene.Game);
        }
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
