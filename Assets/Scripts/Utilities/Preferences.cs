using UnityEngine;
using System;
public enum GameMode
{
    WithBot, ForTwo, Multiplayer
}
public enum Complexity
{
    Easy, Medium, Hard
}
public enum ScreenSide
{
    Left, Right
}
public static class Preferences
{
    public static readonly Color disabledColor = new(0.5f, 0.5f, 0.5f);
    public static readonly Color enabledColor = Color.white;
    private const string SENSITIVITY_COEFFICIENT_KEY = "SENSITIVITY_COEFFICIENT";
    private const string NICKNAME_KEY = "NICKNAME";
    private const string VOLUME_KEY = "VOLUME";
    private const string COMPLEXITY_KEY = "COMPLEXITY";
    private const string PLAYER_SIDE_KEY = "PLAYER_SIDE";
    private const string GAME_UP_KEY = "GAME_UP";
    private static float sensitivityCoefficient = 0f;
    private static float screenInch = 0f;
    private static int gameUp = 0;
    private static bool? soundIsEnabled;
    private static string nickname;
    public const float ScreenWorldHeight = 10f;
    public static GameMode GameMode;
    private static Complexity? complexity;
    private static ScreenSide? playerSide;
    public static float DistanceBetweenFlats;
    public static void Init()
    {
        soundIsEnabled = SoundIsEnabled;
        screenInch = ScreenInch;
        gameUp = GameUp;
        nickname = NickName;
        complexity = PlayerComplexity;
        playerSide = PlayerSide;
        AudioListener.volume = (bool)soundIsEnabled ? 1f : 0f;
    }
    public static string NickName
    {
        get
        {
            if (nickname == null)
                nickname = PlayerPrefs.GetString(NICKNAME_KEY, "player");
            return nickname;
        }
        set
        {
            nickname = value;
            PlayerPrefs.SetString(NICKNAME_KEY, nickname);
            PlayerPrefs.Save();
        }
    }
    public static int GameUp
    {
        get
        {
            if (gameUp == 0)
                gameUp = PlayerPrefs.GetInt(GAME_UP_KEY, 50);
            return gameUp;
        }
        set
        {
            gameUp = value;
            PlayerPrefs.SetInt(GAME_UP_KEY, gameUp);
            PlayerPrefs.Save();  
        }
    }
    public static Complexity PlayerComplexity
    {
        get
        {
            if (complexity == null)
                complexity = (Complexity)PlayerPrefs.GetInt(COMPLEXITY_KEY);
            return (Complexity)complexity;
        }
        set
        {
            complexity = value;
            PlayerPrefs.SetInt(COMPLEXITY_KEY,(int)complexity);
            PlayerPrefs.Save();
        }
    }
    public static (float,float) SpeedSpread
    {
        get
        {
            return complexity switch
            {
                Complexity.Easy => (0.0045f, 0.0305f),
                Complexity.Medium => (0.00575f, 0.03075f),
                Complexity.Hard => (0.00675f, 0.03075f),
                _ => (0f, 0f),
            };
        }
    }
    public static ScreenSide PlayerSide
    {
        get
        {
            if (playerSide == null)
                playerSide = (ScreenSide)PlayerPrefs.GetInt(PLAYER_SIDE_KEY);
            return (ScreenSide)playerSide;
        }
        set
        {
            playerSide = value;
            PlayerPrefs.SetInt(PLAYER_SIDE_KEY, (int)playerSide);
            PlayerPrefs.Save();
        }
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
