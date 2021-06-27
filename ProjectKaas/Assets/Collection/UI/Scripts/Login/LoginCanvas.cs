using System;
using Collection.Authentication.Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Collection.UI.Scripts.Login
{
    public class LoginCanvas : MonoBehaviour
    {
        #region Private Serializable Fields

        [SerializeField] private TMP_InputField emailInputField;
        [SerializeField] private TMP_InputField passwordInputField;
        [SerializeField] private Toggle stayLoggedIn;
        [SerializeField] private TMP_Text loginOutputText;

        #endregion

        #region Public Fields

        public TMP_InputField EmailInputField => emailInputField;
        public TMP_InputField PasswordInputField => passwordInputField;
        public Toggle StayLoggedIn => stayLoggedIn;
        public TMP_Text LoginOutputText => loginOutputText;

        #endregion

        #region Public Methods

        public void ClearUI()
        {
            LoginOutputText.text = String.Empty;
        }

        public void OnClickLogin()
        {
            //StartCoroutine(FirebaseAuthManager.Instance.LoginCo(EmailInputField.text, passwordInputField.text));
        }

        public void OnClickRegister()
        {
            ClearUI();
            AuthUIManager.Instance.RegisterCanvas.gameObject.SetActive(true);
        }

        #endregion
    }
}
