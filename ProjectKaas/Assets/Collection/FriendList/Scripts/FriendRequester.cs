using System;
using System.Collections.Generic;
using System.Linq;
using Collection.Profile.Scripts;
using Collection.UI.Scripts;
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
        public static readonly UnityEvent<string> OnAddFriendFailed = new UnityEvent<string>();
        public static readonly UnityEvent<string> OnFriendRequestAccepted = new UnityEvent<string>();
        public static readonly UnityEvent<string> OnFriendRequestDeclined = new UnityEvent<string>();

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
            
            OnFriendRequestAccepted.AddListener(AddFriend);
        }

        #endregion

        #region Public Methods

        public void SendFriendRequest(PlayerProfileModel friend)
        {
            Debug.Log($"Friend request send to {friend.DisplayName}");
            
            // find requester user.
            var index = PhotonNetwork.PlayerList.ToList().FindIndex(x => x.UserId == friend.PlayerId);
            photonView.RPC("FriendRequestRPC", PhotonNetwork.PlayerList[index].Get(int.Parse(friend.PlayerId)), LocalProfile.Instance.PlayerProfileModel); 
        }

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

        #endregion

        #region Photon RPC

        [PunRPC]
        public void FriendRequestRPC(PlayerProfileModel requester)
        {
            MainMenuCanvases.Instance.FriendRequestCanvas.gameObject.SetActive(true);
            MainMenuCanvases.Instance.FriendRequestCanvas.Initialize(requester);
        }

        #endregion
    }
}
