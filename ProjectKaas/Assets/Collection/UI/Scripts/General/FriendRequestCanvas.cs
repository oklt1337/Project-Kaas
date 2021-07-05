using System.Collections.Generic;
using Collection.Profile.Scripts;
using PlayFab.ClientModels;
using UnityEngine;

namespace Collection.UI.Scripts.General
{
    public class FriendRequestCanvas : MonoBehaviour
    {
        #region Private Serializable Fields

        [SerializeField] private GameObject friendRequestPrefab;
        [SerializeField] private List<FriendRequest> friendRequests = new List<FriendRequest>();

        #endregion

        #region Public Methods

        public void AddNewFriendRequest(FriendInfo friendList)
        {
            Debug.Log("open request");
            // Instantiate friendRequestObj.
            var friendRequestObj = Instantiate(friendRequestPrefab,  transform, false);

            // Set Obj name to nickname.
            friendRequestObj.name = friendList.TitleDisplayName;
            
            // find friendRequest script and Initialize.
            var friendRequest = friendRequestObj.GetComponent<FriendRequest>();
            friendRequest.Initialize(friendList);

            // add to list.
            friendRequests.Add(friendRequest);
        }

        #endregion
    }
}
