using UnityEngine;
public class Game : MonoBehaviour
{
    [SerializeField] private GameObject WithBot, ForTwo, Multiplayer;
    private void Start()
    {
        switch(Preferences.gameMode)
        {
            case GameMode.WithBot:
                Instantiate(WithBot);
                break;
            case GameMode.ForTwo:
                Instantiate(ForTwo);
                break;
            case GameMode.Multiplayer:
                Instantiate(Multiplayer);
                break;
        }
   }

}

