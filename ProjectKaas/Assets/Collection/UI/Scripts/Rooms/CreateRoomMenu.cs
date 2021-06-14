using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;

namespace Collection.UI.Scripts.Rooms
{
    public class CreateRoomMenu : MonoBehaviourPunCallbacks
    {
        [SerializeField] private TextMeshProUGUI roomName;

        private RoomCanvases _roomCanvases;

        public void Initialize(RoomCanvases canvases)
        {
            _roomCanvases = canvases;
        }

        /// <summary>
        /// Create a room
        /// </summary>
        public void OnClickCreateRoom()
        {
            //Make sure u are connected to the server
            if (!PhotonNetwork.IsConnected)
                return;

            //Make sure room name is not empty
            if (string.IsNullOrEmpty(roomName.text) || roomName.text.Length <= 1)
            {
                Debug.LogError("Enter a room name before trying to create a room.", this);
            }
            else
            {
                //Create RoomOptions
                var roomOptions = new RoomOptions {MaxPlayers = 8};
                //Create Room
                PhotonNetwork.JoinOrCreateRoom(roomName.text, roomOptions, TypedLobby.Default);
            }
        }

        /// <summary>
        /// Photon internal method
        /// </summary>
        public override void OnCreatedRoom()
        {
            Debug.Log("Created room successfully.", this);
            //Activate room.
            _roomCanvases.CurrentRoomCanvas.ActivateRoom();
        }

        /// <summary>
        /// Photon internal method
        /// </summary>
        public override void OnCreateRoomFailed(short returnCode, string message)
        {
            Debug.Log("Creating room failed: " + message, this);
        }
    }
}
