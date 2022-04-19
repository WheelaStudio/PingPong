using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Lean.Localization;
using Photon.Pun;
public class MenuManager : MonoBehaviourPunCallbacks
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
        complexityButtonsBorders[(int)Preferences.PlayerComplexity].color = enabledColor;
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
        complexityButtonsBorders[(int)Preferences.PlayerComplexity].color = disabledColor;
        Preferences.PlayerComplexity = (Complexity)complexity;
        complexityButtonsBorders[complexity].color = enabledColor;
    }
    public void CreateRoom()
    {
        PhotonNetwork.JoinRandomOrCreateRoom(roomName: null);
    }
    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("Game");
    }
    public void StartGame(int gameMode)
    {
        Preferences.GameMode = (GameMode)gameMode;
        if (gameMode != 2)
        {
            SceneLoader.LoadScene(Scene.Game);
        }
        else
        {
            CreateRoom();
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
