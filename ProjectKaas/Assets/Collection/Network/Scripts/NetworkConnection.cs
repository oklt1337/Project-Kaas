using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace Collection.Network.Scripts
{
    public class NetworkConnection : MonoBehaviourPunCallbacks
    {
        #region Private Fields

        /// <summary>
        /// This client's version number. Users are separated from each other by gameVersion.
        /// </summary>
        private const string GameVersion = "0.0.0";

        #endregion


        #region MonoBehaviour CallBacks

        private void Awake()
        {
            // Connect to Photon Online Server.
            Debug.Log("Connecting to server...");
                
            // Set GameVersion.
            PhotonNetwork.GameVersion = GameVersion;
            
            PhotonNetwork.ConnectUsingSettings();
            
            // Make sure if LoadLevel is called all clients sync their level automatically
            PhotonNetwork.AutomaticallySyncScene = true;
        }

        #endregion

        #region PhotonNetwork Callbacks

        public override void OnConnectedToMaster()
        {
            Debug.Log("Connected to master.");
            
            PhotonNetwork.LoadLevel(1);
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            Debug.LogWarning("Connection lost. Reason: " + cause);
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            Debug.Log("Failed to join room.");
        }


        public override void OnJoinedRoom()
        {
            Debug.Log("Joined room: " + PhotonNetwork.CurrentRoom.Name);
        }

        #endregion
    }
}
