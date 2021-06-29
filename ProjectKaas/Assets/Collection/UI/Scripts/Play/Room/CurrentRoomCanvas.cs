using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
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

        [SerializeField] private PlayerLayoutGroup playerLayoutGroup;
        
        [SerializeField] private CountDown.CountDown countDown;

        #endregion

        #region Public Fields

        public CountDown.CountDown CountDown => countDown;

        #endregion

        #region Private Fields

        private bool _ready;

        #endregion

        #region Photon Callbacks

        public override void OnEnable()
        {
            base.OnEnable();
            SetReadyState(false);
        }

        public override void OnJoinedRoom()
        {
            playerCount.text = PhotonNetwork.CurrentRoom.MaxPlayers.ToString();
            roomName.text = PhotonNetwork.CurrentRoom.Name;
            toggle.isOn = PhotonNetwork.CurrentRoom.IsVisible;
        }

        #endregion

        #region Private Methods

        private void SetReadyState(bool state)
        {
            _ready = state;
        }

        [PunRPC]
        private void RPCChangeReadyState(Player player, bool ready)
        {
            var index = playerLayoutGroup.PlayerList.FindIndex(x => x.PhotonPlayer == player);

            if (index != -1)
            {
                playerLayoutGroup.PlayerList[index].Ready = ready;
            }
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
            SetReadyState(!_ready);
            photonView.RPC("RPCChangeReadyState", RpcTarget.All, PhotonNetwork.LocalPlayer, _ready);
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
