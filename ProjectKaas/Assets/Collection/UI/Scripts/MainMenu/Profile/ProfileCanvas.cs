using System;
using Collection.Profile.Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Collection.UI.Scripts.MainMenu.Profile
{
    public class ProfileCanvas : MonoBehaviour
    {
        #region Private Serializable Fields

        [Header("General")] 
        [SerializeField] private TMP_InputField displayNameText;
        [SerializeField] private TextMeshProUGUI displayNamePlaceholderText;
        [SerializeField] private RawImage profilePicImage;
        
        [Header("Stats")]
        [SerializeField] private RawImage mostUsedMapImage;
        [SerializeField] private TextMeshProUGUI mostUsedMapText;
        
        [Header("Placements")]
        [SerializeField] private TextMeshProUGUI firstCountText;
        [SerializeField] private TextMeshProUGUI secondCountText;
        [SerializeField] private TextMeshProUGUI thirdCountText;

        [Header("Data")]
        [SerializeField] private TextMeshProUGUI registerDateText;
        [SerializeField] private TextMeshProUGUI topThreePercentageText;
        [SerializeField] private Slider percentageSlider;
        [SerializeField] private TextMeshProUGUI mostUsedCar;

        #endregion

        #region MonoBehaviour Callbacks

        private void OnEnable()
        {
            Initialize();
        }

        #endregion

        #region Private Methods

        public void Initialize()
        {
            displayNamePlaceholderText.text = LocalProfile.Instance.PlayerProfileModel.DisplayName;
            
            registerDateText.text = LocalProfile.Instance.AccountInfo.Created.ToShortDateString();
        }

        #endregion

        #region Public Methods

        public void ClearPlaceHolderText()
        {
            displayNamePlaceholderText.text = String.Empty;
        }
        
        public void ChangeName()
        {
            displayNamePlaceholderText.text = displayNameText.text;
            LocalProfile.Instance.ChangeProfileDisplayName(displayNameText.text);
            displayNameText.text = String.Empty;
        }

        public void OnClickBack()
        {
            gameObject.SetActive(false);
        }

        #endregion
    }
}
