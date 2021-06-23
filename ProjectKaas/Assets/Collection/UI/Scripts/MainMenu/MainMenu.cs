using TMPro;
using UnityEngine;

namespace Collection.UI.Scripts.MainMenu
{
    public class MainMenu : MonoBehaviour
    {
        #region Private Serializable Fields

        [Tooltip("Gameobject witch contains MainMenu Canvas.")]
        [SerializeField] private GameObject mainMenuCanvases;
        
        #endregion

        #region Public Methods
        
        public void OnClickRoomList()
        {
            OverlayCanvases.Instance.RoomListCanvas.SetActive(true);
        }
        
        public void OnClickSettings()
        {
            mainMenuCanvases.SetActive(false);
        }

        public void OnClickBonus()
        {
            mainMenuCanvases.SetActive(false);
        }

        public void OnClickReward()
        {
            mainMenuCanvases.SetActive(false);
        }

        public void OnClickObjectives()
        {
            mainMenuCanvases.SetActive(false);
        }
        
        #endregion
    }
}
