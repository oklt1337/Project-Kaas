using System;
using System.Collections;
using Collection.Authentication.Scripts;
using Collection.LocalPlayerData.Scripts;
using PlayFab;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Collection.UI.Scripts.Login
{
    public class LoginCanvas : MonoBehaviour
    {
        #region Private Serializable Fields

        [SerializeField] private TMP_InputField userNameInputField;
        [SerializeField] private TMP_InputField passwordInputField;
        [SerializeField] private Toggle stayLoggedIn;
        [SerializeField] private TMP_Text loginOutputText;

        [Header("LoginData")]
        [SerializeField] private LoginData loginData;

        #endregion

        #region Public Fields

        public TMP_InputField UserNameInputField => userNameInputField;
        public TMP_InputField PasswordInputField => passwordInputField;
        public Toggle StayLoggedIn => stayLoggedIn;
        public TMP_Text LoginOutputText => loginOutputText;

        #endregion
        
        #region MonoBehaviour Callbacks

        private void OnEnable()
        {
            if (loginData.stayLogin)
            {
                var username = LocalPlayerDataManager.GetUserName();
                var password = LocalPlayerDataManager.GetPassword();

                if (username != String.Empty || password != String.Empty)
                {
                    PlayFabAuthManager.SignIn(username,password);
                }
            }
            
            PlayFabAuthManager.OnLoginFailed.AddListener(OnLoginFailed);
            PlayFabAuthManager.OnLoginSuccess.AddListener(OnLoginSuccess);
        }

        private void Awake()
        {
            PlayFabAuthManager.OnLogOut.AddListener(OnLogout);
        }

        private void OnDisable()
        {
            PlayFabAuthManager.OnLoginFailed.RemoveListener(OnLoginFailed);
            PlayFabAuthManager.OnLoginSuccess.AddListener(OnLoginSuccess);
        }

        #endregion

        #region Private Methods

        private void OnLogout()
        {
            loginData.stayLogin = false;
        }

        private void OnLoginSuccess()
        {
            MainMenuCanvases.Instance.MainMenu.gameObject.SetActive(true);
            AuthUIManager.Instance.RegisterCanvas.gameObject.SetActive(false);

            gameObject.SetActive(false);
        }

        private void OnLoginFailed(string error)
        {
            StartCoroutine(WarningCo(error));
            LocalPlayerDataManager.DeleteLoginData();
            loginData.stayLogin = false;
        }
        
        private void ClearUI()
        {
            LoginOutputText.text = String.Empty;
        }
        
        private IEnumerator WarningCo(string text)
        {
            LoginOutputText.text = text;

            yield return new WaitForSeconds(5f);
            
            ClearUI();
        }

        #endregion

        #region Public Methods

        public void OnClickLogin()
        {
            PlayFabAuthManager.SignIn(UserNameInputField.text, PasswordInputField.text);
            loginData.stayLogin = stayLoggedIn.isOn;
            
            if (StayLoggedIn.isOn)
            {
                LocalPlayerDataManager.SaveLoginData(UserNameInputField.text, PasswordInputField.text);
            }
            else
            {
                LocalPlayerDataManager.DeleteLoginData();
            }
        }

        public void OnClickRegister()
        {
            ClearUI();
            AuthUIManager.Instance.RegisterCanvas.gameObject.SetActive(true);
        }

        public void OnClickGuest()
        {
            AuthUIManager.Instance.LoginCanvas.gameObject.SetActive(false);
            MainMenuCanvases.Instance.MainMenu.gameObject.SetActive(true);
        }

        #endregion
    }
}
