using Photon.Pun;
using UnityEngine;

namespace Collection.Items.Scripts
{
    public class TripleRocketLauncher : ItemBehaviour
    {
        public override void OnUse()
        {
            var position = Owner.transform.position;
            PhotonNetwork.Instantiate("Rocket",position + Vector3.left, Quaternion.identity);
            PhotonNetwork.Instantiate("Rocket",position, Quaternion.identity);
            PhotonNetwork.Instantiate("Rocket",position + Vector3.right, Quaternion.identity);
            base.OnUse();
        }
    }
}
