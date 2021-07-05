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

        private float _updateInterval;
        private const float StartInterval = 300f;

        #endregion

        #region Public Fields
        
        public List<FriendInfo> friendList = new List<FriendInfo>();
        public PlayerProfileModel PlayerProfileModel { get; private set; } = new PlayerProfileModel();
        public UserAccountInfo AccountInfo { get; private set; } = new UserAccountInfo();
        public Dictionary<string, UserDataRecord> UserData { get; private set; } = new Dictionary<string, UserDataRecord>();

        #endregion
        
        #region Events
        
        public static readonly UnityEvent OnProfileInitialized = new UnityEvent();
        public static readonly UnityEvent OnAccountInfoInitialized = new UnityEvent();
        public static readonly UnityEvent OnUserDataInitialized = new UnityEvent();
        public static readonly UnityEvent<List<FriendInfo>> OnFriendListUpdated = new UnityEvent<List<FriendInfo>>();
        public static readonly UnityEvent OnDisplayNameUpdated = new UnityEvent();
        public static readonly UnityEvent OnUserDataUpdated = new UnityEvent();
        
        #endregion

        #region MonoBehaviour Callbacks

        private void Awake()
        {
            friendList.Clear();
            _updateInterval = StartInterval;

            if (Instance != null)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
            }

            PlayFabAuthManager.OnLoginSuccess.AddListener(InitializeProfile);
            FriendRequester.OnSendFriendRequest.AddListener(UpdateFriendList);
            FriendRequester.OnRemoveSuccess.AddListener(UpdateFriendList);
            FriendRequester.OnFriendRequestAccepted.AddListener(UpdateFriendList);
            FriendRequester.OnFriendRequestDenied.AddListener(UpdateFriendList);
        }

        private void Update()
        {
            ConstUpdates();
        }

        #endregion

        #region Private Methods

        #region Initializing Methods

        /// <summary>
        /// Initialize userprofile
        /// </summary>
        private void InitializeProfile()
        {
            GetProfile();
            GetAccountInfo();
            GetUserData();
            UpdateFriendList(null);
        }

        private void GetProfile()
        {
            PlayFabClientAPI.GetPlayerProfile(new GetPlayerProfileRequest(),
                result =>
                {
                    PlayerProfileModel = result.PlayerProfile; 
                    OnProfileInitialized?.Invoke();
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
                    OnAccountInfoInitialized?.Invoke();
                },
                error => { { Debug.LogError($"Cant Get AccountInfo: {error.ErrorMessage}"); } });
        }

        private void GetUserData()
        {
            PlayFabClientAPI.GetUserData(new GetUserDataRequest(),
                result =>
                {
                    UserData = result.Data;
                    OnUserDataInitialized?.Invoke();
                },
                error => { Debug.LogError($"Cant Get UserData: {error.ErrorMessage}"); });
        }

        #endregion

        private void UpdateFriendList(string id)
        {
            friendList.Clear();

            PlayFabClientAPI.GetFriendsList(new GetFriendsListRequest(),
                result =>
                {
                    friendList = result.Friends;
                    Debug.Log("Friend list updated.");
                    OnFriendListUpdated?.Invoke(friendList);
                }, error => { Debug.LogError($"FriendList not found: {error.ErrorMessage}"); });
        }

        private void ConstUpdates()
        {
            _updateInterval -= Time.deltaTime;
            if (!(_updateInterval <= 0)) return;
            UpdateFriendList(null);
            _updateInterval = StartInterval;
        }

        #endregion

        #region Public Methods

        public void ChangeProfileDisplayName(string newName)
        {
            if (string.IsNullOrEmpty(newName)) return;

            PlayFabClientAPI.UpdateUserTitleDisplayName(new UpdateUserTitleDisplayNameRequest
                { DisplayName = newName },
                result =>
                {
                    if (result.DisplayName == newName)
                    {
                        OnDisplayNameUpdated?.Invoke();
                    }
                }, error => { Debug.LogError($"Failed to update UserDisplayName: {error.ErrorMessage}"); });
        }

        public void UpdateUserData(Dictionary<string, string> updatedData)
        {
            PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest
                {
                    Data = updatedData
                }, 
                result => { OnUserDataUpdated?.Invoke(); }, 
                error => { Debug.LogError($"Failed to update UserData: {error.ErrorMessage}"); });
        }
        
        #endregion
    }
}
