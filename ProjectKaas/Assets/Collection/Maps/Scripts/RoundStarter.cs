using Collection.NetworkPlayer.Scripts;
using Photon.Pun;
using TMPro;
using UnityEngine;
using static Collection.Maps.Scripts.PositionManager;

namespace Collection.Maps.Scripts
{
    public class RoundStarter : MonoBehaviourPunCallbacks
    {
        public static RoundStarter RoundStarterInstance;
        
        [SerializeField] private float startTime;
        [SerializeField] private byte startTimeSoundIndicator;
        [SerializeField] private bool mayStart;
        [SerializeField] private TextMeshProUGUI text;
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip audioClip;

        private void Awake()
        {
            if (RoundStarterInstance != null)
            {
                Destroy(this);
            }
            
            RoundStarterInstance = this;
        }

        private void Start()
        {
            startTimeSoundIndicator = (byte)startTime;
            audioSource.clip = audioClip;
        }

        private void Update()
        {
            if(!mayStart)
                return;
            
            // Lets the start time tick down.
            startTime -= Time.deltaTime;

            // Playing the sound after every second.
            if (startTimeSoundIndicator != (byte) startTime)
            {
                audioSource.Play();
                startTimeSoundIndicator--;
            }
            
            text.text = "" + (byte)(startTime + 1);
            
            if (!(startTime < 0)) 
                return;
            
            // Allows player to drive and deactivates the text and object.
            foreach (var t in PositionManagerInstance.AllPlayers)
            {
                t.LocalRaceState = RaceState.Race;
            }
            
            text.gameObject.SetActive(false);
            gameObject.SetActive(false);
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
