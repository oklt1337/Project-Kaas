using Collection.UI.Scripts.Play;
using Collection.UI.Scripts.Play.CreateRoom;
using Photon.Pun;
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
        
        [Tooltip("Gameobject witch contains Room Canvas.")]
        [SerializeField] private GameObject currenRoomCanvas;
        
        [Tooltip("Gameobject witch contains player info canvas.")]
        [SerializeField] private GameObject playerInfoCanvas;
        
        #endregion
        
        #region Public Properies
        public RoomListCanvas RoomListCanvas => roomListCanvas;
        public GameObject CurrenRoomCanvas => currenRoomCanvas;
        public GameObject PlayerInfoCanvas => playerInfoCanvas;
        
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
