using System.Collections;
using System.Collections.Generic;
using Collection.Authentication.Scripts;
using Photon.Chat;
using Photon.Pun;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using UnityEngine.Events;

namespace Collection.Profile.Scripts
{
    public class LocalProfile : MonoBehaviour
    {

        #region Private Fields

        private List<FriendInfo> _friendList = new List<FriendInfo>();

        #endregion
        
        #region Public Fields

        public PlayerProfileModel PlayerProfileModel { get; private set; } = new PlayerProfileModel();
        public static readonly UnityEvent<PlayerProfileModel> OnProfileInitialized = new UnityEvent<PlayerProfileModel>();

        #endregion

        #region MonoBehaviour Callbacks

        private void Awake()
        {
            PlayFabAuthManager.OnLoginSuccess.AddListener(InitializeProfile);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Initialize userprofile
        /// </summary>
        private void InitializeProfile()
        {
            PlayFabClientAPI.GetPlayerProfile(new GetPlayerProfileRequest(), 
                result =>
                {
                    PlayerProfileModel = result.PlayerProfile;
                    PhotonNetwork.LocalPlayer.NickName = result.PlayerProfile.DisplayName;
                    var authenticationValues = new Photon.Realtime.AuthenticationValues(result.PlayerProfile.PlayerId);
                    PhotonNetwork.AuthValues = authenticationValues;
                }, 
                error =>
                {
                    Debug.LogError($"Cant Get UserProfile: {error.ErrorMessage}");
                });
            
            PlayFabClientAPI.GetFriendsList(new GetFriendsListRequest(), 
                result =>
                {
                    _friendList = result.Friends;
                }, 
                error =>
                {
                    Debug.LogError($"FriendList not found: {error.ErrorMessage}");
                });
        }

        #endregion
    }
}
