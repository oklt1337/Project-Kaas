using Collection.NetworkPlayer.Scripts;
using UnityEngine;

namespace Collection.Items.Scripts
{
    public class Nitro : ItemBehaviour
    {
        [SerializeField] private float speedUpTime;
        
        public override void OnUse()
        {
            var boostedPlayer = Owner.GetComponent<PlayerHandler>();
            //TODO: Get owner as car and give it speed up.
            base.OnUse();
        }
    }
}
