using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

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

        [Tooltip("UI Panel to let the user enter name, connect and play")] [SerializeField]
        private GameObject controlPanel;

        [Tooltip("UI Label to inform the user that the connection is in progress")] [SerializeField]
        private GameObject progressLabel;

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
        public void OnClickPlay()
        {
            progressLabel.SetActive(true);
            controlPanel.SetActive(false);

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
            progressLabel.SetActive(false);
            controlPanel.SetActive(true);
            
            _joinMatchmaking = false;
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            // Create new room.
            if (_joinMatchmaking)
            {
                PhotonNetwork.CreateRoom(null, new RoomOptions {MaxPlayers = maxPlayersPerRoom});
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
                
                // Load the Map.
                PhotonNetwork.LoadLevel("Map " + Random.Range(1, 3));
            }
        }

        #endregion
    }
}
