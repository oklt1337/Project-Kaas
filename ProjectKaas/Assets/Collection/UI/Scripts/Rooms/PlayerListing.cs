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

        public Photon.Realtime.Player Player { get; private set; }
        public bool Ready { get; set; }

        public void SetPlayerInfo(Photon.Realtime.Player player)
        {
            Player = player;

            SetPlayerText(player);
        }

        public override void OnPlayerPropertiesUpdate(Photon.Realtime.Player target, ExitGames.Client.Photon.Hashtable changedProps)
        {
            if (target != null && target == Player)
            {
                if (changedProps.ContainsKey("RandomNumber"))
                {
                    SetPlayerText(target);
                }
            }
        }

        private void SetPlayerText(Photon.Realtime.Player player)
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
