using System;
using System.Numerics;
using Collection.Profile.Scripts;
using Collection.UI.Scripts.Play.ChoosingCar;
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
        [SerializeField] private Image mostUsedMapImage;
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

        [Header("MapPic")]
        [SerializeField] private Sprite cityImage;
        [SerializeField] private Sprite japanImage;
        [SerializeField] private Sprite mountainsImage;

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
            MapStuff();
            CarStuff();
            PlacementStuff();
        }

        private void MapStuff()
        {
            var cityAmount = 0;
            var japanAmount = 0;
            var mountainsAmount = 0;
            
            if (LocalProfile.Instance.UserData.ContainsKey(LocalProfile.Data.City.ToString()))
            {
                cityAmount = int.Parse(LocalProfile.Instance.UserData[LocalProfile.Data.City.ToString()].Value);
            }
            if (LocalProfile.Instance.UserData.ContainsKey(LocalProfile.Data.Japan.ToString()))
            {
                japanAmount = int.Parse(LocalProfile.Instance.UserData[LocalProfile.Data.Japan.ToString()].Value);
            }
            if (LocalProfile.Instance.UserData.ContainsKey(LocalProfile.Data.Mountains.ToString()))
            {
                mountainsAmount = int.Parse(LocalProfile.Instance.UserData[LocalProfile.Data.Mountains.ToString()].Value);
            }

            if (cityAmount > japanAmount)
            {
                if (cityAmount > mountainsAmount)
                {
                    mostUsedMapImage.sprite = cityImage;
                    mostUsedMapText.text = LocalProfile.Data.City.ToString();
                }
                else
                {
                    mostUsedMapImage.sprite = mountainsImage;
                    mostUsedMapText.text = LocalProfile.Data.Mountains.ToString();
                }
            }
            else
            {
                if (japanAmount < mountainsAmount)
                {
                    mostUsedMapImage.sprite = mountainsImage;
                    mostUsedMapText.text = LocalProfile.Data.Mountains.ToString();
                }
                else
                {
                    mostUsedMapImage.sprite = japanImage;
                    mostUsedMapText.text = LocalProfile.Data.Japan.ToString();
                }
            }
        }

        private void CarStuff()
        {
            var formulaAmount = 0;
            var veteranAmount = 0;
            var passengerAmount = 0;
            var baywatchAmount = 0;
            
            if (LocalProfile.Instance.UserData.ContainsKey(ChooseCar.Formula.ToString()))
            {
                formulaAmount = int.Parse(LocalProfile.Instance.UserData[ChooseCar.Formula.ToString()].Value);
            }
            if (LocalProfile.Instance.UserData.ContainsKey(ChooseCar.Veteran.ToString()))
            {
                veteranAmount = int.Parse(LocalProfile.Instance.UserData[ChooseCar.Veteran.ToString()].Value);
            }
            if (LocalProfile.Instance.UserData.ContainsKey(ChooseCar.Passenger.ToString()))
            {
                passengerAmount = int.Parse(LocalProfile.Instance.UserData[ChooseCar.Passenger.ToString()].Value);
            }
            if (LocalProfile.Instance.UserData.ContainsKey(ChooseCar.Baywatch.ToString()))
            {
                baywatchAmount = int.Parse(LocalProfile.Instance.UserData[ChooseCar.Baywatch.ToString()].Value);
            }

            var largest = Mathf.Max(formulaAmount, veteranAmount, passengerAmount, baywatchAmount);
            if (largest == formulaAmount)
            {
                mostUsedCar.text = ChooseCar.Formula.ToString();
            }
            else if (largest == veteranAmount)
            {
                mostUsedCar.text = ChooseCar.Veteran.ToString();
            }
            else if (largest == passengerAmount)
            {
                mostUsedCar.text = ChooseCar.Passenger.ToString();
            }
            else if (largest == baywatchAmount)
            {
                mostUsedCar.text = ChooseCar.Baywatch.ToString();
            }
        }

        private void PlacementStuff()
        {
            var cityAmount = 0;
            var japanAmount = 0;
            var mountainsAmount = 0;
            
            if (LocalProfile.Instance.UserData.ContainsKey(LocalProfile.Data.City.ToString()))
            {
                cityAmount = int.Parse(LocalProfile.Instance.UserData[LocalProfile.Data.City.ToString()].Value);
            }
            if (LocalProfile.Instance.UserData.ContainsKey(LocalProfile.Data.Japan.ToString()))
            {
                japanAmount = int.Parse(LocalProfile.Instance.UserData[LocalProfile.Data.Japan.ToString()].Value);
            }
            if (LocalProfile.Instance.UserData.ContainsKey(LocalProfile.Data.Mountains.ToString()))
            {
                mountainsAmount = int.Parse(LocalProfile.Instance.UserData[LocalProfile.Data.Mountains.ToString()].Value);
            }

            var totalRaces = cityAmount + japanAmount + mountainsAmount;
            var first = 0;
            var second = 0;
            var third = 0;
            if (LocalProfile.Instance.UserData.ContainsKey(LocalProfile.Data.First.ToString()))
            {
                firstCountText.text = LocalProfile.Instance.UserData[LocalProfile.Data.First.ToString()].Value;
                first = int.Parse(firstCountText.text);
            }
            if (LocalProfile.Instance.UserData.ContainsKey(LocalProfile.Data.Second.ToString()))
            {
                secondCountText.text = LocalProfile.Instance.UserData[LocalProfile.Data.Second.ToString()].Value;
                second = int.Parse(secondCountText.text);
            }
            if (LocalProfile.Instance.UserData.ContainsKey(LocalProfile.Data.Third.ToString()))
            {
                thirdCountText.text = LocalProfile.Instance.UserData[LocalProfile.Data.Third.ToString()].Value;
                third = int.Parse(thirdCountText.text);
            }
            var totalPlace = first + second + third;

            var percent = totalPlace / totalRaces * 100;
            percentageSlider.value = percent;

            topThreePercentageText.text = percent + "%";
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
