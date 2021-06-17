using Collection.Network.Scripts.Utility;
using UnityEngine;

namespace Collection.Network.Scripts
{
    public class PlayerNetworkManager : MonoBehaviour
    {
        public static PlayerNetworkManager Instance;
        
        /// <summary>
        /// Player Name
        /// </summary>
        public string PlayerName { get; private set; }
        public int PlayerID { get; private set; }
        
        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
            }

            Initialize();
        }

        /// <summary>
        /// Initialize first connection to server.
        /// </summary>
        private void Initialize()
        {
            PlayerName = DefaultValues.GetDefaultPlayerName();
            PlayerID = DefaultValues.GetRandom4DigitNumber();
            PlayerName += "#" + PlayerID;
        }
    }
}
