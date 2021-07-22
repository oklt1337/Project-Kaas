using Collection.Items.Scripts.Field_Objects;
using Photon.Pun;
using UnityEngine;

namespace Collection.Items.Scripts
{
    public class HomingRocketLauncher : ItemBehaviour
    {
        public override void OnUse()
        { 
            var rocket = PhotonNetwork.Instantiate("Prefabs/Items/Homing Rocket",Owner.ForwardItem.transform.position, Quaternion.identity);
            print("spawned rocket");
            var homingRocket = rocket.GetComponent<HomingRocketBehaviour>();
            print("found rocket");
            homingRocket.AcquireTarget(Owner);
            print("spawned rocket");
            base.OnUse();
        }
    }
}
