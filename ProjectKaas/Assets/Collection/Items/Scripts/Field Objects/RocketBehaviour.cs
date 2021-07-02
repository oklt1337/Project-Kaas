using Collection.NetworkPlayer.Scripts;
using UnityEngine;

namespace Collection.Items.Scripts.Field_Objects
{
    public class RocketBehaviour : MonoBehaviour
    {
        [SerializeField] private float speed;
        
        public void FixedUpdate()
        {
            transform.position += Vector3.forward * (speed * Time.deltaTime);
        }

        private void OnCollisionEnter(Collision other)
        {
            if (!other.gameObject.CompareTag("Player"))
                return;
            
            var hitPlayer = other.gameObject.GetComponent<PlayerHandler>();
            hitPlayer.Car.OnHit();
            
            Destroy(this);
        }
    }
}
