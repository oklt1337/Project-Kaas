using UnityEngine;

namespace Collection.UI.Scripts.Rooms
{
    public class CurrentRoomCanvas : MonoBehaviour
    {
        private RoomCanvases _roomCanvases;
        
        public void Initialize(RoomCanvases canvases)
        {
            _roomCanvases = canvases;
        }

        /// <summary>
        /// Activate room in inspector.
        /// </summary>
        public void ActivateRoom()
        {
            gameObject.SetActive(true);
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
