using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

namespace Collection.UI.Scripts.Play
{
    public class CurrentRoomCanvas : MonoBehaviour
    {
        #region Private Serializable Fields

        [SerializeField] private Toggle toggle;
        
        #endregion
        
        #region MonoBehaviour Callbacks
        
        

        #endregion
        
        #region Public Methods
        
        public void OnTogglePrivacyRoom()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.CurrentRoom.IsOpen = toggle.isOn;
                PhotonNetwork.CurrentRoom.IsVisible = toggle.isOn;
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
