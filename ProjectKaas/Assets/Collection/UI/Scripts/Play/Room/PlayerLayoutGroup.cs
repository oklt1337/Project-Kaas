using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace Collection.UI.Scripts.Play.Room
{
    public class PlayerLayoutGroup : MonoBehaviourPunCallbacks
    {
        #region Private Serializable Fields

        [Tooltip("Prefab for PlayerListing")]
        [SerializeField] private GameObject playerListingPrefab;

        #endregion
        
        #region Private Fields

        private GameObject PlayerListingPrefab => playerListingPrefab;
        
        #endregion

        #region Public Fields

        public List<PlayerListing> PlayerList { get; set; } = new List<PlayerListing>();

        #endregion

        #region Photon Callbacks

        public override void OnJoinedRoom()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                foreach (var listing in PlayerList)
                {
                    Destroy(listing.gameObject);
                }
                
                PlayerList.Clear();
            }
            
            Debug.Log("Player joined room now create listings");
            
            // get list of player in room.
            var photonPlayers = PhotonNetwork.PlayerList;

            foreach (var player in photonPlayers)
            {
                PlayerJoinedRoom(player);
            }
        }
        
        public override void OnPlayerEnteredRoom(Player player)
        {
            PlayerJoinedRoom(player);
        }

        public override void OnPlayerLeftRoom(Player player)
        {
            PlayerLeftRoom(player);
        }

        #endregion
        
        #region Private Methods

        /// <summary>
        /// Add player to list and instantiate listing obj.
        /// </summary>
        /// <param name="player">Photon.Realtime.Player</param>
        private void PlayerJoinedRoom(Player player)
        {
            if (player == null)
                return;
            
            Debug.Log($"PlayerListing for {player.NickName}");
            
            // just to make sure to not add duplicates.
            PlayerLeftRoom(player);

            // Instantiate player listing obj.
            var playerListingObj = Instantiate(playerListingPrefab, transform, false);

            // Set Obj name to nickname.
            playerListingObj.name = player.NickName;
            
            // find playerListing script and apply player.
            var playerListing = playerListingObj.GetComponent<PlayerListing>();
            playerListing.ApplyPhotonPlayer(player);
            
            // add to list.
            PlayerList.Add(playerListing);
            
            var hashtable = playerListing.PhotonPlayer.CustomProperties;
            if (hashtable.ContainsKey("Position"))
            {
                hashtable.Remove("Position");
            }
            
            hashtable.Add("Position", PlayerList.Count -1);
            playerListing.PhotonPlayer.CustomProperties = hashtable;
        }
        
        /// <summary>
        /// Destroy listing obj and remove from list.
        /// </summary>
        /// <param name="player">Photon.Realtime.Player</param>
        private void PlayerLeftRoom(Player player)
        {
            // Find player in list.
            var index = PlayerList.FindIndex(x => x.PhotonPlayer == player);

            if (index != -1)
            {
                // Destroy if exist
                Destroy(PlayerList[index].gameObject);
                // Remove from list.
                PlayerList.RemoveAt(index);
            }
        }

        #endregion
    }
}
