using Collection.Items.Scripts.Field_Objects;
using Photon.Pun;
using UnityEngine;
using static Collection.GameManager.Scripts.GameManager;

namespace Collection.Items.Scripts
{
    public class HomingRocketLauncher : ItemBehaviour
    {
        public override void OnUse()
        { 
            var rocket = PhotonNetwork.Instantiate("Prefabs/Items/Homing Rocket",Owner.ForwardItem.transform.position, Quaternion.identity);
            rocket.transform.parent = Gm.transform;
            var homingRocket = rocket.GetComponent<HomingRocketBehaviour>();
            homingRocket.AcquireTarget(Owner);
            base.OnUse();
        }
    }
}
