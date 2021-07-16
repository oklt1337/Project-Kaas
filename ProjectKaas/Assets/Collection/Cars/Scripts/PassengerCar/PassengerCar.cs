using UnityEngine;

namespace Collection.Cars.Scripts.PassengerCar
{
    public class PassengerCar : Car
    {
        #region Private Serializable Fields

        [SerializeField] private GameObject myCamera;

        [Tooltip("MaxSpeed of FormulaCar.")] 
        [SerializeField] private float maxSpeed = 120;

        [SerializeField] private float forwardAccel = 80f;
        [SerializeField] private float reverseAccel = 25f;
        [SerializeField] private float turnStrength = 150f;
        [SerializeField] private float gravityForce = 10f;

        #endregion

        #region MonoBehavior Callback

        private void Start()
        {
            MaxSpeed = maxSpeed;
            ForwardAccel = forwardAccel;
            ReverseAccel = reverseAccel;
            TurnStrength = turnStrength;
            GravityForce = gravityForce;
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
