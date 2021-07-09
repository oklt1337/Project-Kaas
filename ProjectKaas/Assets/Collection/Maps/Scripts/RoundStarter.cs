using System;
using Collection.NetworkPlayer.Scripts;
using Photon.Pun;
using TMPro;
using UnityEngine;

namespace Collection.Maps.Scripts
{
    public class RoundStarter : MonoBehaviourPunCallbacks
    {
        [SerializeField] private float startTime;
        [SerializeField] private TextMeshProUGUI text;

        private void Start()
        {
            text.gameObject.SetActive(true);
        }

        private void Update()
        {
            // Lets the start time tick down.
            startTime -= Time.deltaTime;
            text.text = "" + startTime;
            if (!(startTime < 0)) 
                return;
            
            for (var i = 0; i < PositionManager.PositionManagerInstance.AllPlayers.Count; i++)
            {
                PositionManager.PositionManagerInstance.AllPlayers[i].LocalRaceState = RaceState.Race;
            }
            text.gameObject.SetActive(false);
            gameObject.SetActive(false);
        }
    }
}
