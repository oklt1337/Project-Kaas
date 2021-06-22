using UnityEngine;

namespace Collection.Cars.Scripts
{
    [RequireComponent(typeof(Animator))]
    public class CarAnimationHandler : MonoBehaviour
    {
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
            
            AnimateCar(_car.PlayerHandler.PlayerInputHandler.Drive);
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
    }
}
