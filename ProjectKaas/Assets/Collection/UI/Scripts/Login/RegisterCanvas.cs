using System;
using System.Collections;
using Collection.Authentication.Scripts;
using TMPro;
using UnityEngine;

namespace Collection.UI.Scripts.Login
{
    public class RegisterCanvas : MonoBehaviour
    {
        #region Private Serializable Fields

        [SerializeField] private TMP_InputField emailInputField;
        [SerializeField] private TMP_InputField usernameInputField;
        [SerializeField] private TMP_InputField passwordInputField;
        [SerializeField] private TMP_InputField confirmedPasswordInputField;
        [SerializeField] private TMP_Text registryOutputText;

        #endregion

        #region Public Fields

        public TMP_InputField EmailInputField => emailInputField;
        public TMP_InputField UsernameInputField => usernameInputField;
        public TMP_InputField PasswordInputField => passwordInputField;
        public TMP_InputField ConfirmedPasswordInputField => confirmedPasswordInputField;
        public TMP_Text RegistryOutputText => registryOutputText;

        #endregion

        #region MonoBehaviour Callbacks

        private void OnEnable()
        {
            PlayFabAuthManager.OnCreateAccountFailed.AddListener(OnCreateAccountFailed);
        }

        private void OnDisable()
        {
            PlayFabAuthManager.OnCreateAccountFailed.RemoveListener(OnCreateAccountFailed);
        }

        #endregion
        
        #region Private Methods

        private void OnCreateAccountFailed(string error)
        {
            StartCoroutine(WarningCo(error));
        }

        private void ClearUI()
        {
            RegistryOutputText.text = String.Empty;
        }
        
        private IEnumerator WarningCo(string text)
        {
            RegistryOutputText.text = text;

            yield return new WaitForSeconds(5f);
            
            ClearUI();
        }

        #endregion

        #region Public Methods

        public void OnClickBack()
        {
            ClearUI();
            gameObject.SetActive(false);
        }

        public void OnClickRegister()
        {
            PlayFabAuthManager.Instance.CreateAccount(usernameInputField.text, emailInputField.text,
                passwordInputField.text, confirmedPasswordInputField.text);
        }

        #endregion
    }
}
