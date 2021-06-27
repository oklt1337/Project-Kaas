using System;
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

        #region Public Methods

        public void ClearUI()
        {
            RegistryOutputText.text = String.Empty;
        }

        public void OnClickBack()
        {
            ClearUI();
            gameObject.SetActive(false);
        }

        public void OnClickRegister()
        {
            //StartCoroutine(FirebaseAuthManager.Instance.RegistryCo(usernameInputField.text, emailInputField.text,
               // passwordInputField.text, confirmedPasswordInputField.text));
        }

        #endregion
    }
}
