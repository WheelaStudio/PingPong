using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Lean.Localization;
public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject modeChooser, sideChooser, settingsPanel;
    [SerializeField] private Slider gameUpSlider;
    [SerializeField] private TextMeshProUGUI gameUpText;
    private void Start()
    {
        gameUpSlider.value = Preferences.GameUp;
        DisplayGameUp(Preferences.GameUp);
    }
    public void SetActiveSettings(bool value)
    {
        if (!modeChooser.activeSelf && !sideChooser.activeSelf)
        {
            gameUpSlider.enabled = !value;
            settingsPanel.SetActive(value);
        }
    }
    public void SetActiveModeChooser(bool active)
    {
        if (!settingsPanel.activeSelf && !sideChooser.activeSelf)
        {
            gameUpSlider.enabled = !active;
            modeChooser.SetActive(active);
        }
    }
    public void SetActiveSideChooser(bool active)
    {
        sideChooser.SetActive(active);
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
        Preferences.PlayerSide = (ScreenSide)side;
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
            if (sideChooser.activeSelf)
                SetActiveSideChooser(false);
        }
    }
}
