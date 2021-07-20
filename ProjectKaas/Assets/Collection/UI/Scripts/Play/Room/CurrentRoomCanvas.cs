using System.Collections.Generic;
using Collection.Network.Scripts;
using Collection.UI.Scripts.Play.ChoosingCar;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;
using Toggle = UnityEngine.UI.Toggle;

namespace Collection.UI.Scripts.Play.Room
{
    public class CurrentRoomCanvas : MonoBehaviourPunCallbacks
    {
        #region Private Serializable Fields

        [SerializeField] private PlayerLayoutGroup playerLayoutGroup;
        [SerializeField] private ChooseCarHandler chooseCarHandler;

        [Tooltip("Room name")] [SerializeField]
        private TextMeshProUGUI roomName;

        [Tooltip("Current Max Player Count")] [SerializeField]
        private TextMeshProUGUI playerCount;

        [Header("Intractable Objects")]
        [Tooltip("Toggle to make room private or public.")] [SerializeField]
        private Toggle toggle;
        
        [SerializeField] private Button readyButton;
        [SerializeField] private Button leaveButton;
        [SerializeField] private Button addButton;
        [SerializeField] private Button removeButton;

        #endregion

        #region Private Fields

        private bool _ready;

        #endregion

        #region Public Fields

        public PlayerLayoutGroup PlayerLayoutGroup => playerLayoutGroup;
        public ChooseCarHandler ChooseCarHandler => chooseCarHandler;

        #endregion

        #region Photon Callbacks

        public override void OnEnable()
        {
            base.OnEnable();
            SetReadyState(false);
            SetTouchableState(true);

            if (!PhotonNetwork.IsMasterClient)
            {
                toggle.interactable = false;
            }
        }

        public override void OnJoinedRoom()
        {
            playerCount.text = PhotonNetwork.CurrentRoom.MaxPlayers.ToString();
            roomName.text = PhotonNetwork.CurrentRoom.Name;
            toggle.isOn = PhotonNetwork.CurrentRoom.IsVisible;
        }

        public override void OnDisable()
        {
            base.OnDisable();
            OverlayCanvases.Instance.PlayerInfoCanvas.gameObject.SetActive(false);
        }

        public override void OnMasterClientSwitched(Player newMasterClient)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                Debug.Log("MasterClient left room.");
                photonView.RPC("RPCCloseRoom", RpcTarget.All);
            }
        }

        #endregion

        #region Private Methods

        private void OnReadyButtonStates(bool state)
        {
            toggle.interactable = state;
            leaveButton.interactable = state;
            addButton.interactable = state;
            removeButton.interactable = state;

            foreach (var playerListing in playerLayoutGroup.PlayerList)
            {
                playerListing.GetComponent<Button>().interactable = state;
            }
        }

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

        [PunRPC]
        private void RPCCloseRoom()
        {
            Debug.Log("MasterClient closed room.");
            OnClickLeave();
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
                ReadyUpManager.Scripts.ReadyUpManager.Instance.LobbySize = PhotonNetwork.CurrentRoom.MaxPlayers;
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
                ReadyUpManager.Scripts.ReadyUpManager.Instance.LobbySize = PhotonNetwork.CurrentRoom.MaxPlayers;
            }

            playerCount.text = PhotonNetwork.CurrentRoom.MaxPlayers.ToString();
        }

        /// <summary>
        /// Ready.
        /// </summary>
        public void OnClickReady()
        {
            OnReadyButtonStates(!leaveButton.interactable);
            ChooseCarHandler.DeactivateCars();
            ChooseCarHandler.SetButtonInteractableState();
            
            var hashTable = PhotonNetwork.LocalPlayer.CustomProperties;
            if (hashTable.ContainsKey("Car"))
            {
                hashTable["Car"] = ChooseCarHandler.Car;
            }
            else
            {
                hashTable.Add("Car", ChooseCarHandler.Car);
            }
            PhotonNetwork.LocalPlayer.SetCustomProperties(hashTable);
            
            SetReadyState(!_ready);
            photonView.RPC("RPCChangeReadyState", RpcTarget.All, PhotonNetwork.LocalPlayer, _ready);
        }

        /// <summary>
        /// Leave current room.
        /// </summary>
        public void OnClickLeave()
        {
            PhotonNetwork.LeaveRoom();

            // make sure to remove old rooms from list.
            OverlayCanvases.Instance.RoomListCanvas.RoomLayoutGroup.RemoveOldRooms();
            OverlayCanvases.Instance.CurrenRoomCanvas.gameObject.SetActive(false);
        }

        /// <summary>
        /// Set Interactable Status of obj.
        /// </summary>
        /// <param name="state">bool</param>
        public void SetTouchableState(bool state)
        {
            toggle.interactable = state;
            readyButton.interactable = state;
            leaveButton.interactable = state;
            addButton.interactable = state;
            removeButton.interactable = state;

            foreach (var playerListing in playerLayoutGroup.PlayerList)
            {
                playerListing.GetComponent<Button>().interactable = state;
            }
        }

        #endregion
    }
}
