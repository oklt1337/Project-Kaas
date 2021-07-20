using UnityEngine;

namespace Collection.Cars.Scripts.FormulaCar
{
    public class FormulaCar : Car
    {
        #region Private Serializable Fields

        [SerializeField] private GameObject myCamera;
        
        [Tooltip("MaxSpeed of FormulaCar.")]
        [SerializeField] private float maxSpeed = 200f;

        [SerializeField] private float forwardAccel = 100f;
        [SerializeField] private float reverseAccel = 50f;
        [SerializeField] private float turnStrength = 200f;
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
