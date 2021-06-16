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
        void Update()
        {
            if (photonView.IsMine)
            {
                _animator.SetBool(Driving, _playerInputHandler.Drive);
                
                if (_playerInputHandler.Drive)
                {
                    var newPos = transform.position + Vector3.forward * (speed * Time.deltaTime);
                    _rigidbody.MovePosition(newPos);
                }
            }
        }
    }
}
