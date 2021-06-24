using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

namespace Collection.UI.Scripts.Play
{
    public class CurrentRoomCanvas : MonoBehaviourPunCallbacks
    {
        #region Private Serializable Fields

        [SerializeField] private Toggle toggle;
        
        #endregion
        
        #region MonoBehaviour Callbacks

        public override void OnJoinedRoom()
        {
            toggle.isOn = PhotonNetwork.CurrentRoom.IsVisible;
        }

        #endregion
        
        #region Public Methods
        
        public void OnTogglePrivacyRoom()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.CurrentRoom.IsOpen = toggle.isOn;
                PhotonNetwork.CurrentRoom.IsVisible = toggle.isOn;

                Debug.Log(PhotonNetwork.CurrentRoom.IsVisible ? "Lobby is Open" : "Lobby is Closed");
            }
        }

        public void OnClickReady()
        {
            
        }

        public void OnClickLeave()
        {
            PhotonNetwork.LeaveRoom();
            OverlayCanvases.Instance.CurrenRoomCanvas.SetActive(false);
        }
        
        #endregion
    }
}
