using System;
using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.Photon;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

namespace _Project.Scripts.PlayFab
{
    public class PlayFabFriendController : MonoBehaviour
    {
        public static PlayFabFriendController Instance;
        
        #region Serializable Fields

        #endregion

        #region Private Fields

        private List<FriendInfo> _friends;
        
        #endregion

        #region Public Fields

        #endregion

        #region Events

        public event Action<List<FriendInfo>> OnFriendListUpdated;

        #endregion

        #region Unity Methods

        private void Awake()
        {
            if (Instance != null)
                Destroy(gameObject);
            else
                Instance = this;
            
            _friends = new List<FriendInfo>();
        }

        private void Start()
        {
            PlayFabLogin.Instance.OnLoginSuccess += HandleGetFriends;
            //UIAddFriend.OnAddFriend += HandleAddPlayFabFriend;
            //UIFriend.OnRemoveFriend += HandleDelFriend;
        }

        private void OnDestroy()
        {
            PlayFabLogin.Instance.OnLoginSuccess -= HandleGetFriends;
            //UIAddFriend.OnAddFriend -= HandleAddPlayFabFriend;
            //UIFriend.OnRemoveFriend -= HandleDelFriend;
        }

        #endregion

        #region Private Methods

        private void HandleAddPlayFabFriend(string friendName)
        {
            var request = new AddFriendRequest {FriendTitleDisplayName = friendName};
            PlayFabClientAPI.AddFriend(request, OnFriendAddedSuccess, OnFriendAddedFailed);
        }

        private void GetPlayFabFriends()
        {
            var request = new GetFriendsListRequest
            {
                IncludeSteamFriends = false,
                IncludeFacebookFriends = false,
                XboxToken = null
            };
            PlayFabClientAPI.GetFriendsList(request, OnFriendListReceivedSuccess, OnFriendListReceivedFailed);
        }
        
        private void HandleDelFriend(string delFriend)
        {
            var id = _friends.FirstOrDefault(f => f.TitleDisplayName == delFriend)?.FriendPlayFabId;
            var request = new RemoveFriendRequest {FriendPlayFabId = id};
            PlayFabClientAPI.RemoveFriend(request, OnFriendRemoveSuccess, OnFriendRemoveFailed);
        }
        
        private void HandleGetFriends()
        {
            GetPlayFabFriends();
        }

        #endregion

        #region Public Methods

        #endregion

        #region PlayFab Callbacks

        private void OnFriendAddedSuccess(AddFriendResult result)
        {
            Debug.Log("Add Friend Successful");
            GetPlayFabFriends();
        }

        private void OnFriendAddedFailed(PlayFabError error)
        {
            Debug.LogError($"ERROR {error.GenerateErrorReport()}");
        }

        private void OnFriendListReceivedSuccess(GetFriendsListResult result)
        {
            Debug.Log("Received Friend List Successful");
            _friends = result.Friends;
            OnFriendListUpdated?.Invoke(result.Friends);
        }

        private void OnFriendListReceivedFailed(PlayFabError error)
        {
            Debug.LogError($"ERROR {error.GenerateErrorReport()}");
        }
        
        private void OnFriendRemoveSuccess(RemoveFriendResult result)
        {
            Debug.Log("Friend Remove Successful");
            GetPlayFabFriends();
        }
        
        private void OnFriendRemoveFailed(PlayFabError error)
        {
            Debug.LogError($"ERROR {error.GenerateErrorReport()}");
        }

        #endregion
    }
}
