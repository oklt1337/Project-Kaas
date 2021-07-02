using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace Collection.UI.Scripts.MainMenu.FriendList
{
    public class FriendListLayoutGroup : MonoBehaviourPunCallbacks
    {
        #region Private Serializable Fields
        
        [SerializeField] private GameObject friendListingPrefab;

        #endregion
        
        #region Private Fields

        private GameObject FriendListingPrefab => friendListingPrefab;
        
        #endregion

        #region Public Fields

        public List<FriendListings> FriendList { get; set; } = new List<FriendListings>();

        #endregion

        #region Photon Callbacks

        public override void OnFriendListUpdate(List<FriendInfo> friendList)
        {
            foreach (var friend in friendList)
            {
                FriendAdded(friend);
            }
            
            base.OnFriendListUpdate(friendList);
        }

        #endregion
        
        
        #region Private Methods
        
        private void FriendAdded(FriendInfo friend)
        {
            if (friend == null)
                return;
            
            // just to make sure to not add duplicates.
            FriendDeleted(friend);

            // Instantiate player listing obj.
            var friendListingObj = Instantiate(FriendListingPrefab, transform, false);

            // Set Obj name to nickname.
            friendListingObj.name = friend.UserId;
            
            // find playerListing script and apply player.
            var friendListings = friendListingObj.GetComponent<FriendListings>();
            friendListings.ApplyFriend(friend);

            // add to list.
            FriendList.Add(friendListings);
        }

        /// <summary>
        /// Destroy listing obj and remove from list.
        /// </summary>
        private void FriendDeleted(FriendInfo friend)
        {
            // Find player in list.
            var index = FriendList.FindIndex(x => x.Friend == friend);

            if (index != -1)
            {
                // Destroy if exist
                Destroy(FriendList[index].gameObject);
                // Remove from list.
                FriendList.RemoveAt(index);
            }
        }

        #endregion
    }
}
