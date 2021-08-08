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
        #region Private Fields

        #endregion

        #region Public Fields

        #endregion

        #region Events

        public static Action<List<global::Photon.Realtime.FriendInfo>> OnDisplayFriends = delegate { };

        #endregion

        #region Unity Methods

        private void Awake()
        {
            // TODO: PlayFabFriendController.
            PlayFabFriendController.OnFriendListUpdated += HandleFriendsUpdated;
        }

        private void OnDestroy()
        {
            PlayFabFriendController.OnFriendListUpdated -= HandleFriendsUpdated;
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
                OnDisplayFriends?.Invoke(photonFriends);
            }
        }

        #endregion

        #region Public Methods

        #endregion

        #region Photon Callbacks

        #endregion
    }
}
