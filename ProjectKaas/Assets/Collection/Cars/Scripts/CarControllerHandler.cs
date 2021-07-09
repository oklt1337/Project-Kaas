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
        }

        // Update is called once per frame
        private void Update()
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

            // Make sure only local player can control car.
            //if (!_car.PlayerHandler.photonView.IsMine) return;

            //MoveCar();
        }

        private void FixedUpdate()
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

        #region Private Methods

        /// <summary>
        /// handles car movement.
        /// </summary>
        private void MoveCar()
        {
            var movement = _car.PlayerHandler.PlayerInputHandler.MovementInput;
            movement *= _car.MaxSpeed;
            var myTransform = transform;
            var movePos = myTransform.right * movement.x + myTransform.forward * movement.y;
            var direction = new Vector3(movePos.x, rigidbody.velocity.y, movePos.z);

            rigidbody.velocity = direction;
        }

        #endregion
    }
}
