using System.Collections.Generic;
using Collection.UI.Scripts.Utilities;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace Collection.UI.Scripts.Rooms
{
    public class PlayerListingMenu : MonoBehaviourPunCallbacks
    {
        [SerializeField] private PlayerListing playerListingPrefab;
        [SerializeField] private Transform content;

        private List<PlayerListing> _playerListings = new List<PlayerListing>();
        private RoomCanvases _roomCanvases;
        
        public void Initialize(RoomCanvases canvases)
        {
            _roomCanvases = canvases;
        }

        private void Awake()
        {
            GetCurrentRoomPlayer();
        }

        /// <summary>
        /// Photon internal method
        /// </summary>
        public override void OnLeftRoom()
        {
            content.DestroyChildren();
        }

        /// <summary>
        /// Get all player currently in a room
        /// </summary>
        private void GetCurrentRoomPlayer()
        {
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
            var listing = Instantiate(playerListingPrefab, content);
            if (listing != null)
            {
                listing.SetPlayerInfo(newPlayer);
                _playerListings.Add(listing);
            }
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
    }
}
