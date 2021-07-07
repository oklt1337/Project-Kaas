using System.Collections;
using Collection.Maps.Scripts;
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
        
        // For the position manager.
        public byte LapCount { get; internal set; }
        public byte ZoneCount { get; internal set; }

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
        /// How the car reacts to a hit.
        /// </summary>
        public void OnHit()
        {
            
        }

        /// <summary>
        /// Increases the lap count. 
        /// </summary>
        private void OnNextLap()
        {
            LapCount++;
            ZoneCount = 0;
        }
        
        /// <summary>
        /// Increases the lap count. 
        /// </summary>
        public void OnNextZone()
        {
            ZoneCount++;

            if (ZoneCount > PositionManager.PositionManagerInstance.Zones.Length)
            {
                OnNextLap();
            }
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
