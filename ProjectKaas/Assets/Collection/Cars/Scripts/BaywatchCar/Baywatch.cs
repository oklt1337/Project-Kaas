using UnityEngine;

namespace Collection.Cars.Scripts.BaywatchCar
{
    public class Baywatch : Car
    {
        #region Private Serializable Fields

        [SerializeField] private GameObject myCamera;

        [Tooltip("MaxSpeed of BaywatchCar.")] 
        [SerializeField] private float maxSpeed = 90;

        [SerializeField] private float forwardAccel = 60f;
        [SerializeField] private float reverseAccel = 35f;
        [SerializeField] private float turnStrength = 100f;
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
