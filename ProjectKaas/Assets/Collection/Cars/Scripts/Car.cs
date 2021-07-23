using System.Collections;
using Collection.NetworkPlayer.Scripts;
using Photon.Pun;
using UnityEngine;
using static Collection.Maps.Scripts.PositionManager;

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

        [Header("Audio Stuff")] 
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip onNewLapClip;

        #region Public Fields

        /// <summary>
        /// Essentials for the car to work.
        /// </summary>
        public CarControllerHandler CarControllerHandler { get; private set; }

        public CarAnimationHandler CarAnimationHandler { get; private set; }
        public PlayerHandler PlayerHandler { get; private set; }

        public CarStates MyCarStates { get; internal set; }

        // Depends on car type.
        public float MaxSpeed { get; internal set; }

        public float NitroSpeed { get; internal set; }

        public float SlowedMaxSpeed { get; internal set; }
        public float ForwardAccel { get; internal set; }
        
        public float NitroForwardAccel { get; internal set; }

        public float SlowedForwardAccel { get; internal set; }
        public float ReverseAccel { get; internal set; }
        public float TurnStrength { get; internal set; }
        public float GravityForce { get; internal set; }

        public GameObject[] VisibleObj { get; internal set; }

        // For the position manager.
        public byte place;
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

        public void ChangeSpeed(float duration)
        {
            StartCoroutine(ChangeSpeedCo(duration));
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

            // Playing the correct clip.
            audioSource.clip = onNewLapClip;
            audioSource.Play();
            
            // Triggers event when finished.
            if (PositionManagerInstance.LapCount < LapCount)
            {
                PositionManagerInstance.OnFinish.Invoke(PlayerHandler);
            }
        }

        /// <summary>
        /// Increases the lap count. 
        /// </summary>
        public void OnNextZone()
        {
            zoneCount++;

            if (ZoneCount == PositionManagerInstance.Zones.Length)
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
        }

        public void ChangeSpeedState(SpeedState state)
        {
            if (PlayerHandler.SpeedState != SpeedState.Nitro)
            {
                PlayerHandler.SpeedState = state;
            }
        }

        /// <summary>
        /// Plays an audio clip.
        /// </summary>
        /// <param name="newClip"> The audio clip you want to have played. </param>
        public void PlayAudioClip(AudioClip newClip)
        {
            audioSource.clip = newClip;
            audioSource.Play();
        }

        #endregion

        #region Private Methods

        private IEnumerator OnHitCo(float duration)
        {
            MyCarStates = CarStates.Hit;
            yield return new WaitForSeconds(duration);
            MyCarStates = CarStates.Drive;
        }

        private IEnumerator ChangeSpeedCo(float duration)
        {
            PlayerHandler.SpeedState = SpeedState.Nitro;

            yield return new WaitForSeconds(duration);

            PlayerHandler.SpeedState = SpeedState.None;
        }

        #endregion
    }
}
