using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Lean.Localization;

public class Settings : MonoBehaviour
{
    [SerializeField] private Toggle soundToggle;
    [SerializeField] private TextMeshProUGUI sensitivityValueText;
    [SerializeField] private Slider sensitivityCoefficientSlider;
    [SerializeField] private TextMeshProUGUI[] setDesignButtonTexts;
    private void Awake()
    {
        soundToggle.isOn = Preferences.SoundIsEnabled;
        sensitivityCoefficientSlider.value = Preferences.SensitivityCoefficient;
        DisplaySensitivityCoefficient(Preferences.SensitivityCoefficient);
        SetDesignButtonTexts((int)Preferences.CurrentGameDesign);
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
    public void SetDesign(int designIndex)
    {
        var newGameDesignIndex = designIndex;
        if ((int)Preferences.CurrentGameDesign == newGameDesignIndex)
            return;
        SetDesignButtonTexts(newGameDesignIndex);
        Preferences.CurrentGameDesign = (GameDesign)newGameDesignIndex;
    }
    private void SetDesignButtonTexts(int designIndex)
    {
        setDesignButtonTexts[designIndex].text = LeanLocalization.GetTranslationText("Established");
        setDesignButtonTexts[designIndex == 0 ? 1 : 0].text = LeanLocalization.GetTranslationText("Set");
    }
}
