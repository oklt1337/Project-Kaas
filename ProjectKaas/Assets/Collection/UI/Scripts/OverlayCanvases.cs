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
        
        #endregion
        
        #region Public Properies
        public GameObject RoomListCanvas => roomListCanvas;
        
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
