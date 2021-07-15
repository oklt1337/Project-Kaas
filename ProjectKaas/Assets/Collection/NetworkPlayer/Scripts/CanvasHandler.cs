using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Collection.NetworkPlayer.Scripts
{
    public class CanvasHandler : MonoBehaviour
    {
        #region Private Serializable Fields

        [SerializeField] private Button itemButton;
        [SerializeField] private Button gasButton;
        [SerializeField] private Joystick joystick;

        #endregion

        #region Public Fields

        public Button ItemButton => itemButton;
        public Button GasButton => gasButton;
        public Joystick Joystick => joystick;

        #endregion

        #region Public Methods

        public void ChangeControls(Controls controls)
        {
            switch (controls)
            {
                case Controls.Joystick:
                    JoystickControl();
                    break;
                case Controls.Tilt:
                    TiltControl();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(controls), controls, null);
            }
        }

        #endregion

        #region Private Methods

        private void JoystickControl()
        {
            Joystick.gameObject.SetActive(true);
            GasButton.gameObject.SetActive(false);
        }
        
        private void TiltControl()
        {
            Joystick.gameObject.SetActive(false);
            gasButton.gameObject.SetActive(true);
        }

        #endregion
    }
}
