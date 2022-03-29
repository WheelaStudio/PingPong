using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Settings : MonoBehaviour
{
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private Toggle soundToggle;
    [SerializeField] private TextMeshProUGUI sensitivityValueText;
    [SerializeField] private Slider sensitivityCoefficientSlider;
    private void Start()
    {
        soundToggle.isOn = Preferences.SoundIsEnabled;
        sensitivityCoefficientSlider.SetValueWithoutNotify(Preferences.SensitivityCoefficient);
        DisplaySensitivityCoefficient(sensitivityCoefficientSlider.value);
    }
    public void SetActive(bool value)
    {
        settingsPanel.SetActive(value);
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
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && settingsPanel.activeSelf)
            SetActive(false);
    }
}
