using System.Collections.Generic;
using UnityEngine;

namespace Collection.UI.Scripts.Play.Room
{
    public class PlayerLayoutGroup : MonoBehaviour
    {
        #region Private Serializable Fields

        [SerializeField] private GameObject playerListingPrefab;

        #endregion
        
        #region Private Fields

        private GameObject PlayerListingPrefab => playerListingPrefab;
        private List<PlayerListing> PlayerList { get; set; } = new List<PlayerListing>();

        #endregion

        #region Photon Callbacks

        private void OnJoinedRoom()
        {
            
        }

        #endregion
        
        #region Private Methods

        private void PlayerJoinedRoom(Photon.Realtime.Player player)
        {
            
        }
        
        private void PlayerLeftRoom(Photon.Realtime.Player player)
        {
            
        }

        #endregion
    }
}
