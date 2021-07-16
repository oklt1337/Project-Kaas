using Collection.Authentication.Scripts;
using Collection.Profile.Scripts;
using Collection.UI.Scripts.MainMenu;
using Collection.UI.Scripts.MainMenu.FriendList;
using Collection.UI.Scripts.MainMenu.Profile;
using Collection.UI.Scripts.MainMenu.Settings;
using UnityEngine;

namespace Collection.UI.Scripts
{
    public class MainMenuCanvases : MonoBehaviour
    {
        #region Singleton

        public static MainMenuCanvases Instance;

        #endregion

        #region Private Serializable Fields

        [SerializeField] private MainMenu.MainMenu mainMenu;
        [SerializeField] private LogOutCanvas logOutCanvas;
        [SerializeField] private FriendListCanvas friendListCanvas;
        [SerializeField] private FriendInfoCanvas friendInfoCanvas;
        [SerializeField] private ProfileCanvas profileCanvas;
        [SerializeField] private SettingsCanvas settingsCanvas;

        #endregion

        #region Public Fields

        public MainMenu.MainMenu MainMenu => mainMenu;
        public LogOutCanvas LogOutCanvas => logOutCanvas;
        public FriendListCanvas FriendListCanvas => friendListCanvas;
        public FriendInfoCanvas FriendInfoCanvas => friendInfoCanvas;
        public ProfileCanvas ProfileCanvas => profileCanvas;
        public SettingsCanvas SettingsCanvas => settingsCanvas;

        #endregion

        #region MonoBehaviour Callbacks

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
            }

            PlayFabAuthManager.OnLoginSuccess.AddListener(MainMenu.ToggleButtonOnlyForLoggedUser);
            PlayFabAuthManager.OnLogOut.AddListener(MainMenu.ToggleButtonOnlyForLoggedUser);
            LocalProfile.OnProfileInitialized.AddListener(FriendListCanvas.SetIDText);
            LocalProfile.OnFriendListUpdated.AddListener(FriendListCanvas.FriendListLayoutGroup.UpdateFriendList);
        }

        #endregion
    }
}
