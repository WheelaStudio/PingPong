using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Lean.Localization;
using Photon.Pun;
public class MenuManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private TMP_InputField nickNameField;
    [SerializeField] private Image[] sideButtonsBorders;
    [SerializeField] private Image[] complexityButtonsBorders;
    [SerializeField] private GameObject modeChooser, botSettingsPanel, settingsPanel;
    [SerializeField] private Slider gameUpSlider;
    [SerializeField] private TextMeshProUGUI gameUpText;
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
