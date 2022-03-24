using UnityEngine.SceneManagement;
public enum Scene
{
    LoadingScreen, Lobby, Game
}
public static class SceneLoader
{
    public static void LoadScene(Scene scene)
    {
        SceneManager.LoadScene((int)scene);
    }
}
