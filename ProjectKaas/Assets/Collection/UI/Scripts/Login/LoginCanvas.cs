using System;
using System.Collections;
using Collection.Authentication.Scripts;
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
            PlayFabAuthManager.OnLoginFailed.AddListener(OnLoginFailed);
            PlayFabAuthManager.OnLoginSuccess.AddListener(OnLoginSuccess);
        }

        private void OnDisable()
        {
            PlayFabAuthManager.OnLoginFailed.RemoveListener(OnLoginFailed);
            PlayFabAuthManager.OnLoginSuccess.AddListener(OnLoginSuccess);
        }

        #endregion

        #region Private Methods

        private void OnLoginSuccess()
        {
            gameObject.SetActive(false);
        }

        private void OnLoginFailed(string error)
        {
            StartCoroutine(WarningCo(error));
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
            PlayFabAuthManager.Instance.SignIn(userNameInputField.text, passwordInputField.text);
        }

        public void OnClickRegister()
        {
            ClearUI();
            AuthUIManager.Instance.RegisterCanvas.gameObject.SetActive(true);
        }

        #endregion
    }
}
