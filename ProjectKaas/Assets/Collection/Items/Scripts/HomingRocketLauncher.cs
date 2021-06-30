using Photon.Pun;
using UnityEngine;

namespace Collection.Items.Scripts
{
    public class HomingRocketLauncher : ItemBehaviour
    {
        public override void OnUse()
        { 
            PhotonNetwork.Instantiate("Homing Rocket",Owner.transform.position+Vector3.forward, Quaternion.identity);
            base.OnUse();
        }
    }
}
