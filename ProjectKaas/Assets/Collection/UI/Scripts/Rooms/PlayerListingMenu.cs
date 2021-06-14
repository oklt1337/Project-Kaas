using System.Collections.Generic;
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

        /// <summary>
        /// Photon internal method
        /// </summary>
        /// <param name="newPlayer">Player</param>
        public override void OnPlayerEnteredRoom(Player newPlayer)
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
