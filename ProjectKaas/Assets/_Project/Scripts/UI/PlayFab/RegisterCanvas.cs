using System;
using System.Collections;
using _Project.Scripts.PlayFab;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.UI.PlayFab
{
    public class RegisterCanvas : MonoBehaviour
    {
        [SerializeField] private GameObject loginCanvas;
        [SerializeField] private TextMeshProUGUI outputText;
        
        [Header("Inputs")]
        [SerializeField] private TMP_InputField userName;
        [SerializeField] private TMP_InputField email;
        [SerializeField] private TMP_InputField password;
        [SerializeField] private TMP_InputField verifyPassword;

        [Header("Buttons")] 
        [SerializeField] private Button registerButton;
        [SerializeField] private Button backButton;

        public static event Action<string,string,string> OnClickRegisterButton;

        private void OnEnable()
        {
            InteractableStatus(true);
        }

        private void Start()
        {
            PlayFabRegister.Instance.OnRegisterFailed += FailedRegister;
        }

        private void OnDisable()
        {
            ClearInputFields();
            
            PlayFabRegister.Instance.OnRegisterFailed -= FailedRegister;
        }

        public void OnClickRegister()
        {
            if (password != verifyPassword)
            {
                StartCoroutine(WarningCo("Non Matching Password."));
            }
            else
            {
                InteractableStatus(false);
                
                OnClickRegisterButton?.Invoke(email.text, userName.text, password.text);
            }
        }

        public void OnClickBack()
        {
            loginCanvas.SetActive(true);
            gameObject.SetActive(false);
        }
        
        private void ClearInputFields()
        {
            email.text = String.Empty;
            userName.text = String.Empty;
            password.text = String.Empty;
            verifyPassword.text = String.Empty;
            outputText.text = String.Empty;
        }
        
        private void InteractableStatus(bool status)
        {
            userName.interactable = status;
            email.interactable = status;
            password.interactable = status;
            verifyPassword.interactable = status;
            registerButton.interactable = status;
            backButton.interactable = status;
        }

        private void FailedRegister(string errorMessage)
        {
            InteractableStatus(true);
            StartCoroutine(WarningCo(errorMessage));
        }
        
        private IEnumerator WarningCo(string text)
        {
            outputText.text = text;

            yield return new WaitForSeconds(5f);
            
            outputText.text = String.Empty;
        }
    }
}
