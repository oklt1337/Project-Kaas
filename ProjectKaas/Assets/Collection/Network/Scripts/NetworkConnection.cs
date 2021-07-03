using System.Collections;
using Collection.UI.Scripts;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using Hashtable = ExitGames.Client.Photon.Hashtable;

namespace Collection.Network.Scripts
{
    public class NetworkConnection : MonoBehaviourPunCallbacks
    {
        #region Private Fields

        /// <summary>
        /// This client's version number. Users are separated from each other by gameVersion.
        /// </summary>
        private const string GameVersion = "0.0.0";
        
        /// <summary>
        /// This client's version number. Users are separated from each other by gameVersion.
        /// </summary>
        private const string AppVersion = "1.0.0";

        #endregion
        
        #region MonoBehaviour CallBacks

        private void Awake()
        {
            // Connect to Photon Online Server.
            Debug.Log("Connecting to server...");

            // Set AppVersion.
            PhotonNetwork.PhotonServerSettings.AppSettings.AppVersion = AppVersion;

            PhotonNetwork.ConnectUsingSettings();

            // Set GameVersion.
            PhotonNetwork.GameVersion = GameVersion;
            
            // Make sure if LoadLevel is called all clients sync their level automatically
            PhotonNetwork.AutomaticallySyncScene = true;
            
            // Set Random Default name as long not logged in
            NetworkManager.SetRandomDefaultNickName();
        }

        #endregion

        #region PhotonNetwork Callbacks

        public override void OnConnectedToMaster()
        {
            Debug.Log("Connected to master.");
            
            PhotonNetwork.JoinLobby();
        }

        public override void OnJoinedLobby()
        {
            Debug.Log("Joined Lobby.");
            PhotonNetwork.LoadLevel(1);
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            Debug.LogWarning("Connection lost. Reason: " + cause);
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            Debug.Log("Failed to join room.");
            OverlayCanvases.Instance.CurrenRoomCanvas.gameObject.SetActive(false);
        }
        
        public override void OnJoinedRoom()
        {
            Debug.Log("Joined room: " + PhotonNetwork.CurrentRoom.Name);
            OverlayCanvases.Instance.CurrenRoomCanvas.gameObject.SetActive(true);
            
            var customProp = PhotonNetwork.LocalPlayer.CustomProperties;
            if (customProp.ContainsKey("Room"))
            {
                customProp["Room"] = PhotonNetwork.CurrentRoom.Name;
            }
            else
            {
                customProp.Add("Room", PhotonNetwork.CurrentRoom.Name);
            }

            PhotonNetwork.LocalPlayer.SetCustomProperties(customProp);
        }

        #endregion
    }
}
