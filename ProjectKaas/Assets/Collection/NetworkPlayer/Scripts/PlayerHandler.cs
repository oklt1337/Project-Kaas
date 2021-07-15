using Collection.Cars.Scripts;
using Collection.Cars.Scripts.FormulaCar;
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

        #endregion
        
        #region Public Fields

        public bool developerMode;
        
        public PlayerInputHandler PlayerInputHandler { get; private set; }

        public Car Car { get; private set; }
        
        public CanvasHandler CanvasHandler { get; private set; }
        
        public ItemBehaviour Item { get; private set; }
        
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

            // TODO: Implement in Options
            Controls = Controls.Tilt;

            if (photonView.IsMine)
            {
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
                    var localPosition = carObj.transform.localPosition;
                    localPosition = new Vector3(localPosition.x,
                        localPosition.y - 0.4f, localPosition.z);
                    carObj.transform.localPosition = localPosition;
                    Car = carObj.GetComponent<FormulaCar>();

                    if (photonView.IsMine)
                    {
                        Car.ActivateCamera();
                        Car.Initialize(this);
                    }
                    break;
                default:
                    carObj = Instantiate(CarPrefabsHolder.Formula, transform, false);
                    var position = carObj.transform.localPosition;
                    position = new Vector3(position.x,
                        position.y - 0.4f, position.z);
                    carObj.transform.localPosition = position;
                    Car = carObj.GetComponent<FormulaCar>();
                    
                    if (photonView.IsMine)
                    {
                        Car.ActivateCamera();
                        Car.Initialize(this);
                    }
                    break;
            }

            // Instantiate hud
            var hudObj = Instantiate(hudPrefab);
            CanvasHandler = hudObj.GetComponent<CanvasHandler>();
            CanvasHandler.ChangeControls(Controls);
            
            // Initialize playerHandler
            PlayerInputHandler.Initialize(CanvasHandler.Joystick, CanvasHandler.ItemButton, CanvasHandler.GasButton);

            // add to position manager
            PositionManager.PositionManagerInstance.AllPlayers.Add(this);
        }

        #endregion

        #region Public Methods

        public void SetItem(ItemBehaviour newItem)
        {
            Item = newItem;
        }

        public void ChangeHud(Controls controls)
        {
            Controls = controls;
            CanvasHandler.ChangeControls(Controls);
        }

        #endregion
    }
}
