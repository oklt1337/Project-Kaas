using Collection.NetworkPlayer.Scripts;
using Photon.Pun;
using UnityEngine;

namespace Collection.Items.Scripts.Field_Objects
{
    public class BearTrapBehaviour : MonoBehaviour
    {
        private void OnCollisionEnter(Collision other)
        {
            PhotonNetwork.Destroy(gameObject);

            if (other.gameObject.CompareTag("Player"))
            {
                var hitPlayer = other.gameObject.GetComponent<PlayerHandler>();
                // TODO: Stop Car from driving for x seconds.
            }
        }
    }
}
