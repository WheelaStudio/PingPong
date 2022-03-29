using UnityEngine;
public enum GameMode
{
    WithBot, ForTwo, Multiplayer
}
public static class Preferences
{
    private static float screenInch = 0f;
    public static GameMode gameMode;
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
}
