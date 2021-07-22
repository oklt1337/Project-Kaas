using Collection.Items.Scripts.Field_Objects;
using Photon.Pun;
using UnityEngine;

namespace Collection.Items.Scripts
{
    public class RocketLauncher : ItemBehaviour
    {
        public override void OnUse()
        {
            var position = Owner.ForwardItem.transform.position;
            var rocket = PhotonNetwork.Instantiate("Prefabs/Items/Rocket",position, Quaternion.identity);
            var actualRocket = rocket.GetComponent<RocketBehaviour>();
            actualRocket.SetFlyVector(position-Owner.transform.position);
            base.OnUse();
        }
    }
}
