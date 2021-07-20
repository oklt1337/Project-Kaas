using System.Collections.Generic;
using Collection.Profile.Scripts;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UIElements;

namespace Collection.UI.Scripts.MainMenu.FriendList
{
    public class FriendListLayoutGroup : MonoBehaviourPunCallbacks
    {
        #region Const Tags

        private const string Requestee = "requestee";
        private const string Requester = "requester";

        #endregion
        
        #region Private Serializable Fields
        
        [SerializeField] private GameObject friendListingPrefab;

        #endregion
        
        #region Private Fields

        private GameObject FriendListingPrefab => friendListingPrefab;
        
        #endregion

        #region Public Fields

        public static List<FriendListings> FriendList { get; } = new List<FriendListings>();

        #endregion

        #region Private Methods
        
        private void FriendAdded(PlayFab.ClientModels.FriendInfo friend)
        {
            if (friend == null)
                return;

            if (friend.Tags.Contains(Requestee) || friend.Tags.Contains(Requester))
            {
                Debug.Log("not accepted yet");
                
                if (friend.Tags.Contains(Requester))
                {
                    Debug.Log("accepted request open");
                    OverlayCanvases.Instance.FriendRequestCanvas.AddNewFriendRequest(friend);
                }
            }
            else
            {
                // just to make sure to not add duplicates.
                FriendDeleted(friend);

                // Instantiate player listing obj.
                var friendListingObj = Instantiate(FriendListingPrefab, transform, false);

                // Set Obj name to nickname.
                friendListingObj.name = friend.TitleDisplayName;
            
                // find playerListing script and apply player.
                var friendListings = friendListingObj.GetComponent<FriendListings>();
                friendListings.ApplyFriend(friend);

                // add to list.
                FriendList.Add(friendListings);
            }
        }

        /// <summary>
        /// Destroy listing obj and remove from list.
        /// </summary>
        private void FriendDeleted(PlayFab.ClientModels.FriendInfo friend)
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

        #region Public Methods

        public void UpdateFriendList(List<PlayFab.ClientModels.FriendInfo> friends)
        {
            Debug.Log("Updating FriendsListings");

            foreach (var t in FriendList)
            {
                Destroy(t.gameObject);
            }
            
            FriendList.Clear();

            if (friends.Count == 0)
            {
                var children = new List<GameObject>();
                for (var i = 0; i < transform.childCount - 1; i++)
                {
                    var child = transform.GetChild(i);
                    children.Add(child.gameObject);
                }

                if (children.Count > 0)
                {
                    foreach (var child in children)
                    {
                        Destroy(child);
                    }
                }
            }
            
            foreach (var friend in friends)
            {
                FriendAdded(friend);
            }
        }

        #endregion
    }
}
