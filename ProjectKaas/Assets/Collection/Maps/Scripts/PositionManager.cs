using System.Collections;
using System.Collections.Generic;
using Collection.NetworkPlayer.Scripts;
using Photon.Pun;
using TMPro;
using UnityEngine;

namespace Collection.Maps.Scripts
{
    public class PositionManager : MonoBehaviourPunCallbacks
    {
        public static PositionManager PositionManagerInstance;
        
        [Header ("Players")]
        [SerializeField] private List<PlayerHandler> allPlayers;
        [SerializeField] private List<PlayerHandler> allPlayersPositions;
        [SerializeField] private List<PlayerHandler> playersStandings;
        
        
        [SerializeField] private List<PlayerHandler> currentStandings;
        
        [Header("Map")]
        [SerializeField] private byte lapCount;
        [SerializeField] private GameObject[] zones;

        [Header("Finish related stuff")] 
        [SerializeField] private GameObject victoryScreen;
        [SerializeField] private TextMeshProUGUI victoryScreenText;
        [SerializeField] private bool raceFinished;
        [SerializeField] private float victoryScreenTime;
        
        public delegate void Finish(PlayerHandler player);
        public Finish OnFinish;
        
        public GameObject[] Zones => zones;

        public byte LapCount => lapCount;
        
        public List<PlayerHandler> AllPlayers => allPlayers;

        public bool RaceFinished => raceFinished;
        
        private void Awake()
        {
            if(PositionManagerInstance != null)
                Destroy(this);
            
            PositionManagerInstance = this;
            OnFinish += AssignToStandings;
            OnFinish += OnRaceFinish;
        }

        private void Update()
        {
            DeterminePositions();
            
            currentStandings = allPlayersPositions;
        }

        /// <summary>
        /// Updates the List of all player positions. (Probably way to complicated but I hope it doesn't lag too much)
        /// </summary>
        private void DeterminePositions()
        {
            // No need when only 1 player.
            if(allPlayers.Count == 1)
                return;
            
            // Resetting the List. 
            allPlayersPositions = playersStandings;
            var players = SeparatedByLaps(allPlayers);

            if(players != null)
                allPlayersPositions = SeparatedByZones(players);
        }
        
        /// <summary>
        /// Separates the players by their laps.
        /// </summary>
        /// <param name="players"> The player list you want to have sorted. </param>
        /// <returns> A list of players that is separated by their laps. </returns>
        private PlayerHandler[,] SeparatedByLaps(List<PlayerHandler> players)
        {
            if (players == null)
                return null;
            
            var separatedPlayers = new PlayerHandler[lapCount+1, players.Count];
            byte count = 0;
            
            for (var i = 1; i < LapCount + 1; i++)
            {
                // Stops when every player is in the list.
                if(count == players.Count)
                    break;
                
                for (var j = 0; j < players.Count; j++)
                {
                    // Sorts Players by their laps.
                    if (players[j].Car.LapCount != i) 
                        continue;
                    
                    // Stops when every player is in the list.
                    if(count == players.Count)
                        break;
                    
                    count++;
                    separatedPlayers[i, j] = players[j];
                }
            }
            
            return separatedPlayers;
        }

        private List<PlayerHandler> SeparatedByZones(PlayerHandler[,] players)
        {
            var sortedPlayers = new List<PlayerHandler>();
            
            // Repeated for every lap backwards.
            for (var i = LapCount+1; i <= 0; i--)
            {
                // Skips process when no player is at that lap. 
                if(players[i,0] == null)
                    continue;

                // Shortens process when only one player is at that lap.
                if (players[i, 1] == null)
                {
                    sortedPlayers.Add(players[i, 0]);
                    continue;
                }
                
                print("penis");
                Sorter(ref players, i);
                print("Post Sort Players Length "+players.Length);
                for (var j = 0; j < allPlayers.Count; j++)
                {
                    // Adds to list if existing and stops for when null.
                    if (players[i, j] != null)
                    {
                        print(players[i,j]);
                        sortedPlayers.Add(players[i,j]);
                        
                        // Sets their position.
                        players[i, j].Position = (byte)(sortedPlayers.Count + 1);
                        players[i, j].Car.place = (byte) sortedPlayers.Count;
                    }
                    else
                    {
                        break;
                    }
                }
            }
            
            return sortedPlayers;
        }

