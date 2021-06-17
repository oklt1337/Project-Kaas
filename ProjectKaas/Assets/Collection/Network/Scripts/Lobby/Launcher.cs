using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace Collection.Network.Scripts.Lobby
{
    public class Launcher : MonoBehaviourPunCallbacks
    {
        #region Private Serializable Fields

        /// <summary>
        /// The maximum number of players per room. When a room is full, it can't be joined by new players, and so new room will be created.
        /// </summary>
        [Tooltip(
            "The maximum number of players per room. When a room is full, it can't be joined by new players, and so new room will be created")]
        [SerializeField]
        private byte maxPlayersPerRoom = 8;

        [Tooltip("UI Panel to let the user enter name, connect and play")] [SerializeField]
        private GameObject controlPanel;

        [Tooltip("UI Label to inform the user that the connection is in progress")] [SerializeField]
        private GameObject progressLabel;

        #endregion


        #region Private Fields

        /// <summary>
        /// This client's version number. Users are separated from each other by gameVersion.
        /// </summary>
        private const string GameVersion = "0.0.0";
        
        /// <summary>
        /// Keep track of the current process.
        /// </summary>
        private bool _isConnecting;

        #endregion


        #region MonoBehaviour CallBacks

        private void Awake()
        {
            // Make sure if LoadLevel is called all clients sync their level automatically
            PhotonNetwork.AutomaticallySyncScene = true;
        }

        private void Start()
        {
            progressLabel.SetActive(false);
            controlPanel.SetActive(true);
        }

        #endregion


        #region Public Methods

        /// <summary>
        /// Start the connection process.
        /// - If already connected, attempt joining a random room
        /// - if not yet connected, Connect to Photon Cloud Network
        /// </summary>
        public void Connect()
        {
            progressLabel.SetActive(true);
            controlPanel.SetActive(false);

            // Check if we are connected or not,
            // Join if we are
            // Else we initiate the connection to the server.
            if (PhotonNetwork.IsConnected)
            {
                // Joining a Random Room.
                // If it fails, we'll get notified in OnJoinRandomFailed() and we'll create one.
                Debug.Log("Joining room...");
                PhotonNetwork.JoinRandomRoom();
            }
            else
            {
                // Connect to Photon Online Server.
                Debug.Log("Connecting to server...");
                
                // Keep track of the will to join a room
                _isConnecting = PhotonNetwork.ConnectUsingSettings();
                // Set GameVersion.
                PhotonNetwork.GameVersion = GameVersion;
            }
        }

        #endregion

        #region PhotonNetwork Callbacks

        public override void OnConnectedToMaster()
        {
            Debug.Log("Connected to master.");

            // Make sure we want to join a room.
            if (_isConnecting)
            {
                Debug.Log("Joining room...");
                
                // Joining a Random Room.
                // If it fails, we'll get notified in OnJoinRandomFailed() and we'll create one.
                PhotonNetwork.JoinRandomRoom();
                _isConnecting = false;
            }
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            progressLabel.SetActive(false);
            controlPanel.SetActive(true);
            
            _isConnecting = false;
            Debug.LogWarning("Connection lost. Reason: " + cause);
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            Debug.Log("Failed to join room.");

            // Create new room.
            PhotonNetwork.CreateRoom(null, new RoomOptions {MaxPlayers = maxPlayersPerRoom});
        }


        public override void OnJoinedRoom()
        {
            Debug.Log("Joined room: " + PhotonNetwork.CurrentRoom.Name);

            // We only load if we are the first player
            // else we rely on `PhotonNetwork.AutomaticallySyncScene` to sync our instance scene.
            if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
            {
                Debug.Log("Load map...");
                
                // Load the Map.
                PhotonNetwork.LoadLevel("Map " + Random.Range(1, 3));
            }
        }

        #endregion
    }
}
