using Collection.Audio.Scripts;
using Collection.NetworkPlayer.Scripts;
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
        [SerializeField] private Slider ambientSoundMusicSlider;
        
        [Header("VideoPanel")] 
        [SerializeField] private GameObject videoPanel;

        #endregion

        #region MonoBehaviour Callbacks

        private void OnEnable()
        {
            backButton.onClick.AddListener(OnClickBack);
            
            generalButton.onClick.AddListener(OnClickGeneral);
            audioButton.onClick.AddListener(OnClickAudio);
            videoButton.onClick.AddListener(OnClickVideo);
            
            controlSlider.onValueChanged.AddListener(OnToggleControls);
            masterSlider.onValueChanged.AddListener(OnMasterVolumeChanged);
            musicSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
            sfxSlider.onValueChanged.AddListener(OnSfxVolumeChanged);
            ambientSoundMusicSlider.onValueChanged.AddListener(OnAmbientSoundVolumeChanged);
            
            generalPanel.SetActive(true);
            audioPanel.SetActive(false);
            videoPanel.SetActive(false);
        }

        private void OnDestroy()
        {
            backButton.onClick.AddListener(OnClickBack);
            
            generalButton.onClick.RemoveListener(OnClickGeneral);
            audioButton.onClick.RemoveListener(OnClickAudio);
            videoButton.onClick.RemoveListener(OnClickVideo);
            
            controlSlider.onValueChanged.RemoveListener(OnToggleControls);
            masterSlider.onValueChanged.AddListener(OnMasterVolumeChanged);
            musicSlider.onValueChanged.RemoveListener(OnMusicVolumeChanged);
            sfxSlider.onValueChanged.RemoveListener(OnSfxVolumeChanged);
            ambientSoundMusicSlider.onValueChanged.RemoveListener(OnAmbientSoundVolumeChanged);
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
        
        #endregion

        #region Private Methods

        private void OnToggleControls(bool toggle)
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

                controlText.text = "Joystick";
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
                
                controlText.text = "Tilt";
            }
        }

        private static void OnMasterVolumeChanged(float value)
        {
            AudioManager.Instance.SetMasterVolume(value * 20f);
        }

        private static void OnMusicVolumeChanged(float value)
        {
            AudioManager.Instance.SetMusicVolume(value * 20f);
        }

        private static void OnSfxVolumeChanged(float value)
        {
            AudioManager.Instance.SetSfxVolume(value * 20f);
        }

        private static void OnAmbientSoundVolumeChanged(float value)
        {
            AudioManager.Instance.SetAmbientSoundVolume(value * 20f);
        }

        #endregion
    }
}
