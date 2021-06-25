using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

namespace Collection.UI.Scripts.Play.Room
{
    public class CurrentRoomCanvas : MonoBehaviourPunCallbacks
    {
        #region Private Serializable Fields

        [Tooltip("Toggle to make room private or public.")]
        [SerializeField] private Toggle toggle;
        
        #endregion
        
        #region MonoBehaviour Callbacks

        public override void OnJoinedRoom()
        {
            toggle.isOn = PhotonNetwork.CurrentRoom.IsVisible;
        }

        #endregion
        
        #region Public Methods
        
        /// <summary>
        /// Toggle room private/public.
        /// </summary>
        public void OnTogglePrivacyRoom()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.CurrentRoom.IsOpen = toggle.isOn;
                PhotonNetwork.CurrentRoom.IsVisible = toggle.isOn;

                Debug.Log(PhotonNetwork.CurrentRoom.IsVisible ? "Lobby is Public" : "Lobby is Private");
            }
        }

        /// <summary>
        /// Ready.
        /// </summary>
        public void OnClickReady()
        {
            
        }

        /// <summary>
        /// Leave current room.
        /// </summary>
        public void OnClickLeave()
        {
            PhotonNetwork.LeaveRoom();
            OverlayCanvases.Instance.CurrenRoomCanvas.SetActive(false);
        }
        
        #endregion
    }
}
