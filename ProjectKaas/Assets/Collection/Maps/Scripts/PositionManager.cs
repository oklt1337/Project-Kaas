using System.Collections.Generic;
using Collection.NetworkPlayer.Scripts;
using UnityEngine;

namespace Collection.Maps.Scripts
{
    public class PositionManager : MonoBehaviour
    {
        public static PositionManager PositionManagerInstance;
        
        [SerializeField] private List<PlayerHandler> allPlayers;


        private void Awake()
        {
            if(PositionManagerInstance != null)
                Destroy(this);
            
            PositionManagerInstance = this;
        }

        /// <summary>
        /// Finds the next Player.
        /// </summary>
        /// <returns></returns>
        public PlayerHandler FindNextPlayer(PlayerHandler currentPlayer)
        {
            return null;
        }
    }
}
