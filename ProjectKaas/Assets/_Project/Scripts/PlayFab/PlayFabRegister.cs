using System;
using _Project.Scripts.UI.PlayFab;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

namespace _Project.Scripts.PlayFab
{
    public class PlayFabRegister : MonoBehaviour
    {
        public static PlayFabRegister Instance;
        
        #region Serializable Fields

        #endregion
        
        #region Private Fields
        
        private string _userName;
        private string _email;
        private string _password;

        #endregion

        #region Public Fields

        #endregion

        #region Pubic Events

        public event Action<string,string,bool?> OnRegisterSuccess;
        public event Action<string> OnRegisterFailed;

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
            RegisterCanvas.OnClickRegisterButton += Register;
        }

        private void OnDestroy()
        {
            RegisterCanvas.OnClickRegisterButton -= Register;
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
        /// Setting Username
        /// </summary>
        /// <param name="newUserName">Username to Login to PlayFab</param>
        public void SetUserName(string newUserName)
        {
            _userName = newUserName;
        }

        /// <summary>
        /// Setting Email
        /// </summary>
        /// <param name="newEmail"></param>
        public void SetEmail(string newEmail)
        {
            _email = newEmail;
        }

        /// <summary>
        /// Setting Password
        /// </summary>
        /// <param name="newPassword">Password to Login to PlayFab</param>
        public void SetPassword(string newPassword)
        {
            _password = newPassword;
        }

        /// <summary>
        /// Sending Request to PlayFab with userName and Password.
        /// If Request Fails get Callback with OnFailedToLogin
        /// If Request is Successful get Callback with OnLoginPlayFabSuccess
        /// </summary>
        private void RegisterPlayFabUserRequest()
        {
            Debug.Log($"Register to PlayFab as {_userName}");

            var request = new RegisterPlayFabUserRequest
            {
                Username = _userName,
                Email = _email,
                Password = _password
            };
            PlayFabClientAPI.RegisterPlayFabUser(request, OnRegisterPlayFabSuccess, OnFailedToRegister);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Login in to PlayFab.
        /// </summary>
        /// <param name="email">email register with</param>
        /// <param name="userName">userName registers with</param>
        /// <param name="password">password registers with</param>
        public void Register(string email, string userName, string password)
        {
            SetEmail(email);
            SetUserName(userName);
            SetPassword(password);
            
            if (!IsValidUserName() || !IsValidPassword()) return;
            RegisterPlayFabUserRequest();
        }
        
        #endregion

        #region PlayFab Callbacks

        private void OnRegisterPlayFabSuccess(RegisterPlayFabUserResult result)
        {
            Debug.Log($"Registration Successful: {result.PlayFabId}");

            OnRegisterSuccess?.Invoke(_userName, _password, null);
        }
        
        private void OnFailedToRegister(PlayFabError error)
        {
            Debug.LogError($"ERROR {error.GenerateErrorReport()}");

            OnRegisterFailed?.Invoke(error.GenerateErrorReport());
        }

        #endregion
    }
}
