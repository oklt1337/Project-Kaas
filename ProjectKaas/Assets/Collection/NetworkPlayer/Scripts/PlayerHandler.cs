using Collection.Cars.Scripts;
using Collection.Items.Scripts;
using Collection.Maps.Scripts;
using Photon.Pun;
using UnityEngine;

namespace Collection.NetworkPlayer.Scripts
{
    public enum RaceState
    {
        PreStart,
        Race
    }
    
    [RequireComponent(typeof(PlayerInputHandler))]
    public class PlayerHandler : MonoBehaviourPun
    {
        public PlayerHandler(ItemBehaviour item)
        {
            Item = item;
        }

        #region Public Fields

        public PlayerInputHandler PlayerInputHandler { get; private set; }

        public Car Car { get; private set; }
        
        public ItemBehaviour Item { get; private set; }
        
        public RaceState LocalRaceState { get; set; }

        #endregion

        #region MonoBehaviour Callbacks

        private void Awake()
        {
            LocalRaceState = RaceState.PreStart;
            
            PlayerInputHandler = GetComponent<PlayerInputHandler>();
            Initialize();
        }

        #endregion

        #region Internal Methods

        internal void UseItem()
        {
            if (Item != null)
            {
                Item.OnUse();
                Item = null;
            }
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

        #region Public Methods

        public void SetItem(ItemBehaviour newItem)
        {
            Item = newItem;
        }

        #endregion
    }
}
