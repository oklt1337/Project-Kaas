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
        
        [SerializeField] private GameObject body;
        [SerializeField] private GameObject wheelFl;
        [SerializeField] private GameObject wheelFr;
        [SerializeField] private GameObject wheelBl;
        [SerializeField] private GameObject wheelBr;

        #endregion

        #region MonoBehavior Callback

        private void Start()
        {
            MaxSpeed = maxSpeed;
            ForwardAccel = forwardAccel;
            ReverseAccel = reverseAccel;
            TurnStrength = turnStrength;
            GravityForce = gravityForce;
            
            VisibleObj[0] = body;
            VisibleObj[1] = wheelFl;
            VisibleObj[2] = wheelFr;
            VisibleObj[3] = wheelBl;
            VisibleObj[4] = wheelBr;
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
