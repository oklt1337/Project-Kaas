using Photon.Pun;
using UnityEngine;
using static Collection.GameManager.Scripts.GameManager;

namespace Collection.Items.Scripts
{
    public class BearTrap : ItemBehaviour
    {
        public override void OnUse()
        {
            var item = PhotonNetwork.Instantiate("Prefabs/Items/Bear Trap",Owner.BackItem.transform.position, Quaternion.identity);
            item.transform.parent = Gm.transform;
            base.OnUse();
        }
    }
}
