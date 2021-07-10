using Collection.Cars.Scripts;
using Collection.Cars.Scripts.FormulaCar;
using Collection.Items.Scripts;
using Collection.Maps.Scripts;
using Photon.Pun;
using Unity.VisualScripting;
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
        #region Private Serializeable Fields

        [SerializeField] private GameObject hudPrefab;
        [SerializeField] private GameObject audioListener;

        #endregion
        
        #region Public Fields

        public bool developerMode;
        
        public PlayerInputHandler PlayerInputHandler { get; private set; }

        public Car Car { get; private set; }
        
        public ItemBehaviour Item { get; private set; }
        
        public RaceState LocalRaceState { get; set; }
        
        public byte Position { get; set; }

        #endregion

        #region MonoBehaviour Callbacks

        private void Awake()
        {
            LocalRaceState = RaceState.PreStart;
            PlayerInputHandler = GetComponent<PlayerInputHandler>();

            if (photonView.IsMine)
            {
                // make sure only one audioLister
                audioListener.gameObject.SetActive(true);
            }
            
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
            var car = CarPrefabsHolder.Cars.Formula;
            
            if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("Car"))
            {
                car = (CarPrefabsHolder.Cars) PhotonNetwork.LocalPlayer.CustomProperties["Car"];
            }

            // Initialize car
            GameObject carObj;
            switch (car)
            {
                case CarPrefabsHolder.Cars.Formula:
                    carObj = Instantiate(CarPrefabsHolder.Formula, transform, false);
                    Car = carObj.GetComponent<FormulaCar>();

                    if (photonView.IsMine)
                    {
                        Car.ActivateCamera();
                        Car.Initialize(this);
                    }
                    break;
                default:
                    carObj = Instantiate(CarPrefabsHolder.Formula, transform, false);
                    Car = carObj.GetComponent<FormulaCar>();
                    
                    if (photonView.IsMine)
                    {
                        Car.ActivateCamera();
                        Car.Initialize(this);
                    }
                    break;
            }

            // Instantiate hud
            var hud = Instantiate(hudPrefab);
            // Initialize playerHandler
            PlayerInputHandler.Initialize(hud);

            // add to position manager
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
