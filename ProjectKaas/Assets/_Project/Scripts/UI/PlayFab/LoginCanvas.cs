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
        
        [SerializeField] private TMP_InputField userName;
        [SerializeField] private TMP_InputField password;
        [SerializeField] private Toggle stayLogin;
        [SerializeField] private TextMeshProUGUI outputText;

        public static event Action OnClickLoginSuccess;
        public static event Action OnClickGuestSuccess;

        private void OnDisable()
        {
            ClearInputFields();
        }

        public void OnClickLogin()
        {
            if (userName.text == String.Empty || password.text == String.Empty)
            {
                StartCoroutine(WarningCo("Username or Password can't be empty."));
            }
            else
            {
                PlayFabLogin.SetUserName(userName.text);
                PlayFabLogin.SetPassword(password.text);
                PlayFabLogin.SetLoginData(stayLogin);
                
                OnClickLoginSuccess?.Invoke();
            }
        }

        public void OnClickRegister()
        {
            registerCanvas.SetActive(true);
            gameObject.SetActive(false);
        }

        public void OnClickGuest()
        {
            OnClickGuestSuccess?.Invoke();
        }
        
        private void ClearInputFields()
        {
            userName.text = String.Empty;
            password.text = String.Empty;
            outputText.text = String.Empty;
        }
        
        private IEnumerator WarningCo(string text)
        {
            ClearInputFields();
            outputText.text = text;

            yield return new WaitForSeconds(5f);
            
            outputText.text = String.Empty;
        }
    }
}
