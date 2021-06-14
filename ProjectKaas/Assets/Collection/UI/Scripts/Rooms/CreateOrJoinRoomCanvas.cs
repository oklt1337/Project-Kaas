using UnityEngine;

namespace Collection.UI.Scripts.Rooms
{
    public class CreateOrJoinRoomCanvas : MonoBehaviour
    {
        [SerializeField] private CreateRoomMenu createRoomMenu;
        public CreateRoomMenu CreateRoomMenu => createRoomMenu;
        
        private RoomCanvases _roomCanvases;
        
        public void Initialize(RoomCanvases canvases)
        {
            _roomCanvases = canvases;
            CreateRoomMenu.Initialize(canvases);
        }
    }
}
