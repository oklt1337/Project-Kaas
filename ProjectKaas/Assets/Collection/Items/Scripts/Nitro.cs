using UnityEngine;

namespace Collection.Items.Scripts
{
    public class Nitro : ItemBehaviour
    {
        [SerializeField] private float speedUpTime;
        
        public override void OnUse()
        {
            //TODO: Get owner as car and give it speed up.
            base.OnUse();
        }
    }
}
