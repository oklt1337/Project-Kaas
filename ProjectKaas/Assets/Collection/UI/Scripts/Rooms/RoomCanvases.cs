using UnityEngine;

namespace Collection.UI.Scripts.Rooms
{
    public class RoomCanvases : MonoBehaviour
    {
        [SerializeField] private CreateOrJoinRoomCanvas createOrJoinRoomCanvas;
        public CreateOrJoinRoomCanvas CreateOrJoinRoomCanvas => createOrJoinRoomCanvas;
        
        [SerializeField] private CurrentRoomCanvas currentRoomCanvas;
        public CurrentRoomCanvas CurrentRoomCanvas => currentRoomCanvas;

        private void Awake()
        {
            FirstInitialize();
        }

        /// <summary>
        /// Initialize canvases
        /// </summary>
        private void FirstInitialize()
        {
            CreateOrJoinRoomCanvas.Initialize(this);
            CurrentRoomCanvas.Initialize(this);
        }
    }
}
