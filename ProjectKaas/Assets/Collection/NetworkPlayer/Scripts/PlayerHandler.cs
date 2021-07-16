using Collection.Cars.Scripts;
using Collection.Cars.Scripts.BaywatchCar;
using Collection.Cars.Scripts.FormulaCar;
using Collection.Cars.Scripts.PassengerCar;
using Collection.Cars.Scripts.VeteranCar;
using Collection.Items.Scripts;
using Collection.Maps.Scripts;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace Collection.NetworkPlayer.Scripts
{
    public enum RaceState
    {
        PreStart,
        Race
    }

    public enum Controls
    {
        Joystick,
        Tilt
    }
    
    [RequireComponent(typeof(PlayerInputHandler))]
    public class PlayerHandler : MonoBehaviourPun
    {
        #region Private Serializeable Fields

        [SerializeField] private GameObject hudPrefab;
        [SerializeField] private GameObject audioListener;
        [SerializeField] private ItemBehaviour item;

        #endregion
        
        #region Public Fields

        public bool developerMode;
        
        public PlayerInputHandler PlayerInputHandler { get; private set; }

        public Car Car { get; private set; }
        
        public CanvasHandler CanvasHandler { get; private set; }

        public ItemBehaviour Item => item;
        
        public RaceState LocalRaceState { get; set; }
        
        public byte Position { get; set; }
        
        public Controls Controls { get; private set; }
        
        public Player LocalPlayer { get; private set; }
        
        
        #endregion

        #region MonoBehaviour Callbacks

        private void Awake()
        {
            LocalRaceState = RaceState.PreStart;
            PlayerInputHandler = GetComponent<PlayerInputHandler>();

            if (photonView.IsMine)
            {
                if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("Controls"))
                {
                    Controls = (Controls) PhotonNetwork.LocalPlayer.CustomProperties["Controls"];
                }
                else
                {
                    Controls = Controls.Joystick;
                }
                
                // make sure only one audioLister
                audioListener.gameObject.SetActive(true);
                LocalPlayer = PhotonNetwork.LocalPlayer; 
            }

            Initialize();
        }

        #endregion

        #region Internal Methods

        internal void UseItem()
        {
            if (Item == null) 
                return;
            
            Item.OnUse();
            item = null;
        }

        #endregion

        #region Private Methods

        private void Initialize()
        {
            var car = CarPrefabsHolder.Cars.Veteran;
            
            if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("Car"))
            {
                car = (CarPrefabsHolder.Cars) PhotonNetwork.LocalPlayer.CustomProperties["Car"];
            }

            // Initialize car
            GameObject carObj;
            Vector3 localPosition;
            switch (car)
            {
                case CarPrefabsHolder.Cars.Formula:
                    carObj = Instantiate(CarPrefabsHolder.Formula, transform, false);
                    
                    // Fix Position
                    localPosition = carObj.transform.localPosition;
                    localPosition = new Vector3(localPosition.x,
                        localPosition.y - 0.4f, localPosition.z);
                    carObj.transform.localPosition = localPosition;
                    
                    // get Component
                    Car = carObj.GetComponent<FormulaCar>();

                    // make sure only one cam in scene.
                    if (photonView.IsMine)
                    {
                        Car.ActivateCamera();
                        Car.Initialize(this);
                    }
                    else
                    {
                        Car.DeactivateComponents();
                    }
                    break;
                case CarPrefabsHolder.Cars.Passenger:
                    carObj = Instantiate(CarPrefabsHolder.Passenger, transform, false);
                    
                    // Fix Position
                    localPosition = carObj.transform.localPosition;
                    localPosition = new Vector3(localPosition.x,
                        localPosition.y - 0.4f, localPosition.z);
                    carObj.transform.localPosition = localPosition;
                    
                    // get Component
                    Car = carObj.GetComponent<PassengerCar>();

                    // make sure only one cam in scene.
                    if (photonView.IsMine)
                    {
                        Car.ActivateCamera();
                        Car.Initialize(this);
                    }
                    else
                    {
                        Car.DeactivateComponents();
                    }
                    
                    break;
                case CarPrefabsHolder.Cars.Baywatch:
                    carObj = Instantiate(CarPrefabsHolder.Baywatch, transform, false);
                    
                    // Fix Position
                    localPosition = carObj.transform.localPosition;
                    localPosition = new Vector3(localPosition.x,
                        localPosition.y - 0.4f, localPosition.z);
                    carObj.transform.localPosition = localPosition;
                    
                    // get Component
                    Car = carObj.GetComponent<Baywatch>();

                    // make sure only one cam in scene.
                    if (photonView.IsMine)
                    {
                        Car.ActivateCamera();
                        Car.Initialize(this);
                    }
                    else
                    {
                        Car.DeactivateComponents();
                    }
                    break;
                case CarPrefabsHolder.Cars.Veteran:
                    carObj = Instantiate(CarPrefabsHolder.Veteran, transform, false);
                    
                    // Fix Position
                    localPosition = carObj.transform.localPosition;
                    localPosition = new Vector3(localPosition.x,
                        localPosition.y - 0.4f, localPosition.z);
                    carObj.transform.localPosition = localPosition;
                    
                    // get Component
                    Car = carObj.GetComponent<Veteran>();

                    // make sure only one cam in scene.
                    if (photonView.IsMine)
                    {
                        Car.ActivateCamera();
                        Car.Initialize(this);
                    }
                    else
                    {
                        Car.DeactivateComponents();
                    }
                    break;
                default:
                    carObj = Instantiate(CarPrefabsHolder.Formula, transform, false);
                    
                    // Fix Position
                    localPosition = carObj.transform.localPosition;
                    localPosition = new Vector3(localPosition.x,
                        localPosition.y - 0.4f, localPosition.z);
                    carObj.transform.localPosition = localPosition;
                    
                    // get Component
                    Car = carObj.GetComponent<FormulaCar>();
                    
                    // make sure only one cam in scene.
                    if (photonView.IsMine)
                    {
                        Car.ActivateCamera();
                        Car.Initialize(this);
                    }
                    else
                    {
                        Car.DeactivateComponents();
                    }
                    break;
            }

            if (photonView.IsMine)
            {
                // Instantiate hud
                var hudObj = Instantiate(hudPrefab);
                CanvasHandler = hudObj.GetComponent<CanvasHandler>();
                CanvasHandler.ChangeControls(Controls);
                
                // Initialize playerHandler
                PlayerInputHandler.Initialize(CanvasHandler.Joystick, CanvasHandler.ItemButton, CanvasHandler.GasButton);
            }

            // add to position manager
            PositionManager.PositionManagerInstance.AllPlayers.Add(this);
        }

        #endregion

        #region Public Methods

        public void SetItem(ItemBehaviour newItem)
        {
            item = newItem;
        }

        public void ChangeHud(Controls controls)
        {
            Controls = controls;
            CanvasHandler.ChangeControls(Controls);
        }

        #endregion
    }
}
