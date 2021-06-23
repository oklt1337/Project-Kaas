using UnityEngine;

namespace Collection.UI.Scripts
{
    public class OverlayCanvases : MonoBehaviour
    {
        #region Singleton

        public static OverlayCanvases Instance;

        #endregion

        #region Private Serializable Fields
        
        [Tooltip("Gameobject witch contains RoomList Canvas.")]
        [SerializeField] private GameObject roomListCanvas;
        
        [Tooltip("Gameobject witch contains Room Canvas.")]
        [SerializeField] private GameObject currenRoomCanvas;
        
        #endregion
        
        #region Public Properies
        public GameObject RoomListCanvas => roomListCanvas;
        public GameObject CurrenRoomCanvas => currenRoomCanvas;
        
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
