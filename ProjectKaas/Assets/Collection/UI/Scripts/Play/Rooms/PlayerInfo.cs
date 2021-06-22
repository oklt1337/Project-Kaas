using Collection.Network.Scripts;
using Photon.Pun;
using TMPro;
using UnityEngine;

namespace Collection.UI.Scripts.Rooms
{
    public class PlayerInfo : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI nickname;

        private void OnEnable()
        {
            nickname.text = MasterManager.GameSettings.NickName;
        }
    }
}
