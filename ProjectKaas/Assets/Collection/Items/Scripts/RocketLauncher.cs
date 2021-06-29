using Photon.Pun;
using UnityEngine;

namespace Collection.Items.Scripts
{
    public class RocketLauncher : ItemBehaviour
    {
        public override void OnUse()
        { 
            PhotonNetwork.Instantiate("Rocket",Owner.transform.position, Quaternion.identity);
            base.OnUse();
        }
    }
}
