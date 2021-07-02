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
        #region Singleton

        public static LocalProfile Instance;

        #endregion
        
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
            if (Instance != null)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
            }
            
            PlayFabAuthManager.OnLoginSuccess.AddListener(InitializeProfile);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Initialize userprofile
        /// </summary>
        private void InitializeProfile()
        {
            Debug.Log("Initialize...");
            
            PlayFabClientAPI.GetPlayerProfile(new GetPlayerProfileRequest(), 
                result =>
                {
                    Debug.Log("Initializing Profile Successful");
                    
                    PlayerProfileModel = result.PlayerProfile;
                    OnProfileInitialized?.Invoke(PlayerProfileModel);
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
