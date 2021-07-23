using Photon.Pun;
using UnityEngine;
using static Collection.GameManager.Scripts.GameManager;

namespace Collection.Items.Scripts
{
    public class TripleRocketLauncher : ItemBehaviour
    {
        public override void OnUse()
        {
            var position = Owner.ForwardItem.transform.position;
            var r1 = PhotonNetwork.Instantiate("Prefabs/Items/Rocket",position + Vector3.left, Quaternion.identity);
            var r2 = PhotonNetwork.Instantiate("Prefabs/Items/Rocket",position, Quaternion.identity);
            var r3 = PhotonNetwork.Instantiate("Prefabs/Items/Rocket",position + Vector3.right, Quaternion.identity);
            var transform1 = Gm.transform;
            r1.transform.parent = transform1;
            r2.transform.parent = transform1;
            r3.transform.parent = transform1;
            base.OnUse();
        }
    }
}
