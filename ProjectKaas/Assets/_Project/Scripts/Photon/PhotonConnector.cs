using System;
using System.Collections;
using System.Collections.Generic;
using _Project.Scripts.PlayFab;
using Collection.Authentication.Scripts;
using Collection.Profile.Scripts;
using Collection.UI.Scripts;
using Photon.Pun;
using Photon.Realtime;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using Random = UnityEngine.Random;

namespace _Project.Scripts.Photon
{
    public class PhotonConnector : MonoBehaviourPunCallbacks
    {
        public static PhotonConnector Instance;
        
        #region Private Fields
        
        /// <summary>
        /// This client's version number. Users are separated from each other by gameVersion.
        /// </summary>
        private const string GameVersion = "0.0.1";
        
        /// <summary>
        /// This client's version number. Users are separated from each other by gameVersion.
        /// </summary>
        private const string AppVersion = "1.0.0";
        
        /// <summary>
        /// Coroutine for setting the ping.
        /// </summary>
        private Coroutine _pingCo;

        #endregion

        #region Public Fields

        #endregion

        #region Events

        public event Action OnConnectedToPhoton; 
        
        #endregion

        #region Unity Methods

        private void Awake()
        {
            if (Instance != null)
                Destroy(gameObject);
            else
                Instance = this;
        }

        private void Start()
        {
            // Need to be redone.
            PlayFabLogin.Instance.OnLogoutSuccess += ConnectViaNewConnection;
            //LocalProfile.OnProfileInitialized.AddListener(SetPhotonProfileValues);
            
            if (PhotonNetwork.IsConnected) return;
            ConnectToPhoton(SetRandomNickName());
        }

        private void OnDestroy()
        {
            PlayFabLogin.Instance.OnLogoutSuccess -= ConnectViaNewConnection;
            //LocalProfile.OnProfileInitialized.RemoveListener(SetPhotonProfileValues);
        }

        #endregion

        #region Private Methods

        private void ConnectViaNewConnection()
        {
            if (PhotonNetwork.IsConnected)
            {
                PhotonNetwork.Disconnect();
                ConnectToPhoton(SetRandomNickName());
            }
        }
        
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
            
            // Set GameVersion.
            PhotonNetwork.GameVersion = GameVersion;

            // Setting AuthValues
            PhotonNetwork.AuthValues = new AuthenticationValues(nickName);

            // Connecting to Photon
            PhotonNetwork.ConnectUsingSettings();

            // Make sure if LoadLevel is called all clients sync their level automatically
            PhotonNetwork.AutomaticallySyncScene = true;

            // Make sure on new connection hashtable is empty
            PhotonNetwork.LocalPlayer.CustomProperties = new Hashtable();

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
        
        private void TurnBackToRoom()
        {
            var hashtable = PhotonNetwork.LocalPlayer.CustomProperties;

            if (!hashtable.ContainsKey("MatchFinished")) return;
            if (!(bool) hashtable["MatchFinished"]) return;
            if (!hashtable.ContainsKey("OldRoom")) return;
            if (!hashtable.ContainsKey("MaxPlayer")) return;

            var options = new RoomOptions {MaxPlayers = (byte) hashtable["MaxPlayer"]};
            
            if (hashtable["OldRoom"] == null) return;
            PhotonNetwork.JoinOrCreateRoom((string) hashtable["OldRoom"], options, TypedLobby.Default);
            
            Debug.Log("Turn back to room.");
            OverlayCanvases.Instance.RoomListCanvas.gameObject.SetActive(true);
            OverlayCanvases.Instance.CurrenRoomCanvas.gameObject.SetActive(true);

            hashtable.Remove("MatchFinished");
            hashtable.Remove("WasMasterClient");
            hashtable.Remove("OldRoom");
            hashtable.Remove("MaxPlayer");

            PhotonNetwork.LocalPlayer.SetCustomProperties(hashtable);
        }

        private static IEnumerator SetPingCo()
        {
            while (PhotonNetwork.IsConnected)
            {
                if (!PhotonNetwork.IsConnectedAndReady || !PhotonNetwork.InRoom) continue;
                
                var hashtable = PhotonNetwork.LocalPlayer.CustomProperties;
                if (!hashtable.ContainsKey("Ping"))
                {
                    hashtable.Add("Ping", PhotonNetwork.GetPing());
                }
                else
                {
                    hashtable["Ping"] = PhotonNetwork.GetPing();
                }

                PhotonNetwork.LocalPlayer.SetCustomProperties(hashtable);

                yield return new WaitForSeconds(1f);
            }
        }
        
        /// <summary>
        /// Set Photon Nickname to PlayFab displayName.
        /// Set authValue to PlayFab playerId
        /// </summary>
        private static void SetPhotonProfileValues()
        {
            PhotonNetwork.LocalPlayer.NickName = LocalProfile.Instance.PlayerProfileModel.DisplayName;
            PhotonNetwork.AuthValues.UserId = LocalProfile.Instance.PlayerProfileModel.PlayerId;
        }

        #endregion

        #region Public Methods

        public void UpdatePlayerData(string nickName, string id)
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
            // Make sure coroutine doesnt run twice.
            if (_pingCo != null)
                StopCoroutine(_pingCo);
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
            if (scene.buildIndex != 1 && !PlayFabLogin.Instance.AutoLogin)
            {
                PhotonNetwork.LoadLevel(1);
            }
            else
            {
                Debug.Log("Rejoined Lobby");
                TurnBackToRoom();
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
            PhotonFriendController.Instance.DisplayFriends?.Invoke(friendList);
        }

        public override void OnCreatedRoom()
        {
            Debug.Log($"Created Room: {PhotonNetwork.CurrentRoom.Name}");
        }

        public override void OnJoinedRoom()
        {
            Debug.Log($"Joined Room: {PhotonNetwork.CurrentRoom.Name}");
            
            // Make sure coroutine doesnt run twice.
            if (_pingCo != null)
                StopCoroutine(_pingCo);

            _pingCo = StartCoroutine(SetPingCo());
            
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
