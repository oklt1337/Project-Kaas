using Collection.NetworkPlayer.Scripts;
using ExitGames.Client.Photon;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Collection.UI.Scripts.MainMenu.Settings
{
    public class SettingsCanvas : MonoBehaviour
    {
        #region Private Serializable Fields
        
        [Header("General")]
        [SerializeField] private Button backButton;

        [Header("TabPanel")] 
        [SerializeField] private Button generalButton;
        [SerializeField] private Button audioButton;
        [SerializeField] private Button videoButton;

        [Header("GeneralPanel")] 
        [SerializeField] private GameObject generalPanel;
        [SerializeField] private Toggle controlSlider;
        [SerializeField] private TextMeshProUGUI controlText;

        [Header("AudioPanel")] 
        [SerializeField] private GameObject audioPanel;
        [SerializeField] private Slider masterSlider;
        [SerializeField] private Slider sfxSlider;
        [SerializeField] private Slider musicSlider;
        
        [Header("VideoPanel")] 
        [SerializeField] private GameObject videoPanel;

        #endregion

        #region MonoBehaviour Callbacks

        private void OnEnable()
        {
            generalPanel.SetActive(true);
            audioPanel.SetActive(false);
            videoPanel.SetActive(false);
            controlSlider.onValueChanged.AddListener(OnToggleControls);
        }

        #endregion

        #region Public Methods

        public void OnClickBack()
        {
            gameObject.SetActive(false);
        }

        public void OnClickGeneral()
        {
            generalPanel.SetActive(true);
            audioPanel.SetActive(false);
            videoPanel.SetActive(false);
        }
        
        public void OnClickAudio()
        {
            generalPanel.SetActive(false);
            audioPanel.SetActive(true);
            videoPanel.SetActive(false);
        }
        
        public void OnClickVideo()
        {
            generalPanel.SetActive(false);
            audioPanel.SetActive(false);
            videoPanel.SetActive(true);
        }

        public void OnToggleControls(bool toggle)
        {
            if (toggle)
            {
                //Joystick
                var hashtable = PhotonNetwork.LocalPlayer.CustomProperties;

                if (hashtable.ContainsKey("Controls"))
                {
                    hashtable.Remove("Controls");
                }
                
                hashtable.Add("Controls", Controls.Joystick);
            }
            else
            {
                //Tilt
                
                var hashtable = PhotonNetwork.LocalPlayer.CustomProperties;

                if (hashtable.ContainsKey("Controls"))
                {
                    hashtable.Remove("Controls");
                }
                
                hashtable.Add("Controls", Controls.Tilt);
            }
        }

        #endregion
    }
}
