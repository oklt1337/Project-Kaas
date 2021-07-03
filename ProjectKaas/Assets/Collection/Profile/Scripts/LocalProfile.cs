using System.Collections.Generic;
using Collection.Authentication.Scripts;
using Collection.FriendList.Scripts;
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

        public static List<FriendInfo> FriendList = new List<FriendInfo>();

        #endregion
        
        #region Public Fields

        public PlayerProfileModel PlayerProfileModel { get; private set; } = new PlayerProfileModel();
        public static readonly UnityEvent<PlayerProfileModel> OnProfileInitialized = new UnityEvent<PlayerProfileModel>();
        public static readonly UnityEvent<List<FriendInfo>> OnFriendListUpdated = new UnityEvent<List<FriendInfo>>();

        #endregion

        #region MonoBehaviour Callbacks

        private void Awake()
        {
            FriendList.Clear();
            
            if (Instance != null)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
            }
            PlayFabAuthManager.OnLoginSuccess.AddListener(InitializeProfile);
            FriendRequester.OnAddSuccess.AddListener(UpdateFriendList);
            FriendRequester.OnRemoveSuccess.AddListener(UpdateFriendList);
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

            UpdateFriendList(null);
        }

        private static void UpdateFriendList(string id)
        {
            FriendList.Clear();
            
            PlayFabClientAPI.GetFriendsList(new GetFriendsListRequest(), 
                result =>
                {
                    FriendList = result.Friends;
                    Debug.Log("Friend list updated.");
                    OnFriendListUpdated?.Invoke(FriendList);
                }, 
                error =>
                {
                    Debug.LogError($"FriendList not found: {error.ErrorMessage}");
                });
        }

        #endregion
    }
}
