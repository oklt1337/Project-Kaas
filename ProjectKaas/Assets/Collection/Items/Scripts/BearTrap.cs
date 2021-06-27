using UnityEngine;

namespace Collection.Items.Scripts
{
    public class BearTrap : ItemBehaviour
    {
        public override void OnUse()
        {
            Instantiate(onFieldItem,Owner.transform.position+Vector3.back,Quaternion.identity,Owner.transform);
            base.OnUse();
        }
    }
}
