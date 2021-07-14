using Collection.NetworkPlayer.Scripts;
using Photon.Pun;
using UnityEngine;

namespace Collection.Items.Scripts
{
    public abstract class ItemBehaviour : MonoBehaviourPun
    {
        protected PlayerHandler Owner;

        public Sprite itemSprite;
        
        private void Start()
        {
            Owner = gameObject.GetComponentInParent<PlayerHandler>();
        }

        /// <summary>
        /// What the Item does.
        /// </summary>
        public virtual void OnUse()
        {
    
        }
    }
}
