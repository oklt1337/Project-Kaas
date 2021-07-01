using Collection.Authentication.Scripts;
using Collection.UI.Scripts.Login;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;

namespace Collection.UI.Scripts.MainMenu
{
    public class LogOutCanvas : MonoBehaviour
    {
        #region Public Methods
        public void OnClickLogOut()
        {
            PlayFabAuthManager.LogOut();
            MainMenuCanvases.Instance.LogOutCanvas.gameObject.SetActive(false);
        }

        public void OnClickBack()
        {
            MainMenuCanvases.Instance.LogOutCanvas.gameObject.SetActive(false);
        }

        #endregion
    }
}
