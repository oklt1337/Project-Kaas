using Collection.UI.Scripts.Login;
using Collection.UI.Scripts.Play.CreateRoom;
using Collection.UI.Scripts.Play.Room;
using UnityEngine;

namespace Collection.UI.Scripts
{
    public class OverlayCanvases : MonoBehaviour
    {
        #region Singleton

        public static OverlayCanvases Instance;

        #endregion

        #region Private Serializable Fields
        
        [Tooltip("RoomList Canvas.")]
        [SerializeField] private RoomListCanvas roomListCanvas;
        
        [Tooltip("Current Room Canvas.")]
        [SerializeField] private CurrentRoomCanvas currenRoomCanvas;
        
        [Tooltip("Player Info Canvas.")]
        [SerializeField] private PlayerInfoCanvas playerInfoCanvas;

        #endregion
        
        #region Public Properies
        
        public RoomListCanvas RoomListCanvas => roomListCanvas;
        public CurrentRoomCanvas CurrenRoomCanvas => currenRoomCanvas;
        public PlayerInfoCanvas PlayerInfoCanvas => playerInfoCanvas;
        
        #endregion

        #region MonoBehaviour Callbacks

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
            }
        }

        #endregion
    }
}
