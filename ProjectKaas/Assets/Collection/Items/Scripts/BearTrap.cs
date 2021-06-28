using Photon.Pun;
using UnityEngine;

namespace Collection.Items.Scripts
{
    public class BearTrap : ItemBehaviour
    {
        public override void OnUse()
        {
            PhotonNetwork.Instantiate("Bear Trap",Owner.transform.position+Vector3.back, Quaternion.identity);
            base.OnUse();
        }
    }
}
