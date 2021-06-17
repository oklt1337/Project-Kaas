using Collection.NetworkPlayer.Scripts;
using Photon.Pun;
using UnityEngine;

namespace Collection.Cars.Scripts
{
    [RequireComponent(typeof(CarControllerHandler), typeof(CarAnimationHandler))]
    public abstract class Car : MonoBehaviourPun
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
        
        #endregion
    }
}
