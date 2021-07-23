using Photon.Pun;
using UnityEngine;
using static Collection.GameManager.Scripts.GameManager;

namespace Collection.Items.Scripts
{
    public class Grenade : ItemBehaviour
    {
        public override void OnUse()
        {
            // Spawns a grenade and launches it forward.
            var grenade = PhotonNetwork.Instantiate("Prefabs/Items/Grenade",Owner.ForwardItem.transform.position, Quaternion.identity);
            grenade.transform.parent = Gm.transform;
            var grenadeRb = grenade.gameObject.GetComponent<Rigidbody>();
            grenadeRb.AddForce(0,10,10);
            base.OnUse();
        }
    }
}
