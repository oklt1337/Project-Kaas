using UnityEngine;

namespace Collection.UI.Scripts.Rooms
{
    public class CreateOrJoinRoomCanvas : MonoBehaviour
    {
        [SerializeField] private CreateRoomMenu createRoomMenu;
        public CreateRoomMenu CreateRoomMenu => createRoomMenu;
        
        [SerializeField] private RoomListingMenu roomListingMenu;
        public RoomListingMenu RoomListingMenu => roomListingMenu;
        
        private RoomCanvases _roomCanvases;
        
        public void Initialize(RoomCanvases canvases)
        {
            _roomCanvases = canvases;
            CreateRoomMenu.Initialize(canvases);
            RoomListingMenu.Initialize(canvases);
        }
    }
}
