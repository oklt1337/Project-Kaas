using UnityEngine;

namespace Collection.Cars.Scripts
{
    public class CarControllerHandler : MonoBehaviour
    {
        #region Private Serializabel Fields

        [SerializeField] private new Rigidbody rigidbody;
        [SerializeField] private LayerMask ground;
        [SerializeField] private Transform groundRayPoint;
        [SerializeField] private float groundRayLenght = 0.5f;

        #endregion

        #region Private Fields

        private Car _car;
        private float _speed;
        private float _turn;
        private bool _grounded;
        private float _defaultDrag;
        private float _hitFloat;

        #endregion

        #region internal Fields

        internal float Speed => _speed;

        #endregion

        #region MonoBehaviour CallBacks

        private void Awake()
        {
            _car = GetComponent<Car>();
        }

        private void Start()
        {
            rigidbody.transform.parent = null;
            _defaultDrag = rigidbody.drag;
            _car.MyCarStates = Car.CarStates.Drive;
            _hitFloat = Car.HitFloat;
        }

        // Update is called once per frame
        private void Update()
        {
            // Make sure only local player can control car.
            if (!_car.PlayerHandler.photonView.IsMine) return;
            
            if (_car.MyCarStates == Car.CarStates.Hit)
            {
                _hitFloat -= Time.deltaTime;
                if (_hitFloat < 0)
                {
                    _car.MyCarStates = Car.CarStates.Drive;
                    _hitFloat = Car.HitFloat;
                }
            }
            
            GetSpeed();
        }

        private void FixedUpdate()
        {
            // Make sure only local player can control car.
            if (!_car.PlayerHandler.photonView.IsMine) return;

            if (_car.MyCarStates == Car.CarStates.Drive)
            {
                MoveCar();
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// handles car movement.
        /// </summary>
        private void GetSpeed()
        {
            var myTransform = _car.PlayerHandler.gameObject.transform;
            var xInput = _car.PlayerHandler.PlayerInputHandler.MovementInput.x;
            var yInput = _car.PlayerHandler.PlayerInputHandler.MovementInput.y;
            
            _speed = 0f;
            _turn = yInput;
            
            if (xInput > 0)
            {
                _speed = xInput * _car.ForwardAccel * 1000f;
            }
            else
            {
                _speed = xInput * _car.ReverseAccel * 1000f;
            }

            if (_grounded)
            {
                myTransform.rotation = Quaternion.Euler(myTransform.rotation.eulerAngles +
                                                        new Vector3(0f,
                                                            _turn * _car.TurnStrength * Time.deltaTime * xInput, 0f));
            }

            _car.CarAnimationHandler.TurnWheels(_turn);
            _car.CarAnimationHandler.RotateWheels(_speed);

            myTransform.position = rigidbody.transform.position;
        }

        private void MoveCar()
        {
            var myTransform = _car.PlayerHandler.gameObject.transform;
            _grounded = false;

            if (Physics.Raycast(groundRayPoint.position, -myTransform.up, out var hit, groundRayLenght, ground))
            {
                _grounded = true;

                myTransform.rotation = Quaternion.FromToRotation(myTransform.up, hit.normal) * myTransform.rotation;
            }

            if (_grounded)
            {
                rigidbody.drag = _defaultDrag;
                
                if (Mathf.Abs(_speed) > 0)
                {
                    rigidbody.AddForce(transform.forward * _speed);
                }
            }
            else
            {
                rigidbody.drag = 0.1f;
                rigidbody.AddForce(Vector3.up * (-_car.GravityForce * 100f));
            }
        }

        #endregion
    }
}
