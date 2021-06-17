using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace Collection.Network.Scripts
{
    public class TestConnection : MonoBehaviourPunCallbacks
    {
        // Start is called before the first frame update
        private void Start()
        {
            Debug.Log("Connecting to server.", this);

            PhotonNetwork.SendRate = 40; //20
            PhotonNetwork.SerializationRate = 20; //10
            PhotonNetwork.AutomaticallySyncScene = true;
            PhotonNetwork.NickName = MasterManager.GameSettings.InitializeNickname();
            PhotonNetwork.GameVersion = MasterManager.GameSettings.GameVersion;
            PhotonNetwork.ConnectUsingSettings();
        }

        public override void OnConnectedToMaster()
        {
            Debug.Log(PhotonNetwork.LocalPlayer.NickName + " connected to the server.", this);

            if (!PhotonNetwork.InLobby)
            {
                PhotonNetwork.JoinLobby();
            }
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            Debug.Log("Disconnected from server for reason " + cause, this);
        }
    }
}