        /// <summary>
        /// Sorts a array at its index.
        /// </summary>
        /// <param name="array"> The array you want to have sorted. </param>
        /// <param name="index"> The index of its first value. </param>
        private void Sorter(ref PlayerHandler[,] array, int index)
        {
            var sortedPlayers = new PlayerHandler[allPlayers.Count];
            for (var i = Zones.Length; i <= 0; i--)
            {
                byte playersInSameZone = 0;
                var passedAllChecks = true;
                for (var j = 0; j < array.Length; j++)
                {
                    // Ends for-loop when there is no car anymore.
                    if(array[index,j] == null)
                        break;

                    // Skips car if it isn't in the Zone.
                    if (array[index, j].Car.ZoneCount != i) 
                        continue;

                    playersInSameZone++;
                    // Searches for a free space in the new array.
                    for (var k = 0; k < sortedPlayers.Length; k++)
                    {
                        // Checks if spot is free.
                        if (sortedPlayers[k] != null)
                        {
                            // Checks if both cars are in the same zone.
                            if (sortedPlayers[k].Car.ZoneCount != i)
                                continue;

                            passedAllChecks = false;
                            var currentCarDistance = (Zones[i+1].transform.position - array[index,j].transform.position).magnitude;
                            
                            // Compares Distance to next Zone with the other cars.
                            for (var l = 0; l < playersInSameZone; l++)
                            {
                                var carDistance = (Zones[i + 1].transform.position - sortedPlayers[k+l].transform.position)
                                    .magnitude;
                                if (currentCarDistance < carDistance)
                                {
                                    // Swaps both cars when the current car is closer then the older car.
                                    var tempPlayer = sortedPlayers[k+l];
                                    sortedPlayers[k + l] = array[index, j];
                                    array[index, j] = tempPlayer;
                                }
                                else if (l == playersInSameZone-1)
                                {
                                    sortedPlayers[k + 1] = array[index, j];
                                }
                            }
                        }
                        
                        if(!passedAllChecks)
                            break;

                        array[index, j].Car.place = k; 
                        sortedPlayers[k] = array[index, j];
                        break;
                    }
                }
            }
            
            array.SetValue(sortedPlayers,index);
        }

        /// <summary>
        /// Finds the next Player.
        /// </summary>
        /// <returns> The next player lmao. </returns>
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

        /// <summary>
        /// Assigns the player to the standings and removes him from the all players list.
        /// </summary>
        /// <param name="player"> The player that wants to assign themselves. </param>
        private void AssignToStandings(PlayerHandler player)
        {
            playersStandings.Add(player);
            allPlayers.Remove(player);
        }

        /// <summary>
        /// What happens when the race finishes.
        /// </summary>
        /// <param name="player"> The player to check what position he got in. </param>
        private void OnRaceFinish(PlayerHandler player)
        {
            if(allPlayers.Count > 0)
                return;
            
            if (false)
            {
                TextFixer();
            }
            
            victoryScreen.SetActive(true);
            raceFinished = true;

            if (PhotonNetwork.IsMasterClient)
            {
                StartCoroutine(EndMapCo());
            }
        }

        /// <summary>
        /// Fixes the text of the victory screen.
        /// </summary>
        private void TextFixer()
        {
            victoryScreenText.text = null;
            for (var i = 0; i < playersStandings.Count; i++)
            {
                victoryScreenText.text += i + ".       " + playersStandings[i].LocalPlayer.NickName + "\n\n";
            }
        }

        private IEnumerator EndMapCo()
        {
            photonView.RPC("SetProps", RpcTarget.All);
            
            yield return new WaitForSeconds(victoryScreenTime);
            
            photonView.RPC("RPCLeaveMatch", RpcTarget.All);
        }

        [PunRPC]
        private void SetProps()
        {
            GameManager.Scripts.GameManager.Gm.OnMatchFinished();
        }
        
        [PunRPC]
        private void RPCLeaveMatch()
        {
            PhotonNetwork.LeaveRoom();
        }
    }
}
