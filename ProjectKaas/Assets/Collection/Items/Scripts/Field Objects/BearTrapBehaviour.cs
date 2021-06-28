using Photon.Pun;
using UnityEngine;

namespace Collection.Items.Scripts.Field_Objects
{
    public class BearTrapBehaviour : MonoBehaviour
    {
        private void OnCollisionEnter(Collision other)
        {
            PhotonNetwork.Destroy(gameObject);

            if (other.gameObject.CompareTag("Car"))
            {
                // TODO: Stop Car from driving for x seconds.
            }
        }
    }
}
