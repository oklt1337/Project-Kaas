using Collection.Authentication.Scripts;
using Collection.UI.Scripts.MainMenu;
using Collection.UI.Scripts.MainMenu.FriendList;
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
        [SerializeField] private FriendRequestCanvas friendRequestCanvas;

        #endregion

        #region Public Fields

        public MainMenu.MainMenu MainMenu => mainMenu;
        public LogOutCanvas LogOutCanvas => logOutCanvas;
        public FriendListCanvas FriendListCanvas => friendListCanvas;
        public FriendInfoCanvas FriendInfoCanvas => friendInfoCanvas;
        public FriendRequestCanvas FriendRequestCanvas => friendRequestCanvas;

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
            
            PlayFabAuthManager.OnLoginSuccess.AddListener(MainMenu.ToggleFriendListButton);
            PlayFabAuthManager.OnLogOut.AddListener(MainMenu.ToggleFriendListButton);
        }

        #endregion
    }
}
