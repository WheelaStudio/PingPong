using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Lean.Localization;
public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject modeChooser;
    [SerializeField] private Slider gameUpSlider;
    [SerializeField] private TextMeshProUGUI gameUpText;
    private void Start()
    {
        gameUpSlider.value = Preferences.GameUp;
        DisplayGameUp(Preferences.GameUp);
    }
    public void ToogleModeChooser(bool active)
    {
        modeChooser.SetActive(active);
    }
    public void DisplayGameUp(float value)
    {
        gameUpText.text = string.Format(LeanLocalization.GetTranslationText("GameUp"), Mathf.RoundToInt(value));
    }
    public void SaveGameUp()
    {
        Preferences.GameUp = Mathf.RoundToInt(gameUpSlider.value);
    }
    public void StartGame(int gameMode)
    {
        if (gameMode != 1)
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
            Preferences.gameMode = (GameMode)gameMode;
            SceneLoader.LoadScene(Scene.Game);
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && modeChooser.activeSelf)
            ToogleModeChooser(false);
    }
}
