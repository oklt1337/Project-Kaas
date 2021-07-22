using UnityEngine;

namespace Collection.Cars.Scripts.PassengerCar
{
    public class PassengerCar : Car
    {
        #region Private Serializable Fields

        [SerializeField] private GameObject myCamera;

        [Tooltip("MaxSpeed of Passenger.")] 
        [SerializeField] private float maxSpeed = 120;

        [SerializeField] private float forwardAccel = 80f;
        [SerializeField] private float reverseAccel = 25f;
        [SerializeField] private float turnStrength = 150f;
        [SerializeField] private float gravityForce = 10f;
        
        [SerializeField] private GameObject[] visObj;

        #endregion

        #region MonoBehavior Callback

        private void Start()
        {
            MaxSpeed = maxSpeed;
            SlowedMaxSpeed = maxSpeed * 0.5f;
            NitroSpeed = maxSpeed + 50f;
            ForwardAccel = forwardAccel;
            SlowedForwardAccel = forwardAccel * 0.5f;
            NitroForwardAccel = forwardAccel + 50f;
            ReverseAccel = reverseAccel;
            TurnStrength = turnStrength;
            GravityForce = gravityForce;

            VisibleObj = visObj;
        }

        #endregion

        #region Public Methods

        public override void ActivateCamera()
        {
            myCamera.SetActive(true);
        }

        #endregion
    }
}
