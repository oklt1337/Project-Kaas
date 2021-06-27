using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.Events;

namespace Collection.Authentication.Scripts
{
    public class PlayFabAuthManager : MonoBehaviour
    {
        #region Public Singleton

        public static PlayFabAuthManager Instance;

        #endregion

        #region UnityEvents

        public static readonly UnityEvent OnLoginSuccess = new UnityEvent();
        public static readonly UnityEvent<string> OnLoginFailed = new UnityEvent<string>();
        public static readonly UnityEvent<string> OnCreateAccountFailed = new UnityEvent<string>();

        #endregion

        #region MonoBehaviour Callbacks

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        #endregion

        #region Public Methods

        public void CreateAccount(string username, string email, string password, string confirmationPassword)
        {
            if (confirmationPassword != password)
            {
                OnCreateAccountFailed?.Invoke("Passwords Are Not Matching");
            }
            else
            {
                // Register User.
                PlayFabClientAPI.RegisterPlayFabUser(
                    new RegisterPlayFabUserRequest
                    {
                        Email = email, Password = password, Username = username
                    },
                    response =>
                    {
                        Debug.Log($"Successful Account Creation: {username}, {email}");
                        SignIn(username, password);
                    },
                    error =>
                    {
                        Debug.Log($"Failed Account Creation: {username}, {email} \n {error.ErrorMessage}");
                        OnCreateAccountFailed?.Invoke(error.ErrorMessage);
                    });
            }
        }

        public void SignIn(string username, string password)
        {
            // login user.
            PlayFabClientAPI.LoginWithPlayFab(new LoginWithPlayFabRequest
                {
                    Password = password, Username = username
                },
                response =>
                {
                    Debug.Log($"Successful Account Login: {username}");
                    OnLoginSuccess?.Invoke();
                },
                error =>
                {
                    Debug.Log($"Failed to Login: {username} \n {error.ErrorMessage}");
                    OnLoginFailed?.Invoke(error.ErrorMessage);
                });
        }

        #endregion
    }
}
