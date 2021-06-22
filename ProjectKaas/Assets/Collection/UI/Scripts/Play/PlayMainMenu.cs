using UnityEngine;

namespace Collection.UI.Scripts.Play
{
    public class PlayMainMenu : MonoBehaviour
    {
        #region Private Serializable Fields

        [Tooltip("Canvas for creating a room.")]
        [SerializeField] private GameObject createRoomCanvas;
        
        [Tooltip("Canvas to join matchmaking.")]
        [SerializeField] private GameObject matchmakingCanvas;

        #endregion
        
        
        #region Public Methods

        public void OnClickMatchmaking()
        {
            matchmakingCanvas.SetActive(true);
        }

        public void OnClickCreateRoom()
        {
            createRoomCanvas.SetActive(true);
        }
        
        #endregion
    }
}
