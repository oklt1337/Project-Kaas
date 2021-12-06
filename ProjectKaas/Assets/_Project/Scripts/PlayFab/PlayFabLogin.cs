using System;
using _Project.Scripts.Photon;
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
        public static PlayFabLogin Instance;
        
        #region Serializable Fields

        [Header("LoginData")]
        [SerializeField] private LoginData loginData;
        
        #endregion
        
        #region Private Fields

        private string _userName;
        private string _password;
        private LoginData _loginData;

        #endregion

        #region Public Fields

        public bool LoginStatus { get; private set; }
        public bool AutoLogin => loginData.autoLogin;

        #endregion

        #region Public Events

        public event Action<string,string> OnLoginSuccess;
        public event Action OnLogoutSuccess;

        public event Action<string> OnLoginFailed; 

        #endregion

        #region Unity Methods

        private void Awake()
        {
            if (Instance != null)
                Destroy(gameObject);
            else
                Instance = this;
            
            _loginData = loginData;
        }

        private void Start()
        {
            PlayFabRegister.Instance.OnRegisterSuccess += Login;
            LoginCanvas.OnClickLoginButton += Login;

            if (string.IsNullOrEmpty(PlayFabSettings.TitleId))
            {
                PlayFabSettings.TitleId = "4FCAD";
            }

            LoginWithSavedData();
        }

        private void OnDestroy()
        {
            PlayFabRegister.Instance.OnRegisterSuccess -= Login;
            LoginCanvas.OnClickLoginButton -= Login;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Setting Username and saving it to PlayerPrefs with Key: USERNAME
        /// </summary>
        /// <param name="newUserName">Username to Login to PlayFab</param>
        private void SetUserName(string newUserName)
        {
            _userName = newUserName;

            if (!_loginData.autoLogin)
            {
                PlayerPrefs.SetString("USERNAME" ,_userName);
            }
        }

        /// <summary>
        /// Setting Password and saving it to PlayerPrefs with Key: PASSWORD
        /// </summary>
        /// <param name="newPassword">Password to Login to PlayFab</param>
        private void SetPassword(string newPassword)
        {
            _password = newPassword;

            if (!_loginData.autoLogin)
            {
                PlayerPrefs.SetString("PASSWORD" ,_password);
            }
        }

        /// <summary>
        /// Setting Stay Signed in bool in LoginData.
        /// </summary>
        /// <param name="autoLogin">Bool that represents if player want to stay signed in for the next time he opens the app</param>
        private void SetLoginData(bool autoLogin)
        {
            _loginData.autoLogin = autoLogin;
        }
        
        private void LoginWithSavedData()
        {
            if (!_loginData.autoLogin) return;
            
            Debug.Log("Login with saved player data.");
            Login(PlayerPrefs.GetString("USERNAME"), PlayerPrefs.GetString("PASSWORD"), _loginData.autoLogin);
        }
        
        /// <summary>
        /// Login in to PlayFab.
        /// </summary>
        /// <param name="userName">userName to Login with</param>
        /// <param name="password">password to Login with</param>
        /// <param name="autoLogin">bool that determined if on next login will be automatically logs in</param>
        private void Login(string userName, string password, bool? autoLogin)
        {
            SetUserName(userName);
            SetPassword(password);
            
            // make sure bool is not null so no error occurs
            if (autoLogin != null)
            {
                SetLoginData((bool) autoLogin);
            }
            
            if (!IsValidUserName() || !IsValidPassword()) return;
            
            Debug.Log("Login");
            LoginWithPlayFabRequest();
        }

        private void DeletePlayerPrefsData()
        {
            PlayerPrefs.DeleteAll();
            loginData.autoLogin = false;
        }
        
        /// <summary>
        /// Check if Username is Valid
        /// for a Username to be valid it must be
        /// more then 3 and less than 24 characters.
        /// </summary>
        /// <returns>if userName is valid or not (bool)</returns>
        private bool IsValidUserName()
        {
            return _userName.Length >= 3 && _userName.Length <= 24;
        }

        /// <summary>
        /// Check if Password is Valid
        /// for a Password to be valid it must have
        /// more then 6 character.
        /// </summary>
        /// <returns>if password is valid or not (bool)</returns>
        private bool IsValidPassword()
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
        /// Logs you off of PlayFab.
        /// </summary>
        public void Logout()
        {
            Debug.Log("Logout");
            DeletePlayerPrefsData();
            OnLogoutSuccess?.Invoke();
        }
        
        #endregion

        #region PlayFab Callbacks

        private void OnLoginPlayFabSuccess(LoginResult result)
        {
            Debug.Log($"Login Successful: {result.PlayFabId}");

            LoginStatus = true;
            UpdateDisplayName(_userName);
            
            OnLoginSuccess?.Invoke(_userName, result.PlayFabId);
            PhotonNetwork.LoadLevel(2);
        }
        
        private void OnFailedToLogin(PlayFabError error)
        {
            Debug.LogError($"ERROR {error.GenerateErrorReport()}");
            OnLoginFailed?.Invoke(error.GenerateErrorReport());
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
