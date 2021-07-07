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

        [SerializeField] private byte lapCount;
        
        [field: SerializeField] public GameObject[] Zones { get; }

        public byte LapCount => lapCount;
        
        public List<PlayerHandler> AllPlayers => allPlayers;
        
        private void Awake()
        {
            if(PositionManagerInstance != null)
                Destroy(this);
            
            PositionManagerInstance = this;
        }

        private void Update()
        {
            DeterminePositions();
        }

        /// <summary>
        /// Updates the List of all player positions.
        /// </summary>
        private void DeterminePositions()
        {
            // No need when only 1 player.
            if(allPlayers.Count == 1)
                return;
            
            var players = SeparatedByLaps(allPlayersPositions);
            allPlayersPositions = SeparatedByZones(players);
        }
        
        /// <summary>
        /// Separates the players by their laps.
        /// </summary>
        /// <param name="players"> The player list you want to have sorted. </param>
        /// <returns> a list of players that is separated by their laps</returns>
        private PlayerHandler[,] SeparatedByLaps(List<PlayerHandler> players)
        {
            var separatedPlayers = new PlayerHandler[lapCount,players.Count];

            for (var i = 0; i < players.Count; i++)
            {
                for (var j = 0; j < players.Count; j++)
                {
                    // Sorts Players by their laps.
                    if (separatedPlayers[players[i].Car.LapCount, j] == null) 
                        continue;
                    
                    separatedPlayers[players[i].Car.LapCount, j] = players[i];
                    break;
                }
            }
            
            return separatedPlayers;
        }

        private List<PlayerHandler> SeparatedByZones(PlayerHandler[,] players)
        {
            /*
             * car8, car1, car2, car5 
             * car7,car3
             * car4,car6
             *
             * Go Through ONE lap count -> compare Zones -> sort by Zones
             */
            List<PlayerHandler> sortedPlayers = null;
            
            // Repeated for every lap backwards.
            for (int i = LapCount; i > 0; i--)
            {
                // Skips process when no player is at that lap. 
                if(players[i,0] == null)
                    continue;

                // Shortens process when only one player is at that lap.
                if (players[i, 1] == null)
                {
                    sortedPlayers.Add(players[i,0]);
                    continue;
                }
                
                
            }
            
            
            return sortedPlayers;
        }

        private List<PlayerHandler> FinalList(PlayerHandler[,] players)
        {
            return null;
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

            // Targets the last player when no one is in front of current player.
            if (nextPlayer == null)
            {
                nextPlayer = allPlayersPositions[allPlayersPositions.Count - 1];
            }
            
            return nextPlayer;
        }
    }
}
