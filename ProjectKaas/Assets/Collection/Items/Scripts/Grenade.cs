using Photon.Pun;
using UnityEngine;

namespace Collection.Items.Scripts
{
    public class Grenade : ItemBehaviour
    {
        public override void OnUse()
        {
            // Spawns a grenade and launches it forward.
            var grenade = PhotonNetwork.Instantiate("Grenade",Owner.transform.position, Quaternion.identity);
            var grenadeRb = grenade.gameObject.GetComponent<Rigidbody>();
            grenadeRb.AddForce(0,10,10);
            base.OnUse();
        }
    }
}
