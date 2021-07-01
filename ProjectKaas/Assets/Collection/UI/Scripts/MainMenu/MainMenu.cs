using Collection.UI.Scripts.Login;
using PlayFab;
using TMPro;
using UnityEngine;

namespace Collection.UI.Scripts.MainMenu
{
    public class MainMenu : MonoBehaviour
    {
        #region Public Methods

        public void OnClickRoomList()
        {
            OverlayCanvases.Instance.RoomListCanvas.gameObject.SetActive(true);
        }

        public void OnClickSettings()
        {
            // Settings
        }

        public void OnClickLogin()
        {
            if (!PlayFabClientAPI.IsClientLoggedIn())
            {
                AuthUIManager.Instance.LoginCanvas.gameObject.SetActive(true);
            }
            else
            {
                MainMenuCanvases.Instance.LogOutCanvas.gameObject.SetActive(true);
            }
        }

        #endregion
    }
}
