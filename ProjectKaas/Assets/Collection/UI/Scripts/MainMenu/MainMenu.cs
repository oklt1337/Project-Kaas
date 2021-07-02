using Collection.Authentication.Scripts;
using Collection.UI.Scripts.Login;
using PlayFab;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Collection.UI.Scripts.MainMenu
{
    public class MainMenu : MonoBehaviour
    {
        #region Private Serializable Fields

        [Tooltip("UI Label to inform the user")] [SerializeField]
        private TextMeshProUGUI infoText;

        [SerializeField] private Button friendListButton;

        #endregion

        #region Public Fields

        public TextMeshProUGUI InfoText => infoText;

        #endregion

        #region Public Methods

        public void ToggleFriendListButton()
        {
            friendListButton.gameObject.SetActive(!friendListButton.gameObject.activeSelf);
        }

        public void OnClickFriendList()
        {
            MainMenuCanvases.Instance.FriendListCanvas.gameObject.SetActive(!MainMenuCanvases.Instance.FriendListCanvas.gameObject.activeSelf);
        }
        
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
