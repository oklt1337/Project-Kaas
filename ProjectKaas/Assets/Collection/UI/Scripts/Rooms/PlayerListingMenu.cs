using System.Collections.Generic;
using Collection.UI.Scripts.Utilities;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Collection.UI.Scripts.Rooms
{
    public class PlayerListingMenu : MonoBehaviourPunCallbacks
    {
        [SerializeField] private PlayerListing playerListingPrefab;
        [SerializeField] private Transform content;
        [SerializeField] private GameObject startButton;
        [SerializeField] private GameObject readyButton;
        [SerializeField] private RawImage readyUp;

        [Header("Ready Button Textures")]
        [SerializeField] private Texture readyTexture;
        [SerializeField] private Texture notReadyTexture;
        
        private List<PlayerListing> _playerListings = new List<PlayerListing>();
        private RoomCanvases _roomCanvases;
        private bool _ready;

        /// <summary>
        /// Activates or deactivates buttons for the clients.
        /// </summary>
        /// <param name="show">bool</param>
        public void ActivateButtonsForClient(bool show)
        {
            startButton.SetActive(show);
            readyButton.SetActive(!show);
        }

        public void Initialize(RoomCanvases canvases)
        {
            _roomCanvases = canvases;
        }

        public override void OnEnable()
        {
            GetCurrentRoomPlayer();
            SetReadyUpBool(false);
        }

        public override void OnDisable()
        {
            foreach (var playerListing in _playerListings)
            {
                Destroy(playerListing.gameObject);
            }

            _playerListings.Clear();
        }

        private void SetReadyUpBool(bool state)
        {
            _ready = state;
            readyUp.texture = _ready ? readyTexture : notReadyTexture;
        }

        /// <summary>
        /// Get all player currently in a room
        /// </summary>
        private void GetCurrentRoomPlayer()
        {
            if (!PhotonNetwork.IsConnected)
                return;
            if (PhotonNetwork.CurrentRoom == null || PhotonNetwork.CurrentRoom.Players == null)
                return;

            //in current room
            //find all player in dictionary 
            //and add them to player list
            foreach (var playerInfo in PhotonNetwork.CurrentRoom.Players)
            {
                AddPlayerListing(playerInfo.Value);
            }
        }

        /// <summary>
        /// add new player to player list
        /// </summary>
        /// <param name="newPlayer"></param>
        private void AddPlayerListing(Player newPlayer)
        {
            var index = _playerListings.FindIndex(x => x.Player == newPlayer);

            if (index != -1)
            {
                _playerListings[index].SetPlayerInfo(newPlayer);
            }
            else
            {
                var listing = Instantiate(playerListingPrefab, content);
                if (listing != null)
                {
                    listing.SetPlayerInfo(newPlayer);
                    _playerListings.Add(listing);
                }
            }
        }

        public override void OnMasterClientSwitched(Player newMasterClient)
        {
            _roomCanvases.CurrentRoomCanvas.LeaveRoomMenu.OnClickLeaveRoom();
        }

        /// <summary>
        /// Photon internal method
        /// </summary>
        /// <param name="newPlayer">Player</param>
        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            AddPlayerListing(newPlayer);
        }

        /// <summary>
        /// Photon internal method
        /// </summary>
        /// <param name="otherPlayer">Player</param>
        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            //Find player that left room
            var index = _playerListings.FindIndex(x => x.Player == otherPlayer);

            if (index != -1)
            {
                //destroy player object
                Destroy(_playerListings[index].gameObject);
                //remove player from player list
                _playerListings.RemoveAt(index);
            }
        }

        public void OnClickStartGame()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                for (int i = 0; i < _playerListings.Count; i++)
                {
                    if (_playerListings[i].Player != PhotonNetwork.LocalPlayer)
                    {
                        if (!_playerListings[i].Ready)
                            return;
                    }
                }

                PhotonNetwork.CurrentRoom.IsOpen = false;
                PhotonNetwork.CurrentRoom.IsVisible = false;
                PhotonNetwork.LoadLevel(1);
            }
        }

        public void OnClickReadyUp()
        {
            SetReadyUpBool(!_ready);
            photonView.RPC("RPC_ChangeReadyState", RpcTarget.MasterClient, PhotonNetwork.LocalPlayer, _ready);
        }

        [PunRPC]
        private void RPC_ChangeReadyState(Player player, bool ready)
        {
            var index = _playerListings.FindIndex(x => x.Player == player);
            if (index != -1)
            {
                //set ready status
                _playerListings[index].Ready = ready;
            }
        }
    }
}
