using Photon.Pun;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Collection.NetworkPlayer.Scripts
{
    public class PlayerInputHandler : MonoBehaviourPun
    {
        private Joystick _joystick;
        private Button _gas;

        public bool Drive { get; private set; }
        
        public Vector2 MovementInput { get; private set; }

        private bool _gotInst;

        public void Initialize(GameObject obj)
        {
            if (photonView.IsMine)
            {
                _joystick = obj.GetComponentInChildren<Joystick>();
                _gas = obj.GetComponentInChildren<Button>();

                var trigger = _gas.GetComponent<EventTrigger>();

                var pointerDown = new EventTrigger.Entry {eventID = EventTriggerType.PointerDown};
                pointerDown.callback.AddListener(OnButtonGas);
                trigger.triggers.Add(pointerDown);

                var pointerUp = new EventTrigger.Entry {eventID = EventTriggerType.PointerUp};
                pointerUp.callback.AddListener(OnButtonGasRelease);
                trigger.triggers.Add(pointerUp);

                _gotInst = true;
            }
        }
        private void OnButtonGasRelease(BaseEventData arg0)
        {
            Debug.Log("up");
            Drive = false;
        }

        private void OnButtonGas(BaseEventData arg0)
        {
            Debug.Log("down");
            Drive = true;
        }

        // Update is called once per frame
        private void Update()
        {
            MovementInput = new Vector2(Input.GetAxis("Vertical"), Input.GetAxis("Horizontal"));

            if (_gotInst)
            {
                MovementInput = new Vector2(_joystick.Horizontal, _joystick.Vertical);
            }
        }
    }
}
