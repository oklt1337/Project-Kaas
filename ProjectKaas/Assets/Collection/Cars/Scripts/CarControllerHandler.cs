using Collection.NetworkPlayer.Scripts;
using Photon.Pun;
using UnityEngine;

namespace Collection.Cars.Scripts
{
    public class CarControllerHandler : MonoBehaviourPun
    {
        [SerializeField] private float speed = 30f;

        private PlayerInputHandler _playerInputHandler;
        private Animator _animator;
        private Rigidbody _rigidbody;
        private static readonly int Driving = Animator.StringToHash("Driving");

        private void Awake()
        {
            _playerInputHandler = GetComponent<PlayerInputHandler>();
            _animator = GetComponent<Animator>();
            _rigidbody = GetComponent<Rigidbody>();
        }

        // Update is called once per frame
        private void Update()
        {
            if (photonView.IsMine)
            {
                AnimateCar(_playerInputHandler.Drive);
                MoveCar();
            }
        }

        /// <summary>
        /// handles car movement.
        /// </summary>
        private void MoveCar()
        {
            var movement = _playerInputHandler.MovementInput;
            movement *= speed;
            var myTransform = transform;
            var movePos = myTransform.right * movement.x + myTransform.forward * movement.y;
            var direction = new Vector3(movePos.x, _rigidbody.velocity.y, movePos.z);

            _rigidbody.velocity = direction;
        }

        /// <summary>
        /// Handles car animations.
        /// </summary>
        /// <param name="driving"></param>
        private void AnimateCar(bool driving)
        {
            _animator.SetBool(Driving, driving);
        }
    }
}
