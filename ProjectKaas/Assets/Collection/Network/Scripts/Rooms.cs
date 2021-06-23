using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace Collection.Network.Scripts
{
    public class Rooms : MonoBehaviourPunCallbacks
    {
        #region Private Fields
        
        
        
        #endregion

        #region Public Methods

        public void OnClickCreateRoom()
        {
            PhotonNetwork.CreateRoom(null, new RoomOptions {MaxPlayers = 8});
        }

        #endregion

        #region Photon Callbacks

        public override void OnCreatedRoom()
        {
            Debug.Log("Created room: " + PhotonNetwork.CurrentRoom.Name);
        }

        #endregion
    }
}
