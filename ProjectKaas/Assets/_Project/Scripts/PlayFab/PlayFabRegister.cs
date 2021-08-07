using _Project.Scripts.Scene;
using Collection.LocalPlayerData.Scripts;
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
        /// Login in to PlayFab.
        /// </summary>
        public void Register()
        {
            if (!IsValidUserName() || !IsValidPassword()) return;
            RegisterPlayFabUserRequest();
        }
        
        #endregion

        #region PlayFab Callbacks

        private void OnRegisterPlayFabSuccess(RegisterPlayFabUserResult result)
        {
            Debug.Log($"Registration Successful: {result.PlayFabId}");
            PlayFabLogin.Instance.SetUserName(_userName);
            PlayFabLogin.Instance.SetPassword(_password);
            PlayFabLogin.Instance.LoginData.stayLogin = false;
            PlayFabLogin.Instance.Login();
        }
        
        private void OnFailedToRegister(PlayFabError error)
        {
            Debug.LogError($"ERROR {error.GenerateErrorReport()}");
        }

        #endregion
    }
}
