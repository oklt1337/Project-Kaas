using Unity.VisualScripting;
using UnityEngine;

namespace Collection.Cars.Scripts
{
    public class CarPrefabsHolder : MonoBehaviour
    {
        public enum Cars
        {
            Formula
        }
        
        public static GameObject Formula;

        [SerializeField] private GameObject formula;
        private void Awake()
        {
            Formula = formula;
        }
    }
}
