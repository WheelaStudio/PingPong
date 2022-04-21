using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Settings : MonoBehaviour
{
    [SerializeField] private Toggle soundToggle;
    [SerializeField] private GameObject[] settingsTabs;
    [SerializeField] private Image[] settingsTabsButtonsBorders;
    private int currentSettingsTabIndex = 0; 
    [SerializeField] private TextMeshProUGUI sensitivityValueText;
    [SerializeField] private Slider sensitivityCoefficientSlider;
    private void Awake()
    {
        soundToggle.isOn = Preferences.SoundIsEnabled;
        sensitivityCoefficientSlider.value = Preferences.SensitivityCoefficient;
        DisplaySensitivityCoefficient(Preferences.SensitivityCoefficient);
    }
    public void SwitchTab(int tab)
    {
        settingsTabsButtonsBorders[currentSettingsTabIndex].color = Preferences.disabledColor;
        settingsTabs[currentSettingsTabIndex].SetActive(false);
        currentSettingsTabIndex = tab;
        settingsTabsButtonsBorders[currentSettingsTabIndex].color = Preferences.enabledColor;
        settingsTabs[currentSettingsTabIndex].SetActive(true);
    }
    public void SetNickName(string nickName)
    {
        Preferences.NickName = nickName;
    }
    public void ToggleSound(bool isEnabled)
    {
        Preferences.SoundIsEnabled = isEnabled;
    }
    public void DisplaySensitivityCoefficient(float value)
    {
        sensitivityValueText.text = $"{Mathf.RoundToInt(value * 100f)}%";
    }
    public void SaveSensitivityCoefficient()
    {
        Preferences.SensitivityCoefficient = sensitivityCoefficientSlider.value;
    }
}
