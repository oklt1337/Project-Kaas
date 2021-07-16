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
    }
}
