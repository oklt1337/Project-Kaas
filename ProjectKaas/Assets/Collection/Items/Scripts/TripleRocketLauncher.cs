using Photon.Pun;
using UnityEngine;

namespace Collection.Items.Scripts
{
    public class TripleRocketLauncher : ItemBehaviour
    {
        public override void OnUse()
        {
            var position = Owner.ForwardItem.transform.position;
            PhotonNetwork.Instantiate("Prefabs/Items/Rocket",position + Vector3.left, Quaternion.identity);
            PhotonNetwork.Instantiate("Prefabs/Items/Rocket",position, Quaternion.identity);
            PhotonNetwork.Instantiate("Prefabs/Items/Rocket",position + Vector3.right, Quaternion.identity);
            base.OnUse();
        }
    }
}
