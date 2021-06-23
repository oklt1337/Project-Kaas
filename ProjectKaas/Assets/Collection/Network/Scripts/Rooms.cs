using Collection.UI.Scripts;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace Collection.Network.Scripts
{
    public class Rooms : MonoBehaviourPunCallbacks
    {
        #region Public Methods

        public void OnClickCreateRoom()
        {
            PhotonNetwork.CreateRoom(null, new RoomOptions {MaxPlayers = 8});
            
            OverlayCanvases.Instance.CurrenRoomCanvas.SetActive(true);
        }

        #endregion

        #region Photon Callbacks

        public override void OnCreatedRoom()
        {
            Debug.Log("Created room: " + PhotonNetwork.CurrentRoom.Name);
        }

        public override void OnLeftRoom()
        {
            Debug.Log("Left room: " + PhotonNetwork.CurrentRoom.Name);
        }

        #endregion
    }
}
