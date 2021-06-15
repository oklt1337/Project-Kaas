using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace Collection.NetworkPlayer.Scripts
{
    public class RaiseEvent : MonoBehaviourPun
    {
        [SerializeField] private Material material;

        private const byte ColorChangeEvent = 0;

        private void Update()
        {
            if (photonView.IsMine && Input.GetKeyDown(KeyCode.Space))
                ChangeColor();
        }

        private void OnEnable()
        {
            PhotonNetwork.NetworkingClient.EventReceived += NetworkingClientOnEventReceived;
        }
        
        private void OnDisable()
        {
            PhotonNetwork.NetworkingClient.EventReceived -= NetworkingClientOnEventReceived;
        }

        private void NetworkingClientOnEventReceived(EventData obj)
        {
            if (obj.Code == ColorChangeEvent)
            {
                var data = (object[]) obj.CustomData;

                var r = (float) data[0];
                var g = (float) data[0];
                var b = (float) data[0];
                
                material.color = new Color(r, g, b);
            }
        }

        private void ChangeColor()
        {
            var r = Random.Range(0f, 1f);
            var g = Random.Range(0f, 1f);
            var b = Random.Range(0f, 1f);

            material.color = new Color(r, g, b);

            var data = new object[] {r, g, b,};

            PhotonNetwork.RaiseEvent(ColorChangeEvent, data, RaiseEventOptions.Default, SendOptions.SendUnreliable);
        }
    }
}
