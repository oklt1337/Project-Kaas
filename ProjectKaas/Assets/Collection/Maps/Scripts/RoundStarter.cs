using System;
using Collection.NetworkPlayer.Scripts;
using Photon.Pun;
using TMPro;
using UnityEngine;
using static Collection.Maps.Scripts.PositionManager;
using static Collection.UI.Scripts.Play.UIManager;

namespace Collection.Maps.Scripts
{
    public class RoundStarter : MonoBehaviourPunCallbacks
    {
        public static RoundStarter RoundStarterInstance;
        
        [SerializeField] private float startTime;
        [SerializeField] private bool mayStart;
        [SerializeField] private TextMeshProUGUI text;

        private void Awake()
        {
            if (RoundStarterInstance != null)
            {
                Destroy(this);
            }
            
            RoundStarterInstance = this;
        }

        private void Update()
        {
            if(!mayStart)
                return;
            
            // Lets the start time tick down.
            startTime -= Time.deltaTime;
            text.text = "" + (byte)(startTime + 1);
            
            if (!(startTime < 0)) 
                return;
            
            // Allows player to drive and deactivates the text and object.
            for (var i = 0; i < PositionManagerInstance.AllPlayers.Count; i++)
            {
                PositionManagerInstance.AllPlayers[i].LocalRaceState = RaceState.Race;
            }
            
            text.gameObject.SetActive(false);
            gameObject.SetActive(false);
            UIManagerInstance.gameObject.SetActive(true);
            UIManagerInstance.FindLocalPlayer(PositionManagerInstance.AllPlayers);
        }

        public void RoundStart()
        {
            print(RpcTarget.All);
            photonView.RPC("RoundStartRPC", RpcTarget.All);
        }
        
        [PunRPC]
        private void RoundStartRPC()
        {
            text.gameObject.SetActive(true);
            mayStart = true;
        }
    }
}
