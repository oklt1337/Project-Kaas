using Collection.UI.Scripts;
using Collection.UI.Scripts.Play;
using Photon.Pun;
using Photon.Pun.Demo.Cockpit;
using Photon.Realtime;
using UnityEngine;

namespace Collection.Network.Scripts
{
    public class Rooms : MonoBehaviourPunCallbacks
    {
        #region Public Methods

        public void OnClickCreateRoom()
        {
            var roomOptions = new RoomOptions()
            {
                IsVisible = true,
                IsOpen = true,
                MaxPlayers = 8
            };
            
            //PhotonNetwork.CreateRoom(null, roomOptions, TypedLobby.Default);

            PhotonNetwork.JoinOrCreateRoom("Room", roomOptions, TypedLobby.Default);

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
            Debug.Log("Left room.");
        }

        #endregion
    }
}
