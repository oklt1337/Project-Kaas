using System.Collections;
using Collection.NetworkPlayer.Scripts;
using Photon.Pun;
using UnityEngine;

namespace Collection.Cars.Scripts
{
    [RequireComponent(typeof(CarControllerHandler), typeof(CarAnimationHandler))]
    public abstract class Car : MonoBehaviour
    {
        #region Public Fields
        
        /// <summary>
        /// Essentials for the car to work.
        /// </summary>
        public CarControllerHandler CarControllerHandler { get; private set; }
        public CarAnimationHandler CarAnimationHandler { get; private set; }
        public PlayerHandler PlayerHandler { get;  private set; }
        
        // Depends on car type.
        public float Speed { get; internal set; }

        #endregion
        
        #region MonoBehaviour CallBacks

        private void Awake()
        {
            CarControllerHandler = GetComponent<CarControllerHandler>();
            CarAnimationHandler = GetComponent<CarAnimationHandler>();
        }

        #endregion
        
        #region Public Methods

        public void Initialize(PlayerHandler playerHandler)
        {
            PlayerHandler = playerHandler;
        }

        /// <summary>
        /// Changing speed for a set time, and then change it back to normal.
        /// </summary>
        /// <param name="speed">float</param>
        /// <param name="duration">float</param>
        public void ChangeSpeed(float speed, float duration)
        {
            StartCoroutine(ChangeSpeedCo(speed, duration));
        }
        
        /// <summary>
        /// 
        /// </summary>
        public void OnHit()
        {
            
        }
        
        #endregion

        #region Private Methods

        private IEnumerator ChangeSpeedCo(float speed, float duration)
        {
            var oldSpeed = Speed;
            Speed += speed;
            yield return new WaitForSeconds(duration);
            Speed = oldSpeed;
        }

        #endregion
    }
}
