using Photon.Pun;
using UnityEngine;

namespace Collection.Network.Scripts
{
    public class LobbyNetworkManager : MonoBehaviour
    {
        // Start is called before the first frame update
        private void Start()
        {
            print("Connecting to server...");
            PhotonNetwork.GameVersion = "0.0.0";
        }

        private void OnConnectedToMaster()
        {
            print("Connected to master.");
        }
    }
}
