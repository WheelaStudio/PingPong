using Photon.Pun;
using UnityEngine;
public class MenuManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject modeChooser;
    public void ToogleModeChooser(bool active)
    {
        modeChooser.SetActive(active);
    }
    public void StartGame(int gameMode)
    {
        if (gameMode != 1)
        {
#if UNITY_ANDROID
            AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject unityActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            if (unityActivity != null)
            {
                AndroidJavaClass toastClass = new AndroidJavaClass("android.widget.Toast");
                unityActivity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
                {
                    AndroidJavaObject toastObject = toastClass.CallStatic<AndroidJavaObject>("makeText", unityActivity, "В разработке", 0);
                    toastObject.Call("show");
                }));
            }
#endif
        }
        else
        {
            Preferences.gameMode = (GameMode)gameMode;
            SceneLoader.LoadScene(Scene.Game);
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && modeChooser.activeSelf)
            ToogleModeChooser(false);
    }
}
