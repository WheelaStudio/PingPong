using UnityEngine;
using System;
public enum GameMode
{
    WithBot, ForTwo, Multiplayer
}
public static class Preferences
{
    private const string SENSITIVITY_COEFFICIENT_KEY = "SENSITIVITY_COEFFICIENT";
    private const string VOLUME_KEY = "VOLUME";
    private static float sensitivityCoefficient = 0f;
    private static float screenInch = 0f;
    private static bool? soundIsEnabled;
    public static GameMode gameMode;
    public static void Init()
    {
        soundIsEnabled = SoundIsEnabled;
        screenInch = ScreenInch;
        AudioListener.volume = (bool)soundIsEnabled ? 1f : 0f;
    }
    public static bool SoundIsEnabled
    {
        get
        {
            if (soundIsEnabled == null)
                soundIsEnabled = Convert.ToBoolean(PlayerPrefs.GetFloat(VOLUME_KEY, 1f));
            return (bool)soundIsEnabled;
        }
        set
        {
            soundIsEnabled = value;
            var volume = (bool)soundIsEnabled ? 1f : 0f;
            AudioListener.volume = volume;
            PlayerPrefs.SetFloat(VOLUME_KEY, volume);
            PlayerPrefs.Save();
        }
    }
    public static float ScreenInch
    {
        get
        {
            if (screenInch != 0f)
                return screenInch;
            var width = Screen.width * Screen.width;
            var height = Screen.height * Screen.height;
            var ypotinousa = width + height;
            ypotinousa = (int)Mathf.Sqrt(ypotinousa);
            screenInch = ypotinousa / Screen.dpi;
            return screenInch;
        }
    }
    public static float SensitivityCoefficient
    {
        get
        {
            if (sensitivityCoefficient == 0f)
                sensitivityCoefficient = PlayerPrefs.GetFloat(SENSITIVITY_COEFFICIENT_KEY, 1f);
            return sensitivityCoefficient;
        }
        set
        {
            sensitivityCoefficient = value;
            PlayerPrefs.SetFloat(SENSITIVITY_COEFFICIENT_KEY, sensitivityCoefficient);
            PlayerPrefs.Save();
        }
    }
}
