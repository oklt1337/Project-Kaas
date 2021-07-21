using Photon.Pun;
using UnityEngine;

namespace Collection.Items.Scripts
{
    public class BearTrap : ItemBehaviour
    {
        public override void OnUse()
        {
            PhotonNetwork.Instantiate("Prefabs/Items/Bear Trap",Owner.BackItem.transform.position, Quaternion.identity);
            base.OnUse();
        }
    }
}
