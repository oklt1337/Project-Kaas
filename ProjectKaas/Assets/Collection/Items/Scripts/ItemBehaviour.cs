using Photon.Pun;
using UnityEngine;

namespace Collection.Items.Scripts
{
    public abstract class ItemBehaviour : MonoBehaviourPun
    {
        protected GameObject Owner;
        
        private void Start()
        {
            Owner = transform.parent.gameObject;
        }

        /// <summary>
        /// What the Item does.
        /// </summary>
        public virtual void OnUse()
        {
    
        }
    }
}
