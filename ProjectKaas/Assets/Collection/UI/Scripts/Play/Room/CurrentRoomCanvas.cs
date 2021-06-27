using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Collection.UI.Scripts.Play.Room
{
    public class CurrentRoomCanvas : MonoBehaviourPunCallbacks
    {
        #region Private Serializable Fields

        [Tooltip("Toggle to make room private or public.")]
        [SerializeField] private Toggle toggle;
        
        [Tooltip("Room name")]
        [SerializeField] private TextMeshProUGUI roomName;
        
        [Tooltip("Current Max Player Count")]
        [SerializeField] private TextMeshProUGUI playerCount;

        #endregion
        
        #region MonoBehaviour Callbacks

        public override void OnJoinedRoom()
        {
            playerCount.text = PhotonNetwork.CurrentRoom.MaxPlayers.ToString();
            roomName.text = PhotonNetwork.CurrentRoom.Name;
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

        public void OnClickAdd()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.CurrentRoom.MaxPlayers++;
                if (PhotonNetwork.CurrentRoom.MaxPlayers > 8)
                {
                    PhotonNetwork.CurrentRoom.MaxPlayers = 8;
                }
            }
            
            playerCount.text = PhotonNetwork.CurrentRoom.MaxPlayers.ToString();
        }
        
        public void OnClickRemove()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.CurrentRoom.MaxPlayers--;
                if (PhotonNetwork.CurrentRoom.MaxPlayers < 1)
                {
                    PhotonNetwork.CurrentRoom.MaxPlayers = 1;
                }

                if (PhotonNetwork.CurrentRoom.MaxPlayers < PhotonNetwork.CurrentRoom.PlayerCount)
                {
                    PhotonNetwork.CurrentRoom.MaxPlayers = PhotonNetwork.CurrentRoom.PlayerCount;
                }
            }
            
            playerCount.text = PhotonNetwork.CurrentRoom.MaxPlayers.ToString();
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
            OverlayCanvases.Instance.CurrenRoomCanvas.gameObject.SetActive(false);
        }
        
        #endregion
    }
}
