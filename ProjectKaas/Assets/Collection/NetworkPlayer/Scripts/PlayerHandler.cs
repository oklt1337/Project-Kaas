using System;
using Collection.Cars.Scripts;
using Collection.Cars.Scripts.BaywatchCar;
using Collection.Cars.Scripts.FormulaCar;
using Collection.Cars.Scripts.PassengerCar;
using Collection.Cars.Scripts.VeteranCar;
using Collection.Items.Scripts;
using Collection.Maps.Scripts;
using Collection.UI.Scripts.Play.ChoosingCar;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using static Collection.Maps.Scripts.PositionManager;
using static Collection.Maps.Scripts.RoundStarter;
using static Collection.UI.Scripts.Play.UIManager;

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

    public enum SpeedState
    {
        None,
        Nitro,
        Slowed
    }

    [RequireComponent(typeof(PlayerInputHandler))]
    public class PlayerHandler : MonoBehaviourPun
    {
        #region Private Serializeable Fields

        [SerializeField] private GameObject hudPrefab;
        [SerializeField] private GameObject infoUI;
        [SerializeField] private GameObject audioListener;
        [SerializeField] private GameObject forwardItem;
        [SerializeField] private GameObject backItem;
        [SerializeField] private ItemBehaviour item;

        #endregion

        #region Public Fields

        public bool developerMode;

        public PlayerInputHandler PlayerInputHandler { get; private set; }

        public Car Car { get; private set; }

        public CanvasHandler CanvasHandler { get; private set; }

        public ItemBehaviour Item => item;

        public GameObject ForwardItem => forwardItem;
        public GameObject BackItem => backItem;

        public RaceState LocalRaceState { get; set; }

        public Controls Controls { get; private set; }

        public Player LocalPlayer { get; set; }

        public int ActorNumber { get; set; }
        
        public SpeedState SpeedState { get; set; }

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
            GameManager.Scripts.GameManager.Gm.PlayerHandlers.Add(this);

            PositionManagerInstance.OnFinish += WhenFinnish;
        }

        private void OnDestroy()
        {
            Destroy(Car.gameObject);
            PositionManagerInstance.AllPlayers.Remove(this);
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
            SpeedState = SpeedState.None;
            
            var hashTable = PhotonNetwork.LocalPlayer.CustomProperties;
            ChooseCar chooseCar;

            if (hashTable.ContainsKey("Car"))
            {
                chooseCar = (ChooseCar) hashTable["Car"];
            }
            else
            {
                chooseCar = ChooseCar.Formula;
            }

            // Initialize car
            GameObject carObj;
            switch (chooseCar)
            {
                case ChooseCar.Formula:
                    carObj = Instantiate(CarPrefabsHolder.Formula, transform, false);

                    // get Component
                    Car = carObj.GetComponent<FormulaCar>();
                    break;
                case ChooseCar.Baywatch:
                    carObj = Instantiate(CarPrefabsHolder.Baywatch, transform, false);

                    // get Component
                    Car = carObj.GetComponent<Baywatch>();
                    break;
                case ChooseCar.Passenger:
                    carObj = Instantiate(CarPrefabsHolder.Passenger, transform, false);

                    // get Component
                    Car = carObj.GetComponent<PassengerCar>();
                    break;
                case ChooseCar.Veteran:
                    carObj = Instantiate(CarPrefabsHolder.Veteran, transform, false);

                    // get Component
                    Car = carObj.GetComponent<Veteran>();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            // Fix Position

            var localPosition = carObj.transform.localPosition;
            localPosition = new Vector3(localPosition.x,
                localPosition.y - 0.4f, localPosition.z);
            carObj.transform.localPosition = localPosition;


            // make sure only one cam in scene and instantiate hud.
            if (photonView.IsMine)
            {
                Car.ActivateCamera();
                Car.Initialize(this);

                // Instantiate hud
                var hudObj = Instantiate(hudPrefab);
                hudObj.SetActive(true);
                CanvasHandler = hudObj.GetComponent<CanvasHandler>();
                CanvasHandler.ChangeControls(Controls);

                var uiObj = Instantiate(infoUI);
                uiObj.SetActive(true);

                // Initialize playerHandler
                PlayerInputHandler.Initialize(CanvasHandler.Joystick, CanvasHandler.ItemButton,
                    CanvasHandler.GasButton);
            }
            else
            {
                Car.DeactivateComponents();
            }

            // add to position manager
            PositionManagerInstance.AllPlayers.Add(this);

            // Checks if the AllPlayers list is full and if so starts the round.
            if (PhotonNetwork.CurrentRoom.Players.Count == PositionManagerInstance.AllPlayers.Count)
            {
                RoundStarterInstance.RoundStart();
            }

            var view = GetComponent<PhotonView>();
            ActorNumber = view.ControllerActorNr;
        }

        public void ReInit(ChooseCar newCar)
        {
            Destroy(Car.gameObject);

            // Initialize car
            GameObject carObj;
            switch (newCar)
            {
                case ChooseCar.Formula:
                    carObj = Instantiate(CarPrefabsHolder.Formula, transform, false);

                    // get Component
                    Car = carObj.GetComponent<FormulaCar>();
                    break;
                case ChooseCar.Baywatch:
                    carObj = Instantiate(CarPrefabsHolder.Baywatch, transform, false);

                    // get Component
                    Car = carObj.GetComponent<Baywatch>();
                    break;
                case ChooseCar.Passenger:
                    carObj = Instantiate(CarPrefabsHolder.Passenger, transform, false);

                    // get Component
                    Car = carObj.GetComponent<PassengerCar>();
                    break;
                case ChooseCar.Veteran:
                    carObj = Instantiate(CarPrefabsHolder.Veteran, transform, false);

                    // get Component
                    Car = carObj.GetComponent<Veteran>();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            // Fix Position
            var localPosition = carObj.transform.localPosition;
            localPosition = new Vector3(localPosition.x,
                localPosition.y - 0.4f, localPosition.z);
            carObj.transform.localPosition = localPosition;


            Car.DeactivateComponents();
        }

        /// <summary>
        /// Turns everything off after Finishing.
        /// </summary>
        /// <param name="player"> The player that finished. </param>
        private void WhenFinnish(PlayerHandler player)
        {
            if (player != this)
                return;

            LocalRaceState = RaceState.PreStart;
            hudPrefab.gameObject.SetActive(false);
            infoUI.gameObject.SetActive(false);

            Car.SetObjInvisible();
            Car.MyCarStates = Car.CarStates.Hit;
            PlayerInputHandler.MovementInput = Vector2.zero;
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

        public void SetPosition(Vector3 pos)
        {
            transform.position = pos;
        }

        #endregion
    }
}
