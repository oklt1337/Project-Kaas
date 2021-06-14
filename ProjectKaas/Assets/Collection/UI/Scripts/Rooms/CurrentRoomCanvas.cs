using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace Collection.UI.Scripts.Rooms
{
    public class CurrentRoomCanvas : MonoBehaviour
    {
        [SerializeField] private PlayerListingMenu playerListingsMenu;
        public PlayerListingMenu PlayerListingMenu => playerListingsMenu;
        
        [SerializeField] private LeaveRoomMenu leaveRoomMenu;
        public LeaveRoomMenu LeaveRoomMenu => leaveRoomMenu;
        
        private RoomCanvases _roomCanvases;
        
        public void Initialize(RoomCanvases canvases)
        {
            _roomCanvases = canvases;
            PlayerListingMenu.Initialize(canvases);
            LeaveRoomMenu.Initialize(canvases);
        }

        /// <summary>
        /// Activate room in inspector.
        /// </summary>
        public void ActivateRoom()
        {
            gameObject.SetActive(true);
            PlayerListingMenu.ActivateButtonsForClient(PhotonNetwork.IsMasterClient);
        }

        /// <summary>
        /// Deactivate room in inspector.
        /// </summary>
        public void DeactivateRoom()
        {
            gameObject.SetActive(false);
        }
    }
}
