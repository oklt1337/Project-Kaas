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
                PlayFabLogin.Instance.SetUserName(userName.text);
                PlayFabLogin.Instance.SetPassword(password.text);
                PlayFabLogin.Instance.LoginData.stayLogin = stayLogin;
                PlayFabLogin.Instance.Login();
            }
        }

        public void OnClickRegister()
        {
            registerCanvas.SetActive(true);
            gameObject.SetActive(false);
        }

        public void OnClickGuest()
        {
            
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
