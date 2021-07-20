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
            Hit
        }

        [SerializeField] private byte lapCount;
        [SerializeField] private byte zoneCount;
        
        #region Public Fields
        
        /// <summary>
        /// Essentials for the car to work.
        /// </summary>
        public CarControllerHandler CarControllerHandler { get; private set; }
        public CarAnimationHandler CarAnimationHandler { get; private set; }
        public PlayerHandler PlayerHandler { get;  private set; }
        
        public CarStates MyCarStates  { get; internal set; }

        // Depends on car type.
        public float MaxSpeed { get; internal set; }
        public float ForwardAccel { get; internal set; }
        public float ReverseAccel { get; internal set; }
        public float TurnStrength { get; internal set; }
        public float GravityForce { get; internal set; }
        
        public GameObject[] VisibleObj { get; internal set; }

        // For the position manager.
        public int place;
        public byte LapCount => lapCount;
        public byte ZoneCount => zoneCount;

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

        public void DeactivateComponents()
        {
            Destroy(CarControllerHandler.MoveSphere);
        }
        
        public void Initialize(PlayerHandler playerHandler)
        {
            PlayerHandler = playerHandler;
        }
        
        public virtual void ActivateCamera()
        {
            
        }

        public void ChangeSpeed(float speedUpValue,float duration)
        {
            StartCoroutine(ChangeSpeedCo(speedUpValue, duration));
        }

        /// <summary>
        /// How the car reacts to a hit.
        /// </summary>
        public void OnHit(float duration)
        {
            StartCoroutine(OnHitCo(duration));
        }

        /// <summary>
        /// Increases the lap count. 
        /// </summary>
        private void OnNextLap()
        {
            lapCount++;
            zoneCount = 0;

            // Triggers event when finished.
            if (PositionManager.PositionManagerInstance.LapCount < LapCount)
            {
                PositionManager.PositionManagerInstance.OnFinish.Invoke(PlayerHandler);
            }
        }
        
        /// <summary>
        /// Increases the lap count. 
        /// </summary>
        public void OnNextZone()
        {
            zoneCount++;

            if (ZoneCount == PositionManager.PositionManagerInstance.Zones.Length)
            {
                OnNextLap();
            }
        }

        public void SetObjInvisible()
        {
            foreach (var t in VisibleObj)
            {
                t.SetActive(false);
            }

            var col = CarControllerHandler.MoveSphere.GetComponent<SphereCollider>();
            col.isTrigger = true;
        }
        
        #endregion

        #region Private Methods

        private IEnumerator OnHitCo(float duration)
        {
            MyCarStates = CarStates.Hit;
            yield return new WaitForSeconds(duration);
            MyCarStates = CarStates.Drive;
        }

        private IEnumerator ChangeSpeedCo(float speedUpValue,float duration)
        {
            var oldMaxSpeed = MaxSpeed;
            var oldForwardAccel = ForwardAccel;

            MaxSpeed += speedUpValue;
            ForwardAccel += speedUpValue;
            
            yield return new WaitForSeconds(duration);
            
            MaxSpeed = oldMaxSpeed;
            ForwardAccel = oldForwardAccel;
        }

        #endregion
    }
}
