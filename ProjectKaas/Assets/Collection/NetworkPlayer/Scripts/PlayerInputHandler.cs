using Photon.Pun;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Collection.NetworkPlayer.Scripts
{
    public class PlayerInputHandler : MonoBehaviourPun
    {
        private Joystick _joystick;
        private Button _item;
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
                MovementInput = new Vector2(Input.GetAxis("Vertical"), Input.GetAxis("Horizontal"));

                if (_gotInst)
                {
                    MovementInput = new Vector2(_joystick.Horizontal, _joystick.Vertical);
                }
            }
        }
    }
}
