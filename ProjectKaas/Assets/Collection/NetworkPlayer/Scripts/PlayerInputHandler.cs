using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

namespace Collection.NetworkPlayer.Scripts
{
    public class PlayerInputHandler : MonoBehaviourPun
    {
        private Joystick _joystick;
        private Button _item;
        private Button _gas;
        private PlayerHandler _playerHandler;

        public Vector2 MovementInput { get; private set; }

        private bool _gotInst;

        private void Awake()
        {
            _playerHandler = GetComponent<PlayerHandler>();
        }

        public void Initialize(GameObject obj)
        {
            if (photonView.IsMine)
            {
                _joystick = obj.GetComponentInChildren<Joystick>();
                
                _item = obj.GetComponentInChildren<Button>();
                _item.onClick.AddListener(OnClickItem);
                
                _gotInst = true;
            }
        }
        private void OnClickItem()
        {
            _playerHandler.UseItem();
        }

        // Update is called once per frame
        private void Update()
        {
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
                            MovementInput = new Vector2(Input.acceleration.x, Input.acceleration.y);
                        }
                    }
                }
            }
        }
    }
}
