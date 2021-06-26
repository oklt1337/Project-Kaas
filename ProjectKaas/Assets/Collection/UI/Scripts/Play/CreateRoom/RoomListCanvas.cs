using Photon.Pun;
using UnityEngine;

namespace Collection.UI.Scripts.Play.CreateRoom
{
    public class RoomListCanvas : MonoBehaviour
    {
        #region Private Serializable Fields

        [Tooltip("RoomLayoutGroup")]
        [SerializeField] private RoomLayoutGroup roomLayoutGroup;

        #endregion
        
        #region Public Fields

        public RoomLayoutGroup RoomLayoutGroup => roomLayoutGroup;

        #endregion
        
        #region Public Methods

        /// <summary>
        /// Join room.
        /// </summary>
        /// <param name="roomName">string</param>
        public void OnClickJoinRoom(string roomName)
        {
            var join = PhotonNetwork.JoinRoom(roomName);

            if (join)
            {
                if (PhotonNetwork.CurrentRoom == null)
                {
                    OverlayCanvases.Instance.CurrenRoomCanvas.SetActive(false);
                }
                
                OverlayCanvases.Instance.CurrenRoomCanvas.gameObject.SetActive(true);
                Debug.Log("Join room successful.");
            }
            else
            {
                Debug.Log("Join room failed.");
            }
        }
        
        #endregion
    }
}
