using System;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace Collection.Network.Scripts
{
    public class MasterClientMonitor : MonoBehaviourPunCallbacks
    {
        #region Types.
        /// <summary>
        /// Holds a players ping.
        /// </summary>
        private class PlayerPing
        {
            public PlayerPing(Player player, int ping)
            {
                Player = player;
                _pings.Add(ping);
            }
            /// <summary>
            /// Last time a ping was set for this player.
            /// </summary>
            public float LastUpdatedTime { get; private set; } = -1f;
            /// <summary>
            /// Player this instance is for.
            /// </summary>
            public readonly Player Player;
            /// <summary>
            /// Pings for this player.
            /// </summary>
            private List<int> _pings = new List<int>();
            /// <summary>
            /// Maximum number of ping history to allow.
            /// </summary>
            private const int MaximumRecordedPings = 6;
 
            /// <summary>
            /// Returns the average ping for this player. Returns -1f if recorded ping quantity is not at maximum recorded pings.
            /// </summary>
            /// <returns></returns>
            public int ReturnAveragePing()
            {
                //Not enough pings to consider an average yet.
                if (_pings.Count < MaximumRecordedPings)
                {
                    return -1;
                }
                //Has enough pings to return average.
                else
                {
                    var sum = _pings.Sum();
                    // ReSharper disable once PossibleLossOfFraction
                    return Mathf.CeilToInt(sum / _pings.Count);
                }
            }
 
            /// <summary>
            /// Adds a ping for this player. Replaces oldest ping if at maximum recorded pings.
            /// </summary>
            /// <param name="value"></param>
            public void AddPing(int value)
            {
                //If at max pings remove the first entry.
                if (_pings.Count >= MaximumRecordedPings)
                    _pings.RemoveAt(0);
 
                _pings.Add(value);
                LastUpdatedTime = Time.unscaledTime;
            }
        }
        #endregion
 
        #region Private.
        /// <summary>
        /// List of player pings.
        /// </summary>
        private List<PlayerPing> _playerPings = new List<PlayerPing>();
        /// <summary>
        /// Next time to check for a high ping on master client.
        /// </summary>
        private float _nextCheckChangeMaster;
        /// <summary>
        /// Number of times in a row the current master client has had a significantly higher ping.
        /// </summary>
        private int _consequtiveHighPingCount;
        /// <summary>
        /// True if received a master client change request. This is only set on master client.
        /// </summary>
        private bool _pendingMasterChange;
        /// <summary>
        /// Time a takeover request was sent. -1f if no takeover request is active.
        /// </summary>
        private float _takeoverRequestTime = -1f;
        /// <summary>
        /// Next time to send ping for this client.
        /// </summary>
        private float _nextSendPingTime;
        #endregion
 
        #region Const.
        /// <summary>
        /// How many times in a row the current masters ping must be significantly higher than others to forfeit master client.
        /// </summary>
        private const int HighPingTurnoverRequirement = 3;
        /// <summary>
        /// Minimum current master client pings must be higher than lowest pinging player to be considered significantly higher.
        /// </summary>
        private const int MinimumPingDifference = 50;
        /// <summary>
        /// How often to check for lowest pings. Masterclient must have a high ping after HIGH_PING_TURNOVER_REQUIREMENT times.
        /// </summary>
        private const float PingCheckInterval = 5f;
        /// <summary>
        /// Time master client has to grant a takeover request before it's taken forcefully. It's preferred that the master grants the request to prevent multiple takeovers in a short duration, but when master client is laggy or broken this may not be possible.
        /// </summary>
        private const float TakeoverRequestTimeout = 3f;
        /// <summary>
        /// How frequently to send this clients ping.
        /// </summary>
        private const float SendPingInterval = 5f;
        #endregion
 
        private void Update()
        {
            CheckSendPing();
            CheckChangeMaster();
            CheckTakeoverTimeout();
        }
 
        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            //See if player is in player pings. If so remove them.
            var index = _playerPings.FindIndex(x => x.Player == otherPlayer);
            if (index != -1)
                _playerPings.RemoveAt(index);
        }
 
        public override void OnMasterClientSwitched(Player newMasterClient)
        {
            base.OnMasterClientSwitched(newMasterClient);
            //Unset pending master change.
            _pendingMasterChange = false;
            //Reset takeover data.
            _takeoverRequestTime = -1f;
            _consequtiveHighPingCount = 0;
        }
 
        /// <summary>
        /// Checks if a takeover needs to be forced. Usually occurs when the master client doesn't respond in time.
        /// </summary>
        private void CheckTakeoverTimeout()
        {
            //No takeover in progress.
            if (Math.Abs(_takeoverRequestTime - (-1f)) < .1f)
                return;
 
            //Master client request timed out. Takeover forcefully.
            if ((Time.unscaledTime - _takeoverRequestTime) > TakeoverRequestTimeout)
            {
                _takeoverRequestTime = -1f;
                SetNewMaster(PhotonNetwork.LocalPlayer);
            }
        }

        /// <summary>
        /// Sets a new master client.
        /// </summary>
        /// <param name="newMaster"></param>
        /// <param name="resetHighPingCount"></param>
        private void SetNewMaster(Player newMaster, bool resetHighPingCount = true)
        {
            if (resetHighPingCount)
                _consequtiveHighPingCount = 0;
            //Set new master and use takeover.
            PhotonNetwork.SetMasterClient(newMaster);
        }
 
 
        /// <summary>
        /// Checks to change master if the current master is significantly more laggy than other players.
        /// </summary>
        private void CheckChangeMaster()
        {
            //Network conditions not met.
            if (!PhotonNetwork.IsConnected || !PhotonNetwork.InRoom || PhotonNetwork.IsMasterClient)
                return;
            //Takeover already in progress.
            if (Math.Abs(_takeoverRequestTime - -1f) > .1f)
                return;
            //Too recent since last check.
            if (Time.time < _nextCheckChangeMaster)
                return;
 
            //Next time to check pings.
            _nextCheckChangeMaster = Time.time + PingCheckInterval;
 
            /* Players should already be removed when leaving the room.
             * This is just an extra precautionary. */
            RemoveNullPlayers();
 
            //Get list of all players.
            var players = PhotonNetwork.PlayerList;
            //If only one player.
            if (players.Length <= 1)
                return;
 
            var lowestAverageIndex = -1;
            var lowestAveragePing = -1;
            var masterPing = -1;
            var masterIndex = -1;
 
            foreach (var player in players)
            {
                var pingsIndex = _playerPings.FindIndex(x => x.Player == player);
                //Not found in players pings.
                if (pingsIndex == -1)
                    continue;
 
                //If player being checked is this client.
                if (player == PhotonNetwork.LocalPlayer)
                {
                    //If this client hasn't sent a ping in awhile don't even try to takeover.
                    if ((Time.unscaledTime - _playerPings[pingsIndex].LastUpdatedTime) >= (SendPingInterval * 2))
                        return;
                }
 
                //Get average.
                var averagePing = _playerPings[pingsIndex].ReturnAveragePing();
                //If average isn't -1 then enough pings have been sent to calculate an average.
                if (averagePing != -1)
                {
                    /* If average ping is lowest than the current lowest or
                     * the lowest average index is -1 (which means unset) then
                     * set as new lowest average. */
                    if (averagePing < lowestAveragePing || lowestAverageIndex == -1)
                    {
                        lowestAveragePing = averagePing;
                        lowestAverageIndex = pingsIndex;
                    }
                    //If player being checked is master client.
                    if (_playerPings[pingsIndex].Player.IsMasterClient)
                    {
                        masterIndex = pingsIndex;
 
                        /* If master client hasn't send a ping within a reasonable time then
                         * set the master clients ping unrealistically high to force a high
                         * consequtive ping count. */
                        if ((Time.unscaledTime - _playerPings[masterIndex].LastUpdatedTime) >= SendPingInterval * 2)
                            masterPing = 999999999;
                        //Otherwise set to average ping.
                        else
                            masterPing = averagePing;
                    }
                }
            }
 
            //If the lowest ping index couldn't be found.
            if (lowestAverageIndex == -1)
                return;
            //Master index couldn't be found.
            if (masterIndex == -1)
                return;
            /* If the lowest ping index isn't this client then
             * don't proceed further, let the lowest pinging player
             * try the take over when the time comes. */
            if (_playerPings[lowestAverageIndex].Player != PhotonNetwork.LocalPlayer)
                return;
 
            /* If here this client is the lowest pinging player. */
 
            float masterPingDifference = masterPing - lowestAveragePing;
            //If master ping is difference is high enough to change master client.
            if (masterPingDifference > MinimumPingDifference)
                _consequtiveHighPingCount++;
            //master ping not too much higher.
            else
                _consequtiveHighPingCount = 0;
 
            //If high ping 3 times in a row then request setting a new master.
            if (_consequtiveHighPingCount >= 3)
            {
                _takeoverRequestTime = Time.unscaledTime;
                photonView.RPC("RPC_RequestMasterClient", RpcTarget.MasterClient, _playerPings[lowestAverageIndex].Player);
            }
        }
 
        /// <summary>
        /// Sends ping to all players including self.
        /// </summary>
        private void CheckSendPing()
        {
            if (Time.unscaledTime < _nextSendPingTime)
                return;
 
            _nextSendPingTime = Time.unscaledTime + SendPingInterval;
 
            photonView.RPC("RPC_ReceivePing", RpcTarget.All, PhotonNetwork.GetPing());
        }
 
        /// <summary>
        /// Receives a ping for the specified player.
        /// </summary>
        /// <param name="player"></param>
        /// <param name="ping"></param>
        [PunRPC]
        private void RPC_ReceivePing(Player player, int ping)
        {
            var index = _playerPings.FindIndex(x => x.Player == player);
            if (index == -1)
                _playerPings.Add(new PlayerPing(player, ping));
            else
                _playerPings[index].AddPing(ping);
        }
 
        /// <summary>
        /// Removes null players from player pings list.
        /// </summary>
        private void RemoveNullPlayers()
        {
            for (var i = 0; i < _playerPings.Count; i++)
            {
                //If null player remove from list and decrease i.
                if (_playerPings[i].Player == null)
                {
                    _playerPings.RemoveAt(i);
                    i--;
                }
            }
        }
 
        /// <summary>
        /// Request releasing the master client to the requestor.
        /// </summary>
        /// <param name="requestor">Player requesting the change.</param>
        /// Master client required.
        [PunRPC]
        private void RPC_RequestMasterClient(Player requestor)
        {
            //A change is already pending.
            if (_pendingMasterChange)
                return;
            //If not master.
            if (!PhotonNetwork.IsMasterClient)
                return;
 
            _pendingMasterChange = true;
            //RPC to allow change.
            photonView.RPC("RPC_MasterClientGranted", requestor);
        }
 
        /// <summary>
        /// Received on a player which may takeover as master client.
        /// </summary>
        [PunRPC]
        private void RPC_MasterClientGranted()
        {
            //Set new master as self.
            SetNewMaster(PhotonNetwork.LocalPlayer);
        }
 
        /// <summary>
        /// Called when the game gains or loses focus.
        /// </summary>
        /// <param name="pause"></param>
        private void OnApplicationPause(bool pause)
        {
            if (pause)
                LocallyHandOffMasterClient();
        }
        /// <summary>
        /// Called when the game gains or loses focus.
        /// </summary>
        private void OnApplicationFocus(bool focus)
        {
            if (!focus)
                LocallyHandOffMasterClient();
        }
 
        /// <summary>
        /// Hands off master client to the lowest pinging player at this time. 
        /// </summary>
        private void LocallyHandOffMasterClient()
        {
            //Conditions where this does not apply.
            if (!PhotonNetwork.IsConnected || !PhotonNetwork.InRoom || !PhotonNetwork.IsMasterClient)
                return;
            //Only player.
            if (PhotonNetwork.PlayerList.Length <= 1)
                return;
 
            //Other index which isn't this client.
            var otherIndex = -1;
 
            var lowestIndex = -1;
            var lowestPing = -1;
            for (var i = 0; i < _playerPings.Count; i++)
            {
                //Skip self.
                if (_playerPings[i].Player == PhotonNetwork.LocalPlayer)
                    continue;
 
                otherIndex = i;
 
                var average = _playerPings[i].ReturnAveragePing();
                //If new lowest or lowest isnt yet set.
                if (average < lowestPing || lowestIndex == -1)
                {
                    lowestIndex = i;
                    lowestPing = average;
                }
            }
 
            //If the lowest ping was found.
            if (lowestIndex != -1)
            {
                SetNewMaster(_playerPings[lowestIndex].Player);
            }
            //Lowest ping not found. Maybe not enough data is collected.
            else
            {
                /* Can only proceed if an index of another player was found.
                 * If so send to the last player checked which wasnt self. */
                if (otherIndex != -1)
                    SetNewMaster(_playerPings[otherIndex].Player);
            }
        }
    }
}

