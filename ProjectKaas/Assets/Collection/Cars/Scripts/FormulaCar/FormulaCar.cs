using UnityEngine;

namespace Collection.Cars.Scripts.FormulaCar
{
    public class FormulaCar : Car
    {
        #region Private Serializable Fields

        [Tooltip("Speed of FormulaCar.")]
        [SerializeField] private float speed = 10f;

        #endregion

        #region MonoBehavior Callback

        private void Start()
        {
            Speed = speed;
        }

        #endregion
    }
}
