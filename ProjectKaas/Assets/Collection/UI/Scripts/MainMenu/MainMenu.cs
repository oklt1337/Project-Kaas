using _Project.Scripts.PlayFab;
using Photon.Pun;
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
        [SerializeField] private Button profileButton;

        #endregion

        #region Public Fields

        public TextMeshProUGUI InfoText => infoText;

        #endregion

        #region Unity Methods

        private void Start()
        {
            friendListButton.gameObject.SetActive(PlayFabLogin.Instance.LoginStatus);
            profileButton.gameObject.SetActive(PlayFabLogin.Instance.LoginStatus);
        }

        #endregion

        #region Public Methods

        public void OnClickFriendList()
        {
            MainMenuCanvases.Instance.FriendListCanvas.gameObject.SetActive(!MainMenuCanvases.Instance.FriendListCanvas.gameObject.activeSelf);
        }
        
        public void OnClickRoomList()
        {
            OverlayCanvases.Instance.RoomListCanvas.gameObject.SetActive(true);
        }

        public void OnClickProfile()
        {
            MainMenuCanvases.Instance.ProfileCanvas.gameObject.SetActive(true);
        }

        public void OnClickSettings()
        {
            MainMenuCanvases.Instance.SettingsCanvas.gameObject.SetActive(true);
        }

        public void OnClickLogin()
        {
            if (!PlayFabClientAPI.IsClientLoggedIn())
            {
                PhotonNetwork.LoadLevel(1);
            }
            else
            {
                MainMenuCanvases.Instance.LogOutCanvas.gameObject.SetActive(true);
            }
        }

        #endregion
    }
}
