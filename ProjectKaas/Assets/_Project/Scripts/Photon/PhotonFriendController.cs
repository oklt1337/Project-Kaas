using System;
using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.PlayFab;
using Photon.Pun;
using PlayFab.ClientModels;
using UnityEngine;

namespace _Project.Scripts.Photon
{
    public class PhotonFriendController : MonoBehaviourPunCallbacks
    {
        public static PhotonFriendController Instance;
        
        #region Private Fields

        #endregion

        #region Public Fields

        #endregion

        #region Events

        public readonly Action<List<global::Photon.Realtime.FriendInfo>> DisplayFriends = delegate { };

        #endregion

        #region Unity Methods

        private void Awake()
        {
            if (Instance != null)
                Destroy(gameObject);
            else
                Instance = this;
        }

        private void Start()
        {
            // TODO: PlayFabFriendController.
            PlayFabFriendController.Instance.OnFriendListUpdated += HandleFriendsUpdated;
        }

        private void OnDestroy()
        {
            PlayFabFriendController.Instance.OnFriendListUpdated -= HandleFriendsUpdated;
        }

        #endregion

        #region Private Methods

        private void HandleFriendsUpdated(List<FriendInfo> friends)
        {
            if (friends.Count != 0)
            {
                var friendDisplayNames = friends.Select(f => f.TitleDisplayName).ToArray();
                PhotonNetwork.FindFriends(friendDisplayNames);
            }
            else
            {
                var photonFriends = new List<global::Photon.Realtime.FriendInfo>();
                DisplayFriends?.Invoke(photonFriends);
            }
        }

        #endregion

        #region Public Methods

        #endregion

        #region Photon Callbacks

        #endregion
    }
}
