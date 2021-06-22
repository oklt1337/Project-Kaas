using TMPro;
using UnityEngine;

namespace Collection.UI.Scripts.MainMenu
{
    public class MainMenu : MonoBehaviour
    {
        #region Private Serializable Fields

        [Tooltip("Gameobject witch contains MainMenu Canvas.")]
        [SerializeField] private GameObject mainMenuCanvases;
        
        [Tooltip("Gameobject witch contains all overlay canvases for playing.")]
        [SerializeField] private GameObject overlayCanvases;

        #endregion

        #region Public Methods
        
        public void OnClickSettings()
        {
            overlayCanvases.SetActive(true);
            mainMenuCanvases.SetActive(false);
        }
        
        public void OnClickLogin()
        {
            overlayCanvases.SetActive(true);
            mainMenuCanvases.SetActive(false);
        }
        
        public void OnClickPlay()
        {
            overlayCanvases.SetActive(true);
            mainMenuCanvases.SetActive(false);
        }
        
        public void OnClickLevels()
        {
            overlayCanvases.SetActive(true);
            mainMenuCanvases.SetActive(false);
        }
        
        public void OnClickBonus()
        {
            overlayCanvases.SetActive(true);
            mainMenuCanvases.SetActive(false);
        }

        public void OnClickReward()
        {
            overlayCanvases.SetActive(true);
            mainMenuCanvases.SetActive(false);
        }
        
        public void OnClickObjectives()
        {
            overlayCanvases.SetActive(true);
            mainMenuCanvases.SetActive(false);
        }
        
        #endregion
    }
}
