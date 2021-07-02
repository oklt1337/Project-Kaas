using Photon.Pun;
using TMPro;
using UnityEngine;

namespace Collection.UI.Scripts
{
    [RequireComponent(typeof(TMP_InputField))]
    public class PlayerNameInputField : MonoBehaviour
    {
        #region Private Serializable Fields

        /// <summary>
        /// The Name of the Player text.
        /// </summary>
        [Tooltip("Name of the Player text.")]
        [SerializeField] private TextMeshProUGUI playerName;
        
        /// <summary>
        /// Button to activate the InputField.
        /// </summary>
        [Tooltip("Button to activate the InputField.")]
        [SerializeField] private GameObject renameButton;

        #endregion

        #region Private Fields

        private TMP_InputField _inputField;

        #endregion
        
        #region Private Constants
        
        // Store the PlayerPref Key to avoid typos
        private const string PlayerNamePrefKey = "PlayerName";
        
        #endregion
        
        #region MonoBehaviour CallBacks

        private void Start () 
        {
            var defaultName = string.Empty;
            _inputField = GetComponent<TMP_InputField>();

            if (_inputField!=null)
            {
                // Make sure player didnt already has a saved name.
                if (PlayerPrefs.HasKey(PlayerNamePrefKey))
                {
                    defaultName = PlayerPrefs.GetString(PlayerNamePrefKey);
                    _inputField.gameObject.SetActive(false);
                    renameButton.SetActive(true);
                }
            }

            playerName.text = defaultName;
            PhotonNetwork.NickName = defaultName;
        }

        #endregion
        
        #region Public Methods
        
        /// <summary>
        /// Sets the name of the player, and save it in the PlayerPrefs.
        /// </summary>
        /// <param name="value">The name of the Player</param>
        public void SetPlayerName(string value)
        {
            // Make sure Input isn't null or Empty.
            if (string.IsNullOrEmpty(value))
            {
                Debug.LogError("Player Name is null or empty");
                return;
            }
            PhotonNetwork.NickName = value;
            playerName.text = value;
            
            // Set PlayerPrefs value.
            PlayerPrefs.SetString(PlayerNamePrefKey, value);

            _inputField.text = string.Empty;
            _inputField.gameObject.SetActive(false);
            renameButton.SetActive(true);
        }

        /// <summary>
        /// Activate InputField to allow rename.
        /// </summary>
        public void RequestRename()
        {
            _inputField.gameObject.SetActive(true);
            renameButton.SetActive(false);
        }

        #endregion
    }
}
