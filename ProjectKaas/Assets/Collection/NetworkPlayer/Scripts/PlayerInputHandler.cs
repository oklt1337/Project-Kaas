using Photon.Pun;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Collection.NetworkPlayer.Scripts
{
    public class PlayerInputHandler : MonoBehaviourPun
    {
        private Joystick _joystick;
        private Button _item;
        private Button _gas;
        private PlayerHandler _playerHandler;

        private bool _gasPressed;

        public Vector2 MovementInput { get; private set; }

        private bool _gotInst;

        private void Awake()
        {
            _playerHandler = GetComponent<PlayerHandler>();
        }

        public void Initialize(Joystick joystick, Button itemButton, Button gas)
        {
            if (photonView.IsMine)
            {
                _joystick = joystick;

                _item = itemButton;
                _item.onClick.AddListener(OnClickItem);

                _gas = gas;

                var eventTrigger = gas.gameObject.AddComponent<EventTrigger>();
                var entry = new EventTrigger.Entry
                {
                    eventID = EventTriggerType.PointerDown
                };

                var entry2 = new EventTrigger.Entry
                {
                    eventID = EventTriggerType.PointerUp
                };
                
                
                entry.callback.AddListener(data => { OnPointerDownDelegate((PointerEventData)data); });
                entry2.callback.AddListener(data => { OnPointerUpDelegate((PointerEventData)data); });
                
                eventTrigger.triggers.Add(entry);
                eventTrigger.triggers.Add(entry2);
                
                _gotInst = true;
            }
        }


        private void OnClickItem()
        {
            _playerHandler.UseItem();
        }

        private void OnPointerDownDelegate(PointerEventData data)
        {
            OnGasDown();
        }
        
        private void OnPointerUpDelegate(PointerEventData data)
        {
            OnGasUp();
        }

        private void OnGasDown()
        {
            Debug.Log("Gas");
            _gasPressed = true;
        }

        private void OnGasUp()
        {
            Debug.Log("No Gas");
            _gasPressed = false;
        }

        // Update is called once per frame
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.L))
            {
                PhotonNetwork.LoadLevel(1);
            }
            
            if (_playerHandler.LocalRaceState == RaceState.Race)
            {
                if (_playerHandler.developerMode)
                {
                    MovementInput = new Vector2(Input.GetAxis("Vertical"), Input.GetAxis("Horizontal"));
                }
                else
                {
                    if (_gotInst)
                    {
                        if (_playerHandler.Controls == Controls.Joystick)
                        {
                            MovementInput = new Vector2(_joystick.Vertical, _joystick.Horizontal);
                        }
                        else if (_playerHandler.Controls == Controls.Tilt)
                        {
                            var x = _gasPressed ? 1 : 0;
                            MovementInput = new Vector2(x, Input.acceleration.y);
                        }
                    }
                }
            }
        }
    }
}
