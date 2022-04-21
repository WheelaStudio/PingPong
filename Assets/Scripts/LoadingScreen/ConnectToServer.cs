using Photon.Pun;
using System.Collections;
using UnityEngine;
public class ConnectToServer : MonoBehaviourPunCallbacks
{
    private IEnumerator Start()
    {
        Preferences.Init();
        PhotonNetwork.GameVersion = Application.version;
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.ConnectUsingSettings();
        yield return new WaitForSeconds(10f);
    }

    public override void OnConnectedToMaster()
    {
        SceneLoader.LoadScene(Scene.Lobby);
    }
}
