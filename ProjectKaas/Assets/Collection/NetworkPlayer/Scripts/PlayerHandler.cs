using Collection.Cars.Scripts;
using Collection.Maps.Scripts;
using Photon.Pun;
using UnityEngine;

namespace Collection.NetworkPlayer.Scripts
{
    [RequireComponent(typeof(PlayerInputHandler))]
    public class PlayerHandler : MonoBehaviourPun
    {
        #region Public Fields

        public PlayerInputHandler PlayerInputHandler { get; private set; }

        public Car Car { get; private set; }
        
        #endregion

        #region MonoBehaviour Callbacks

        private void Awake()
        {
            PlayerInputHandler = GetComponent<PlayerInputHandler>();
            Initialize();
        }

        #endregion

        #region Private Methods

        private void Initialize()
        {
            CarPrefabsHolder.Cars car = CarPrefabsHolder.Cars.Formula;
            
            if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("Car"))
            {
                car = (CarPrefabsHolder.Cars) PhotonNetwork.LocalPlayer.CustomProperties["Car"];
            }

            GameObject carObj;
            switch (car)
            {
                case CarPrefabsHolder.Cars.Formula:
                    carObj = Instantiate(CarPrefabsHolder.Formula, transform, false);
                    Car = carObj.GetComponent<Car>();
                    break;
                default:
                    carObj = Instantiate(CarPrefabsHolder.Formula, transform, false);
                   Car = carObj.GetComponent<Car>();
                    break;
            }
            
            PositionManager.PositionManagerInstance.AllPlayers.Add(this);
        }

        #endregion
    }
}
