using Unity.VisualScripting;
using UnityEngine;

namespace Collection.Cars.Scripts
{
    public class CarPrefabsHolder : MonoBehaviour
    {
        public enum Cars
        {
            Formula,
            Passenger
        }
        
        public static GameObject Formula;
        public static GameObject Passenger;
        [SerializeField] private GameObject formula;
        [SerializeField] private GameObject passenger;

        private void Awake()
        {
            Formula = formula;
            Passenger = passenger;
        }
    }
}
