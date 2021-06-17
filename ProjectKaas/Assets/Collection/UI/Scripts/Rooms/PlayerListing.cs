using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Collection.UI.Scripts.Rooms
{
    public class PlayerListing : MonoBehaviourPunCallbacks
    {
        [SerializeField] TextMeshProUGUI playerName;
        [SerializeField] TextMeshProUGUI ping;
        [SerializeField] RawImage pingIcon;

        public Player Player { get; private set; }
        public bool Ready { get; set; }

        public void SetPlayerInfo(Player player)
        {
            Player = player;

            SetPlayerText(player);

            var myPing = PhotonNetwork.GetPing();
            ping.text = myPing.ToString();

            if (myPing <= 50)
            {
                pingIcon.color = Color.green;
            }
            else if (myPing <= 80)
            {
                pingIcon.color = Color.yellow;
            }
            else
            {
                pingIcon.color = Color.red;
            }
        }

        public override void OnPlayerPropertiesUpdate(Player target, ExitGames.Client.Photon.Hashtable changedProps)
        {
            if (target != null && target == Player)
            {
                if (changedProps.ContainsKey("RandomNumber"))
                {
                    SetPlayerText(target);
                }
            }
        }

        private void SetPlayerText(Player player)
        {
            var result = -1;
            if (Player.CustomProperties.ContainsKey("RandomNumber"))
            {
                result = (int) Player.CustomProperties["RandomNumber"];
            }

            playerName.text = result + ", " + player.NickName;
        }
    }
}
