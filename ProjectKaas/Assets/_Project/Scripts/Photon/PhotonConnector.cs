using System;
using System.Collections.Generic;
using _Project.Scripts.PlayFab;
using _Project.Scripts.UI.PlayFab;
using Collection.UI.Scripts;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

namespace _Project.Scripts.Photon
{
    public class PhotonConnector : MonoBehaviourPunCallbacks
    {
        #region Private Fields

        /// <summary>
        /// This client's version number. Users are separated from each other by gameVersion.
        /// </summary>
        private const string GameVersion = "0.0.1";
        
        /// <summary>
        /// This client's version number. Users are separated from each other by gameVersion.
        /// </summary>
        private const string AppVersion = "1.0.0";
        
        #endregion

        #region Public Fields

        #endregion

        #region Events

        public static event Action OnConnectedToPhoton;
        
        #endregion

        #region Unity Methods

        private void Awake()
        {
            if (PhotonNetwork.IsConnected) return;

            ConnectToPhoton(SetRandomNickName());
        }

        #endregion

        #region Private Methods

        private string SetRandomNickName()
        {
            var rndNumber = Random.Range(1000, 9999);
            var nickName = $"Guest#{rndNumber}";

            return nickName;
        }
        
        private void ConnectToPhoton(string nickName)
        {
            Debug.Log($"Connect to Photon as {nickName}");
            
            // Set AppVersion.
            PhotonNetwork.PhotonServerSettings.AppSettings.AppVersion = AppVersion;

            // Connecting to Photon
            PhotonNetwork.ConnectUsingSettings();

            // Set GameVersion.
            PhotonNetwork.GameVersion = GameVersion;
            
            // Make sure if LoadLevel is called all clients sync their level automatically
            PhotonNetwork.AutomaticallySyncScene = true;

            // Make sure on new connection hashtable is empty
            PhotonNetwork.LocalPlayer.CustomProperties = new Hashtable();

            // Setting AuthValues
            PhotonNetwork.AuthValues = new AuthenticationValues(nickName);

            // Setting nickname
            PhotonNetwork.NickName = nickName;
        }

        private void CreatePhotonRoom(string roomName)
        {
            var option = new RoomOptions
            {
                MaxPlayers = 8,
                IsOpen = true,
                IsVisible = true
            };

            PhotonNetwork.JoinOrCreateRoom(roomName, option, TypedLobby.Default);
        }

        #endregion

        #region Public Methods

        public static void UpdatePlayerData(string nickName, string id)
        {
            PhotonNetwork.AuthValues = new AuthenticationValues(id);
            PhotonNetwork.LocalPlayer.NickName = nickName;
        }
        
        #endregion

        #region Photon Callbacks

        #region IConnectionCallbacks

        public override void OnConnected()
        {
            Debug.Log("Connecting...");
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            Debug.LogError($"Connection Lost: {cause}");
        }

        public override void OnConnectedToMaster()
        {
            Debug.Log("Connected to MasterServer.");

            if (!PhotonNetwork.InLobby)
            {
                PhotonNetwork.JoinLobby();
            }
        }

        public override void OnRegionListReceived(RegionHandler regionHandler)
        {
            Debug.Log("RegionList Received.");
            Debug.Log($"Best Region:{regionHandler.BestRegion.ToString()}");
        }

        #endregion

        #region ILobbyCallbacks

        public override void OnJoinedLobby()
        {
            Debug.Log($"Joined Lobby: {PhotonNetwork.CurrentLobby}");
            
            var scene = SceneManager.GetActiveScene();
            if (scene.buildIndex != 1)
            {
                PhotonNetwork.LoadLevel(1);
            }
            else
            {
                Debug.Log("Rejoined Lobby");
            }
        }

        public override void OnLeftLobby()
        {
            Debug.Log("Left Lobby.");
        }

        public override void OnRoomListUpdate(List<RoomInfo> roomList)
        {
            Debug.Log($"RoomList Updated: RoomCount = {roomList.Count}");
        }

        public override void OnLobbyStatisticsUpdate(List<TypedLobbyInfo> lobbyStatistics)
        {
            Debug.Log($"LobbyStatistics Updated: LobbyCount = {lobbyStatistics.Count}");
        }

        #endregion

        #region IMatchmakingCallbacks

        public override void OnFriendListUpdate(List<FriendInfo> friendList)
        {
            Debug.Log($"FriendList Updated: FriendCount = {friendList.Count}");
            PhotonFriendController.OnDisplayFriends?.Invoke(friendList);
        }

        public override void OnCreatedRoom()
        {
            Debug.Log($"Created Room: {PhotonNetwork.CurrentRoom.Name}");
        }

        public override void OnJoinedRoom()
        {
            Debug.Log($"Joined Room: {PhotonNetwork.CurrentRoom.Name}");
            
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

        public override void OnJoinRoomFailed(short returnCode, string message)
        {
            Debug.LogError($"ERROR {returnCode}: {message}");
            
            OverlayCanvases.Instance.CurrenRoomCanvas.gameObject.SetActive(false);
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            Debug.LogError($"ERROR {returnCode}: {message}");
        }

        public override void OnLeftRoom()
        {
            Debug.Log("Left Room.");
        }

        #endregion

        #region IInRoomCallbacks

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            Debug.Log($"{newPlayer.UserId}: Joined Room.");
        }

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            Debug.Log($"{otherPlayer.UserId}: Left the Room.");
        }

        public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
        {
            Debug.Log("RoomProperties Updated");
        }

        public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
        {
            Debug.Log($"PlayerProperties Updated: {targetPlayer.UserId}");
        }

        public override void OnMasterClientSwitched(Player newMasterClient)
        {
            Debug.Log($"New Master Client: {newMasterClient.UserId}");
        }

        #endregion

        #region IErrorInfoCallback

        public override void OnErrorInfo(ErrorInfo errorInfo)
        {
            Debug.LogError($"ERROR {errorInfo.Info}");
        }

        #endregion

        #endregion
    }
}
