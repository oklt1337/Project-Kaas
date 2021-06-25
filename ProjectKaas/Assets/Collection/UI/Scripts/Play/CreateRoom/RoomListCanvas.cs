using UnityEngine;

namespace Collection.UI.Scripts.Play.CreateRoom
{
    public class RoomListCanvas : MonoBehaviour
    {
        #region Private Serializable Fields

        [SerializeField] private RoomLayoutGroup roomLayoutGroup;

        #endregion
        
        #region Public Fields

        public RoomLayoutGroup RoomLayoutGroup => roomLayoutGroup;

        #endregion
        
        #region Public Methods

        public void OnClickJoinRoom(string roomName)
        {
            
        }
        
        #endregion
    }
}
