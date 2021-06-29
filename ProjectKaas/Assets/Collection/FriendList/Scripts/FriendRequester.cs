using System.Collections.Generic;
using Collection.Profile.Scripts;
using Photon.Pun;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using UnityEngine.Events;

namespace Collection.FriendList.Scripts
{
    public class FriendRequester : MonoBehaviour
    {
        #region Public Singleton

        public static FriendRequester Instance;

        #endregion

        #region UnityEvents

        public static readonly UnityEvent<string> OnAddSuccess = new UnityEvent<string>();
        public static readonly UnityEvent<string> OnAddFriendFailed = new UnityEvent<string>();

        #endregion

        #region MonoBehaviour Callbacks

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Add Friend.
        /// Only fill one parameter.
        /// </summary>
        /// <param name="friend">PlayerProfileModel friend</param>
        public void AddFriend(PlayerProfileModel friend)
        {
            // Add friend
            PlayFabClientAPI.AddFriend(new AddFriendRequest
                {
                    FriendPlayFabId = friend.PlayerId
                },
                result =>
                {
                    // check if successful
                    if (result.Created)
                    {
                        OnAddSuccess?.Invoke(friend.DisplayName);
                    }
                    else
                    {
                        Debug.Log($"add failed {friend.DisplayName}");
                        OnAddFriendFailed?.Invoke($"add failed {friend.DisplayName}");
                    }
                }, 
                error =>
            {
                Debug.Log($"Add friend Failed: {error.ErrorMessage}");
                OnAddFriendFailed?.Invoke(error.ErrorMessage);
            });
        }

        #endregion
    }
}
