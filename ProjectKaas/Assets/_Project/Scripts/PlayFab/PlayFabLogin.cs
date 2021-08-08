using System;
using _Project.Scripts.Photon;
using _Project.Scripts.Scene;
using _Project.Scripts.UI.PlayFab;
using Collection.LocalPlayerData.Scripts;
using Photon.Pun;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

namespace _Project.Scripts.PlayFab
{
    public class PlayFabLogin : MonoBehaviour
    {
        #region Serializable Fields

        [Header("LoginData")]
        [SerializeField] private LoginData loginData;
        
        #endregion
        
        #region Private Fields

        private static string _userName;
        private static string _password;
        private static LoginData _loginData;

        #endregion

        #region Public Fields

        #endregion

        #region Public Events

        public static event Action OnLoginSuccess;

        #endregion

        #region Unity Methods

        private void Awake()
        {
            _loginData = loginData;
        }

        private void Start()
        {
            PlayFabRegister.OnRegisterSuccess += Login;
            LoginCanvas.OnClickLoginSuccess += Login;
            LoginCanvas.OnClickGuestSuccess += DeletePlayerPrefsData;

            if (string.IsNullOrEmpty(PlayFabSettings.TitleId))
            {
                PlayFabSettings.TitleId = "4FCAD";
            }

            LoginWithSavedData();
        }

        private void OnDestroy()
        {
            PlayFabRegister.OnRegisterSuccess -= Login;
            LoginCanvas.OnClickLoginSuccess -= Login;
            LoginCanvas.OnClickGuestSuccess -= DeletePlayerPrefsData;
        }

        #endregion

        #region Private Methods

        private void LoginWithSavedData()
        {
            if (!_loginData.stayLogin) return;
            
            Debug.Log("Login with saved player data.");
            SetUserName(PlayerPrefs.GetString("USERNAME"));
            SetPassword(PlayerPrefs.GetString("PASSWORD"));
            Login();
        }
        
        /// <summary>
        /// Login in to PlayFab.
        /// </summary>
        private void Login()
        {
            if (!IsValidUserName() || !IsValidPassword()) return;
            LoginWithPlayFabRequest();
        }

        private static void DeletePlayerPrefsData()
        {
            Debug.Log("Deleting PlayerPref data.");
            PlayerPrefs.DeleteAll();
        }
        
        /// <summary>
        /// Check if Username is Valid
        /// for a Username to be valid it must be
        /// more then 3 and less than 24 characters.
        /// </summary>
        /// <returns>if userName is valid or not (bool)</returns>
        private static bool IsValidUserName()
        {
            return _userName.Length >= 3 && _userName.Length <= 24;
        }

        /// <summary>
        /// Check if Password is Valid
        /// for a Password to be valid it must have
        /// more then 6 character.
        /// </summary>
        /// <returns>if password is valid or not (bool)</returns>
        private static bool IsValidPassword()
        {
            return _password.Length >= 6;
        }

        /// <summary>
        /// Sending Request to PlayFab with userName and Password.
        /// If Request Fails get Callback with OnFailedToLogin
        /// If Request is Successful get Callback with OnLoginPlayFabSuccess
        /// </summary>
        private void LoginWithPlayFabRequest()
        {
            Debug.Log($"Login to PlayFab as {_userName}");

            var request = new LoginWithPlayFabRequest
            {
                Username = _userName,
                Password = _password
            };
            PlayFabClientAPI.LoginWithPlayFab(request, OnLoginPlayFabSuccess, OnFailedToLogin);
        }

        /// <summary>
        /// Updating DisplayName of PlayFab.
        /// </summary>
        /// <param name="displayName">string new displayName</param>
        private void UpdateDisplayName(string displayName)
        {
            Debug.Log($"Updating PlayFab Accounts DisplayName: {displayName}");

            var request = new UpdateUserTitleDisplayNameRequest
            {
                DisplayName = displayName
            };
            PlayFabClientAPI.UpdateUserTitleDisplayName(request, OnDisplayNameUpdateSuccess, OnFailedToUpdateDisplayName);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Setting Username and saving it to PlayerPrefs with Key: USERNAME
        /// </summary>
        /// <param name="newUserName">Username to Login to PlayFab</param>
        public static void SetUserName(string newUserName)
        {
            _userName = newUserName;

            if (!_loginData.stayLogin)
            {
                PlayerPrefs.SetString("USERNAME" ,_userName);
            }
        }

        /// <summary>
        /// Setting Password and saving it to PlayerPrefs with Key: PASSWORD
        /// </summary>
        /// <param name="newPassword">Password to Login to PlayFab</param>
        public static void SetPassword(string newPassword)
        {
            _password = newPassword;

            if (!_loginData.stayLogin)
            {
                PlayerPrefs.SetString("PASSWORD" ,_password);
            }
        }

        /// <summary>
        /// Setting Stay Signed in bool in LoginData.
        /// </summary>
        /// <param name="staySignedIn">Bool that represents if player want to stay signed in for the next time he opens the app</param>
        public static void SetLoginData(bool staySignedIn)
        {
            _loginData.stayLogin = staySignedIn;
        }

        #endregion

        #region PlayFab Callbacks

        private void OnLoginPlayFabSuccess(LoginResult result)
        {
            Debug.Log($"Login Successful: {result.PlayFabId}");
            UpdateDisplayName(_userName);
            PhotonConnector.UpdatePlayerData(_userName , result.PlayFabId);
            
            OnLoginSuccess?.Invoke();
            //PhotonNetwork.LoadLevel(2);
        }
        
        private void OnFailedToLogin(PlayFabError error)
        {
            Debug.LogError($"ERROR {error.GenerateErrorReport()}");
        }
        
        private void OnDisplayNameUpdateSuccess(UpdateUserTitleDisplayNameResult result)
        {
            Debug.Log($"DisplayName Updated Successful: {result.DisplayName}");
        }
        
        private void OnFailedToUpdateDisplayName(PlayFabError error)
        {
            Debug.LogError($"ERROR {error.GenerateErrorReport()}");
        }

        #endregion
    }
}
