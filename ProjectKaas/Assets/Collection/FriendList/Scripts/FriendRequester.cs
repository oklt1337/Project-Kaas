using Photon.Pun;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using UnityEngine.Events;

namespace Collection.FriendList.Scripts
{
    public class FriendRequester : MonoBehaviourPun
    {
        #region Public Singleton

        public static FriendRequester Instance;

        #endregion

        #region UnityEvents

        public static readonly UnityEvent<string> OnAddSuccess = new UnityEvent<string>();
        public static readonly UnityEvent<string> OnRemoveSuccess = new UnityEvent<string>();
        public static readonly UnityEvent<string> OnRemoveFailed = new UnityEvent<string>();
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
        /// </summary>
        /// <param name="friendID">string</param>
        public static void AddFriend(string friendID)
        {
            // Add friend
            PlayFabClientAPI.AddFriend(new AddFriendRequest
                {
                    FriendPlayFabId = friendID
                },
                result =>
                {
                    // check if successful
                    if (result.Created)
                    {
                        OnAddSuccess?.Invoke(friendID);
                    }
                    else
                    {
                        Debug.Log($"add failed {friendID}");
                        OnAddFriendFailed?.Invoke($"Failed to add {friendID}");
                    }
                },
                error =>
                {
                    Debug.Log($"Add friend Failed: {error.ErrorMessage}");
                    OnAddFriendFailed?.Invoke(error.ErrorMessage);
                });
        }

        public static void DeleteFriend(string id)
        {
            PlayFabClientAPI.RemoveFriend(new RemoveFriendRequest
                {
                    FriendPlayFabId = id
                }, 
                result =>
            {
                Debug.Log("Friend removed");
                OnRemoveSuccess?.Invoke(id);
            }, 
                error =>
            {
                Debug.Log($"Remove friend Failed: {error.ErrorMessage}");
                OnRemoveFailed?.Invoke(id);
            });
        }

        #endregion
    }
}
