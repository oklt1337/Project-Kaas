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
        
        [SerializeField] private TMP_InputField userName;
        [SerializeField] private TMP_InputField email;
        [SerializeField] private TMP_InputField password;
        [SerializeField] private TMP_InputField verifyPassword;
        [SerializeField] private TextMeshProUGUI outputText;

        public static event Action OnClickRegisterSucess;

        private void OnDisable()
        {
            ClearInputFields();
        }

        public void OnClickRegister()
        {
            if (password != verifyPassword)
            {
                StartCoroutine(WarningCo("Non Matching Password."));
            }
            else
            {
                PlayFabRegister.SetUserName(userName.text);
                PlayFabRegister.SetEmail(email.text);
                PlayFabRegister.SetPassword(password.text);
                
                OnClickRegisterSucess?.Invoke();
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
        
        private IEnumerator WarningCo(string text)
        {
            outputText.text = text;

            yield return new WaitForSeconds(5f);
            
            outputText.text = String.Empty;
        }
    }
}
