using System;
using Collection.UI.Scripts;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Collection.Network.Scripts
{
    public class MatchmakingManager : MonoBehaviourPunCallbacks
    {
        #region Private Serializable Fields

        /// <summary>
        /// The maximum number of players per room. When a room is full, it can't be joined by new players, and so new room will be created.
        /// </summary>
        [Tooltip(
            "The maximum number of players per room. When a room is full, it can't be joined by new players, and so new room will be created")]
        [SerializeField]
        private byte maxPlayersPerRoom = 8;

        #endregion

        #region Private Fields

        /// <summary>
        /// Keep track of the current process.
        /// </summary>
        private bool _joinMatchmaking;

        #endregion

        #region MonoBehaviour CallBacks

        private void Awake()
        {
            // Make sure if LoadLevel is called all clients sync their level automatically
            PhotonNetwork.AutomaticallySyncScene = true;
        }
        
        #endregion

        #region Public Methods

        /// <summary>
        /// Start the connection process.
        /// - If already connected, attempt joining a random room
        /// - if not yet connected, Connect to Photon Cloud Network
        /// </summary>
        public void OnClickPlay()
        {
            MainMenuCanvases.Instance.MainMenu.InfoText.gameObject.SetActive(true);
            MainMenuCanvases.Instance.MainMenu.InfoText.text = "Connecting...";

            // Check if we are connected or not,
            // Join if we are
            if (!PhotonNetwork.IsConnected) return;

            // Joining a Random Room.
            // If it fails, we'll get notified in OnJoinRandomFailed() and we'll create one.
            Debug.Log("Joining room...");

            _joinMatchmaking = true;
            PhotonNetwork.JoinRandomRoom();
        }

        #endregion

        #region PhotonNetwork Callbacks

        public override void OnDisconnected(DisconnectCause cause)
        {
            if (MainMenuCanvases.Instance.MainMenu.InfoText.gameObject != null)
            {
                MainMenuCanvases.Instance.MainMenu.InfoText.gameObject.SetActive(false);
            }
            
            _joinMatchmaking = false;
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            // Create new room.
            if (_joinMatchmaking)
            {
                PhotonNetwork.CreateRoom(null, new RoomOptions {MaxPlayers = maxPlayersPerRoom});
                var hashtable = PhotonNetwork.CurrentRoom.CustomProperties;
                hashtable.Add("Matchmaking", true);
            }
        }


        public override void OnJoinedRoom()
        {
            if (!_joinMatchmaking) return;

            // We only load if we are the first player
            // else we rely on `PhotonNetwork.AutomaticallySyncScene` to sync our instance scene.
            if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
            {
                Debug.Log("Load map...");


                var randomMap = Random.Range(0, 3);
                var map = randomMap switch
                {
                    0 => "City",
                    1 => "Japan",
                    2 => "Mountains",
                    _ => "City"
                };
                
                // Load the Map.
                PhotonNetwork.LoadLevel(map);
            }
        }

        #endregion
    }
}
