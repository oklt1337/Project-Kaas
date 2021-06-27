using UnityEngine;

namespace Collection.Items.Scripts
{
    public abstract class ItemBehaviour : MonoBehaviour
    {
        protected GameObject Owner;
        [SerializeField] protected GameObject onFieldItem;
        
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
