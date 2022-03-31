using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Settings : MonoBehaviour
{
    [SerializeField] private Toggle soundToggle;
    [SerializeField] private TextMeshProUGUI sensitivityValueText;
    [SerializeField] private Slider sensitivityCoefficientSlider;
    private void Awake()
    {
        soundToggle.isOn = Preferences.SoundIsEnabled;
        sensitivityCoefficientSlider.value = Preferences.SensitivityCoefficient;
        DisplaySensitivityCoefficient(Preferences.SensitivityCoefficient);
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
