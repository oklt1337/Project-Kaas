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

            VisibleObj = new GameObject[5];
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
