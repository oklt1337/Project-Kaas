using _Project.Scripts.Scene;
using Collection.LocalPlayerData.Scripts;
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

        #endregion

        #region Public Fields

        public LoginData LoginData => loginData;

        #endregion

        #region Unity Methods

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
        }

        private void Start()
        {
            if (string.IsNullOrEmpty(PlayFabSettings.TitleId))
            {
                PlayFabSettings.TitleId = "4FCAD";
            }
            
            if (!LoginData.stayLogin) return;
            SetUserName(PlayerPrefs.GetString("USERNAME"));
            SetPassword(PlayerPrefs.GetString("PASSWORD"));
            Login();
        }

        #endregion

        #region Private Methods

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
        /// Setting Username and saving it to PlayerPrefs with Key: USERNAME
        /// </summary>
        /// <param name="newUserName">Username to Login to PlayFab</param>
        public void SetUserName(string newUserName)
        {
            _userName = newUserName;

            if (!loginData.stayLogin)
            {
                PlayerPrefs.SetString("USERNAME" ,_userName);
            }
        }

        /// <summary>
        /// Setting Password and saving it to PlayerPrefs with Key: PASSWORD
        /// </summary>
        /// <param name="newPassword">Password to Login to PlayFab</param>
        public void SetPassword(string newPassword)
        {
            _password = newPassword;

            if (!loginData.stayLogin)
            {
                PlayerPrefs.SetString("PASSWORD" ,_password);
            }
        }

        /// <summary>
        /// Login in to PlayFab.
        /// </summary>
        public void Login()
        {
            if (!IsValidUserName() || !IsValidPassword()) return;
            LoginWithPlayFabRequest();
        }
        
        #endregion

        #region PlayFab Callbacks

        private void OnLoginPlayFabSuccess(LoginResult result)
        {
            Debug.Log($"Login Successful: {result.PlayFabId}");
            UpdateDisplayName(_userName);
        }
        
        private void OnFailedToLogin(PlayFabError error)
        {
            Debug.LogError($"ERROR {error.GenerateErrorReport()}");
        }
        
        private void OnDisplayNameUpdateSuccess(UpdateUserTitleDisplayNameResult result)
        {
            Debug.Log($"DisplayName Updated Successful: {result.DisplayName}");
            SceneController.LoadScene(!loginData.stayLogin ? (byte) 1 : (byte) 2);
        }
        
        private void OnFailedToUpdateDisplayName(PlayFabError error)
        {
            Debug.LogError($"ERROR {error.GenerateErrorReport()}");
        }

        #endregion
    }
}
