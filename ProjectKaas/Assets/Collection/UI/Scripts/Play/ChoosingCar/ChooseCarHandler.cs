using System;
using Collection.Cars.Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Collection.UI.Scripts.Play.ChoosingCar
{
    public enum ChooseCar
    {
        Formula,
        Baywatch,
        Passenger,
        Veteran
    }
    
    public class ChooseCarHandler : MonoBehaviour
    {
        #region Private Serilalizable Fields

        [Header("General")]
        [SerializeField] private Button forward;
        [SerializeField] private Button back;
        [SerializeField] private TextMeshProUGUI carName;
        
        [Header("Formula")] 
        [SerializeField] private GameObject formula;
        
        [Header("Baywatch")]
        [SerializeField] private GameObject baywatch;
        
        [Header("Passenger")]
        [SerializeField] private GameObject passenger;
        
        [Header("Veteran")]
        [SerializeField] private GameObject veteran;

        #endregion

        #region Private Fields

        private const string FormulaCarText = "Formula Car";
        private const string BaywatchCarText = "Baywatch Car";
        private const string PassengerCarText = "Passenger Car";
        private const string VeteranCarText = "Veteran Car";

        #endregion

        #region Public Fields

        public ChooseCar Car { get; set; }

        #endregion

        #region Monobehaviour Callbacks

        private void OnEnable()
        {
            formula.SetActive(true);
            baywatch.SetActive(false);
            passenger.SetActive(false);
            veteran.SetActive(false);

            carName.text = FormulaCarText;
            Car = ChooseCar.Formula;
        }

        #endregion

        #region Private Methods

        public void OnClickForward()
        {
            switch (Car)
            {
                case ChooseCar.Formula:
                    formula.SetActive(false);
                    baywatch.SetActive(true);

                    carName.text = BaywatchCarText;
                    Car = ChooseCar.Baywatch;
                    break;
                case ChooseCar.Baywatch:
                    baywatch.SetActive(false);
                    passenger.SetActive(true);
                    
                    carName.text = PassengerCarText;
                    Car = ChooseCar.Passenger;
                    break;
                case ChooseCar.Passenger:
                    passenger.SetActive(false);
                    veteran.SetActive(true);
                    
                    carName.text = VeteranCarText;
                    Car = ChooseCar.Veteran;
                    break;
                case ChooseCar.Veteran:
                    veteran.SetActive(false);
                    formula.SetActive(true);
                    
                    carName.text = FormulaCarText;
                    Car = ChooseCar.Formula;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void OnClickBack()
        {
            switch (Car)
            {
                case ChooseCar.Formula:
                    formula.SetActive(false);
                    veteran.SetActive(true);
                    
                    carName.text = VeteranCarText;
                    Car = ChooseCar.Veteran;
                    
                    break;
                case ChooseCar.Baywatch:
                    baywatch.SetActive(false);
                    formula.SetActive(true);
                    
                    carName.text = FormulaCarText;
                    Car = ChooseCar.Formula;
                    break;
                case ChooseCar.Passenger:
                    passenger.SetActive(false);
                    baywatch.SetActive(true);
                    
                    carName.text = BaywatchCarText;
                    Car = ChooseCar.Baywatch;
                    break;
                case ChooseCar.Veteran:
                    veteran.SetActive(false);
                    passenger.SetActive(true);
                    
                    carName.text = PassengerCarText;
                    Car = ChooseCar.Passenger;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void DeactivateCars()
        {
            switch (Car)
            {
                case ChooseCar.Formula:
                    formula.SetActive(!formula.activeSelf);
                    break;
                case ChooseCar.Baywatch:
                    baywatch.SetActive(!baywatch.activeSelf);
                    break;
                case ChooseCar.Passenger:
                    passenger.SetActive(!passenger.activeSelf);
                    break;
                case ChooseCar.Veteran:
                    veteran.SetActive(!veteran.activeSelf);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void SetButtonInteractableState()
        {
            forward.interactable = !forward.interactable;
            back.interactable = !back.interactable;
        }

        #endregion
    }
}
