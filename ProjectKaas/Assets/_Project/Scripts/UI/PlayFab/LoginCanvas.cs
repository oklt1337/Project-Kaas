using System;
using System.Collections;
using _Project.Scripts.PlayFab;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.UI.PlayFab
{
    public class LoginCanvas : MonoBehaviour
    {
        [SerializeField] private GameObject registerCanvas;
        [SerializeField] private TextMeshProUGUI outputText;
        
        [Header("Inputs")]
        [SerializeField] private TMP_InputField userName;
        [SerializeField] private TMP_InputField password;

        [Header("Buttons")] 
        [SerializeField] private Button loginButton;
        [SerializeField] private Button registerButton;
        [SerializeField] private Toggle autoLogin;
        

        public static event Action<string,string,bool?> OnClickLoginButton;

        private void Start()
        {
            PlayFabLogin.Instance.OnLoginFailed += FailedLogin;
        }

        private void OnEnable()
        {
            InteractableStatus(true);
        }

        private void OnDisable()
        {
            ClearInputFields();
            PlayFabLogin.Instance.OnLoginFailed -= FailedLogin;
        }

        public void OnClickLogin()
        {
            if (userName.text == string.Empty || password.text == string.Empty)
            {
                StartCoroutine(WarningCo("Username or Password can't be empty."));
            }
            else
            {
                InteractableStatus(false);
                
                OnClickLoginButton?.Invoke(userName.text, password.text, autoLogin);
            }
        }

        public void OnClickRegister()
        {
            InteractableStatus(false);
            
            registerCanvas.SetActive(true);
            gameObject.SetActive(false);
        }

        private void ClearInputFields()
        {
            userName.text = string.Empty;
            password.text = string.Empty;
            outputText.text = string.Empty;
        }

        private void InteractableStatus(bool status)
        {
            userName.interactable = status;
            password.interactable = status;
            loginButton.interactable = status;
            registerButton.interactable = status;
            autoLogin.interactable = status;
        }

        private void FailedLogin(string errorMessage)
        {
            InteractableStatus(true);
            StartCoroutine(WarningCo(errorMessage));
        }
        
        private IEnumerator WarningCo(string text)
        {
            ClearInputFields();
            outputText.text = text;

            yield return new WaitForSeconds(5f);
            
            outputText.text = string.Empty;
        }
    }
}
