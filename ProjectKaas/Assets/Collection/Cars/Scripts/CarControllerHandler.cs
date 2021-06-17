using Collection.NetworkPlayer.Scripts;
using Photon.Pun;
using UnityEngine;

namespace Collection.Cars.Scripts
{
    [RequireComponent(typeof(Rigidbody))]
    public class CarControllerHandler : MonoBehaviour
    {
        #region Private Fields
        
        private Car _car;
        private Rigidbody _rigidbody;
        
        #endregion

        #region MonoBehaviour CallBacks
        
        private void Awake()
        {
            _car = GetComponent<Car>();
            _rigidbody = GetComponent<Rigidbody>();
        }

        // Update is called once per frame
        private void Update()
        {
            // Make sure only local player can control car.
            if (!_car.photonView.IsMine) return;
            
            MoveCar();
        }
        
        #endregion

        #region Private Methods
        
        /// <summary>
        /// handles car movement.
        /// </summary>
        private void MoveCar()
        {
            var movement = _car.PlayerHandler.PlayerInputHandler.MovementInput;
            movement *= _car.Speed;
            var myTransform = transform;
            var movePos = myTransform.right * movement.x + myTransform.forward * movement.y;
            var direction = new Vector3(movePos.x, _rigidbody.velocity.y, movePos.z);

            _rigidbody.velocity = direction;
        }
        
        #endregion
    }
}
