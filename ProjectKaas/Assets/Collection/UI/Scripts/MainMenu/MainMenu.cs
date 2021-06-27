using Collection.UI.Scripts.Login;
using TMPro;
using UnityEngine;

namespace Collection.UI.Scripts.MainMenu
{
    public class MainMenu : MonoBehaviour
    {
        #region Private Serializable Fields

        [Tooltip("Gameobject witch contains MainMenu Canvas.")] [SerializeField]
        private GameObject mainMenuCanvases;

        #endregion

        #region Public Methods

        public void OnClickRoomList()
        {
            OverlayCanvases.Instance.RoomListCanvas.gameObject.SetActive(true);
        }

        public void OnClickSettings()
        {
            mainMenuCanvases.SetActive(false);
        }

        public void OnClickLogin()
        {
            AuthUIManager.Instance.LoginCanvas.gameObject.SetActive(true);
        }

        #endregion
    }
}
