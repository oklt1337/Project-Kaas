using UnityEngine;

namespace Collection.Cars.Scripts.VeteranCar
{
    public class Veteran : Car
    {
        #region Private Serializable Fields

        [SerializeField] private GameObject myCamera;

        [Tooltip("MaxSpeed of VeteranCar.")] 
        [SerializeField] private float maxSpeed = 75;

        [SerializeField] private float forwardAccel = 45f;
        [SerializeField] private float reverseAccel = 25f;
        [SerializeField] private float turnStrength = 150f;
        [SerializeField] private float gravityForce = 10f;
        
        [SerializeField] private GameObject[] visObj;

        #endregion

        #region MonoBehavior Callback

        private void Start()
        {
            MaxSpeed = maxSpeed;
            ForwardAccel = forwardAccel;
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
