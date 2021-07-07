using System.Collections;
using Collection.Maps.Scripts;
using Collection.NetworkPlayer.Scripts;
using Photon.Pun;
using UnityEngine;

namespace Collection.Cars.Scripts
{
    //[RequireComponent(typeof(CarControllerHandler), typeof(CarAnimationHandler))]
    public abstract class Car : MonoBehaviour
    {
        public enum CarStates
        {
            Drive,
            Idle,
            Hit
        }
        
        #region Public Fields
        
        /// <summary>
        /// Essentials for the car to work.
        /// </summary>
        public CarControllerHandler CarControllerHandler { get; private set; }
        public CarAnimationHandler CarAnimationHandler { get; private set; }
        public PlayerHandler PlayerHandler { get;  private set; }
        
        public CarStates MyCarStates  { get; private set; }

        // Depends on car type.
        public float MaxSpeed { get; internal set; }
        public float ForwardAccel { get; internal set; }
        public float ReverseAccel { get; internal set; }
        public float TurnStrength { get; internal set; }
        public float GravityForce { get; internal set; }
        
        // For the position manager.
        public byte LapCount { get; internal set; }
        public byte ZoneCount { get; internal set; }

        #endregion
        
        #region Internal Fields

        internal const float HitFloat = 1.5f;
        
        #endregion
        
        #region MonoBehaviour CallBacks

        private void Awake()
        {
            CarControllerHandler = GetComponent<CarControllerHandler>();
            CarAnimationHandler = GetComponent<CarAnimationHandler>();
            PlayerHandler = GetComponentInParent<PlayerHandler>();
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
            MyCarStates = CarStates.Hit;
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
            var oldSpeed = MaxSpeed;
            MaxSpeed += speed;
            yield return new WaitForSeconds(duration);
            MaxSpeed = oldSpeed;
        }

        #endregion
    }
}
