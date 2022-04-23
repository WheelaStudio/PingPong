using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Lean.Localization;
using Photon.Pun;
public class MenuManager : MonoBehaviourPunCallbacks
{
    [Header("Nickname Settings")]
    [SerializeField] private TMP_InputField nickNameField;
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
    [SerializeField] private GameObject multiplayerLoadingPanel;
    [Header("Multiplayer loading panel")]
    [SerializeField] private TextMeshProUGUI multiplayerLoadingTitle;
    [SerializeField] private TextMeshProUGUI multiplayerLoadingBody;
    private void Start()
    {
        gameUpSlider.value = Preferences.GameUp;
        DisplayGameUp(Preferences.GameUp);
        nickNameField.text = Preferences.NickName;
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
    public void SetActiveMultiplayerLoadingPanel(bool active)
    {
        multiplayerLoadingPanel.SetActive(active);
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
    private void DisplayError(string message = null)
    {
        multiplayerLoadingTitle.text = LeanLocalization.GetTranslationText("Error");
        multiplayerLoadingBody.text = string.Format(LeanLocalization.GetTranslationText("ConnectionFailed"), message);
    }
    public void CreateRoom()
    {
        SetActiveMultiplayerLoadingPanel(true);
        multiplayerLoadingTitle.text = LeanLocalization.GetTranslationText("Loading");
        multiplayerLoadingBody.text = LeanLocalization.GetTranslationText("Wait");
        if (!PhotonNetwork.JoinRandomOrCreateRoom(roomName: null))
            DisplayError();
    }
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        DisplayError(message);
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
            PhotonNetwork.NickName = Preferences.NickName;
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
