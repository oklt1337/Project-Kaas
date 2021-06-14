using Photon.Pun;
using UnityEngine;

namespace Collection.UI.Scripts.Rooms
{
    public class LeaveRoomMenu : MonoBehaviour
    {
        private RoomCanvases _roomCanvases;
        
        public void Initialize(RoomCanvases canvases)
        {
            _roomCanvases = canvases;
        }

        public void OnClickLeaveRoom()
        {
            PhotonNetwork.LeaveRoom();
        }
    }
}
