using Unity.VisualScripting;
using UnityEngine;

namespace Collection.Cars.Scripts
{
    public class CarPrefabsHolder : MonoBehaviour
    {
        public enum Cars
        {
            Formula,
            Passenger,
            Baywatch,
            Veteran
        }
        
        public static GameObject Formula;
        public static GameObject Passenger;
        public static GameObject Baywatch;
        public static GameObject Veteran;
        [SerializeField] private GameObject formula;
        [SerializeField] private GameObject passenger;
        [SerializeField] private GameObject baywatch;
        [SerializeField] private GameObject veteran;

        private void Awake()
        {
            Formula = formula;
            Passenger = passenger;
            Baywatch = baywatch;
            Veteran = veteran;
        }
    }
}
