using System.Collections;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;
using Collection.ReadyUpManager.Scripts;

namespace Collection.UI.Scripts.Play.Room
{
    public class PlayerListing : MonoBehaviourPun
    {
        #region Private Serializable Fields

        [SerializeField] private TextMeshProUGUI playerName;
        [SerializeField] private TextMeshProUGUI playerPing;
        [SerializeField] private RawImage playerPingImage;
        [SerializeField] private RawImage playerBackgroundImage;

        #endregion

        #region Private Fields

        private bool _ready = false;

        #endregion

        #region Public Field

        public Player PhotonPlayer { get; set; }
        public TextMeshProUGUI PlayerName => playerName;
        public TextMeshProUGUI PlayerPing => playerPing;
        public RawImage PlayerPingImage => playerPingImage;

        public bool Ready
        {
            get => _ready;
            set
            {
                _ready = value;
                SetReadyStatus();

                if (_ready)
                {
                    ReadyUpManager.Scripts.ReadyUpManager.Instance.AddReadyPlayer(this);
                }
                else
                {
                    ReadyUpManager.Scripts.ReadyUpManager.Instance.RemoveReadyPlayer(this);
                }
            } 
        }

        #endregion

        #region MonoBehaviour Callbacks

        private void Start()
        {
            var button = GetComponent<Button>();
            button.onClick.AddListener(OnClickPlayerInfo);
        }

        #endregion

        #region Public Methods

        public void ApplyPhotonPlayer(Player photonPlayer)
        {
            PhotonPlayer = photonPlayer;
            PlayerName.text = photonPlayer.NickName;
            StartCoroutine(ShowPing());
        }

        public void OnClickPlayerInfo()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                if (PhotonPlayer.IsLocal)
                {
                    Debug.Log("You cant interact with yourself.");
                    return;
                }
                Debug.Log("Player info of: " + PhotonPlayer.NickName);
                OverlayCanvases.Instance.PlayerInfoCanvas.gameObject.SetActive(true);
                OverlayCanvases.Instance.PlayerInfoCanvas.GetComponent<PlayerInfoCanvas>().Initialize(PhotonPlayer);
            }
        }

        #endregion

        #region Private Methods

        private IEnumerator ShowPing()
        {
            while (PhotonNetwork.IsConnected)
            {
                var ping = (int) PhotonPlayer.CustomProperties["Ping"];

                SetPingColor(ping);
                PlayerPing.text = ping.ToString();

                yield return new WaitForSeconds(1f);
            }
        }

        private void SetReadyStatus()
        {
            playerBackgroundImage.color = Ready ? Color.green : new Color(0,173, 181, 150);
        }

        private void SetPingColor(int ping)
        {
            if (ping < 50)
            {
                PlayerPingImage.color = Color.green;
            }
            else if (ping < 100)
            {
                PlayerPingImage.color = Color.yellow;
            }
            else
            {
                PlayerPingImage.color = Color.red;
            }
        }

        #endregion
    }
}
