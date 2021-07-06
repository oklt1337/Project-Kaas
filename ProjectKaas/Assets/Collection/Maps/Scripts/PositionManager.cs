using System.Collections.Generic;
using Collection.NetworkPlayer.Scripts;
using UnityEngine;

namespace Collection.Maps.Scripts
{
    public class PositionManager : MonoBehaviour
    {
        public static PositionManager PositionManagerInstance;
        
        [SerializeField] private List<PlayerHandler> allPlayers;
        [SerializeField] private List<PlayerHandler> allPlayersPositions;

        [field: SerializeField] public GameObject[] Zones { get; }

        public List<PlayerHandler> AllPlayers => allPlayers;
        
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
            PlayerHandler nextPlayer = null;

            // Goes through every player and finds the current one.
            for (var i = 0; i < allPlayersPositions.Count; i++)
            {
                if (currentPlayer == allPlayersPositions[i] && i > 0)
                {
                    nextPlayer = allPlayersPositions[i - 1];
                }
            }
            
            return nextPlayer;
        }
    }
}
