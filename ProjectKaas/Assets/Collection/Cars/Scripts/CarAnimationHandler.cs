using UnityEngine;

namespace Collection.Cars.Scripts
{
    [RequireComponent(typeof(Animator))]
    public class CarAnimationHandler : MonoBehaviour
    {
        #region Private Serializabel Fields

        [SerializeField] private Transform rightFrontWheel;
        [SerializeField] private Transform leftFrontWheel;
        [SerializeField] private Transform rightBackWheel;
        [SerializeField] private Transform leftBackWheel;
        [SerializeField] private float maxWheelTurn = 25f;
        [SerializeField] private float maxWheelRotation = 5f;

        #endregion

        #region Private Fields

        private Car _car;
        private Animator _animator;
        private static readonly int Driving = Animator.StringToHash("Driving");

        #endregion

        #region MonoBehaviour CallBacks

        private void Awake()
        {
            _car = GetComponent<Car>();
            _animator = GetComponent<Animator>();
        }

        private void Update()
        {
            // Make sure only local player can control car.
            if (!_car.PlayerHandler.photonView.IsMine) return;

            AnimateCar(_car.CarControllerHandler.Speed != 0);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Handles car animations.
        /// </summary>
        /// <param name="driving">bool</param>
        private void AnimateCar(bool driving)
        {
            _animator.SetBool(Driving, driving);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Makes front wheels turn left and right.
        /// </summary>
        /// <param name="turn"></param>
        public void TurnWheels(float turn)
        {
            var leftFrontWheelRotation = leftFrontWheel.localRotation;
            var rightFrontWheelWheelRotation = rightFrontWheel.localRotation;
            
            leftFrontWheelRotation =
                Quaternion.Euler(leftFrontWheelRotation.x, turn * maxWheelTurn, leftFrontWheelRotation.z);
            leftFrontWheel.localRotation = leftFrontWheelRotation;

            rightFrontWheelWheelRotation = Quaternion.Euler(rightFrontWheelWheelRotation.x, turn * maxWheelTurn,
                rightFrontWheelWheelRotation.z);
            rightFrontWheel.localRotation = rightFrontWheelWheelRotation;
        }

        public void RotateWheels(float speed)
        {
            //TODO: wheel rotation.
        }

        #endregion
    }
}
