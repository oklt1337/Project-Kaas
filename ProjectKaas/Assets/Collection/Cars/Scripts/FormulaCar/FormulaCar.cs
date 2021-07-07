using UnityEngine;

namespace Collection.Cars.Scripts.FormulaCar
{
    public class FormulaCar : Car
    {
        #region Private Serializable Fields

        [Tooltip("MaxSpeed of FormulaCar.")]
        [SerializeField] private float maxSpeed = 50f;
        [SerializeField] private float forwardAccel = 8f;
        [SerializeField] private float reverseAccel = 4f;
        [SerializeField] private float turnStrength = 180f;
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
    }
}
