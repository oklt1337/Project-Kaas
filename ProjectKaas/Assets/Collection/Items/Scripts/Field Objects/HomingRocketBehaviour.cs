using Collection.Maps.Scripts;
using Collection.NetworkPlayer.Scripts;
using Photon.Pun;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

namespace Collection.Items.Scripts.Field_Objects
{
    public class HomingRocketBehaviour : MonoBehaviour
    {
        [SerializeField] private GameObject target;
        [SerializeField] private NavMeshAgent agent;

        private void Start()
        {
            agent.updateRotation = true;
        }

        private void Update()
        {
            if(target == null)
                return;
            
            agent.SetDestination(target.transform.position);
            transform.localRotation = Quaternion.Euler(90,0,0);
        }
        
        /// <summary>
        /// Tells the rocket who shoots it to identify the target.  
        /// </summary>
        /// <param name="user"> Who uses it. </param>
        public void AcquireTarget(PlayerHandler user)
        {
            var nextPlayer = PositionManager.PositionManagerInstance.FindNextPlayer(user);
            target = nextPlayer.Car.gameObject;
        }

        private void OnTriggerEnter(Collider other)
        {
            // Checks tag.
            if (!other.gameObject.CompareTag("Player"))
            {
                PhotonNetwork.Destroy(gameObject);
            }
            else
            {
                // Makes the player tumble on hit.
                var hitPlayer = other.gameObject.GetComponentInParent<PlayerHandler>();
                hitPlayer.Car.OnHit(1f);
                PhotonNetwork.Destroy(gameObject);
            }
        }
        
        private void OnCollisionEnter(Collision other)
        {
            PhotonNetwork.Destroy(gameObject);
        }
    }
}
