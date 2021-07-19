using Photon.Pun;
using UnityEngine;

namespace Collection.Items.Scripts
{
    public class RocketLauncher : ItemBehaviour
    {
        public override void OnUse()
        { 
            PhotonNetwork.Instantiate("Prefabs/Items/Rocket",Owner.ForwardItem.transform.position, Quaternion.identity);
            base.OnUse();
        }
    }
}
