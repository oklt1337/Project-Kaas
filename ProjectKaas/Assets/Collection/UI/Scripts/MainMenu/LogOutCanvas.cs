using _Project.Scripts.PlayFab;
using Collection.Authentication.Scripts;
using Photon.Pun;
using UnityEngine;

namespace Collection.UI.Scripts.MainMenu
{
    public class LogOutCanvas : MonoBehaviour
    {
        #region Public Methods
        public void OnClickLogOut()
        {
            PlayFabLogin.Instance.Logout();
            
            PhotonNetwork.LoadLevel(1);
        }

        public void OnClickBack()
        {
            MainMenuCanvases.Instance.LogOutCanvas.gameObject.SetActive(false);
        }

        #endregion
    }
}
