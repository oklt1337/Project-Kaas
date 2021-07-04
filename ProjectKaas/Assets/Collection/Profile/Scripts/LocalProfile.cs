using System;
using System.Collections.Generic;
using Collection.Authentication.Scripts;
using Collection.FriendList.Scripts;
using Collection.UI.Scripts.Play.Room;
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

        public List<FriendInfo> friendList = new List<FriendInfo>();

        #endregion

        #region Public Fields

        public PlayerProfileModel PlayerProfileModel { get; private set; } = new PlayerProfileModel();
        public UserAccountInfo AccountInfo { get; private set; } = new UserAccountInfo();

        #region Events

        #region Public Events

        public static readonly UnityEvent<PlayerProfileModel> OnProfileFullyInitialized =
            new UnityEvent<PlayerProfileModel>();
        public static readonly UnityEvent<List<FriendInfo>> OnFriendListUpdated = new UnityEvent<List<FriendInfo>>();
        public static readonly UnityEvent OnDisplayNameChanged = new UnityEvent();

        #endregion

        #region Private Events

        private readonly UnityEvent _onProfileInitialized = new UnityEvent();

        #endregion

        #endregion

        #endregion

        #region MonoBehaviour Callbacks

        private void Awake()
        {
            friendList.Clear();

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
            _onProfileInitialized.AddListener(GetAccountInfo);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Initialize userprofile
        /// </summary>
        private void InitializeProfile()
        {
            GetProfile();
            UpdateFriendList(null);
        }

        private void UpdateFriendList(string id)
        {
            friendList.Clear();

            PlayFabClientAPI.GetFriendsList(new GetFriendsListRequest(),
                result =>
                {
                    friendList = result.Friends;
                    Debug.Log("Friend list updated.");
                    OnFriendListUpdated?.Invoke(friendList);
                },
                error => { Debug.LogError($"FriendList not found: {error.ErrorMessage}"); });
        }

        private void GetProfile()
        {
            PlayFabClientAPI.GetPlayerProfile(new GetPlayerProfileRequest(),
                result =>
                {
                    PlayerProfileModel = result.PlayerProfile;
                    _onProfileInitialized?.Invoke();
                },
                error => { Debug.LogError($"Cant Get UserProfile: {error.ErrorMessage}"); });
        }

        private void GetAccountInfo()
        {
            PlayFabClientAPI.GetAccountInfo(new GetAccountInfoRequest
                {
                    PlayFabId = PlayerProfileModel.PlayerId
                },
                result =>
                {
                    AccountInfo = result.AccountInfo;

                    OnProfileFullyInitialized?.Invoke(PlayerProfileModel);
                },
                error =>
                {
                    {
                        Debug.LogError($"Cant Get AccountInfo: {error.ErrorMessage}");
                    }
                });
        }

        #endregion

        #region Public Methods

        public void ChangeProfileDisplayName(string newName)
        {
            if (string.IsNullOrEmpty(newName)) return;

            PlayFabClientAPI.UpdateUserTitleDisplayName(new UpdateUserTitleDisplayNameRequest
                {
                    DisplayName = newName
                },
                result =>
                {
                    if (result.DisplayName == newName)
                    {
                        OnDisplayNameChanged?.Invoke();
                    }
                },
                error => { Debug.LogError(error.ErrorMessage); });
        }

        #endregion
    }
}
