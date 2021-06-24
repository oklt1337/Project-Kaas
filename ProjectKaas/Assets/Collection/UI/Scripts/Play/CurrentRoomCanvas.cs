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

        private void OnEnable()
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

                Debug.Log(PhotonNetwork.CountOfRooms); 
                Debug.Log("Open = " + PhotonNetwork.CurrentRoom.IsOpen); 
                Debug.Log("Visible = " + PhotonNetwork.CurrentRoom.IsVisible); 
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
