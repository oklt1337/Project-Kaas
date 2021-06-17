using Collection.Cars.Scripts;
using UnityEngine;

namespace Collection.NetworkPlayer.Scripts
{
    [RequireComponent(typeof(PlayerInputHandler), typeof(CameraHandler))]
    public class PlayerHandler : MonoBehaviour
    {
        #region Public Fields

        public PlayerInputHandler PlayerInputHandler { get; private set; }
        public CameraHandler CameraHandler { get; private set; }
        
        public Car Car { get; private set; }
        
        #endregion

        #region MonoBehaviour Callbacks

        private void Awake()
        {
            PlayerInputHandler = GetComponent<PlayerInputHandler>();
            CameraHandler = GetComponent<CameraHandler>();
        }

        #endregion

    }
}
