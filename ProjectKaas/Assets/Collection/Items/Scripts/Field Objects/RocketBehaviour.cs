using System;
using Collection.NetworkPlayer.Scripts;
using Photon.Pun;
using Unity.VisualScripting;
using UnityEngine;

namespace Collection.Items.Scripts.Field_Objects
{
    public class RocketBehaviour : MonoBehaviour
    {
        [SerializeField] private float speed;
        [SerializeField] private Vector3 flyVector;
        
        public void Update()
        {
            transform.position += flyVector * (speed * Time.deltaTime);
            transform.localRotation = Quaternion.Euler(90,0,0);
        }

        /// <summary>
        /// Sets the vector the Rocket should fly at.
        /// </summary>
        /// <param name="newFlyVector"> The vector you want it to fly like. </param>
        public void SetFlyVector(Vector3 newFlyVector)
        {
            newFlyVector = newFlyVector.normalized;
            flyVector = newFlyVector;
        }
        
        private void OnTriggerEnter(Collider other)
        {
            // Checks tag.
            if (!other.gameObject.CompareTag("Player"))
            {
                return;
            }
            
            // Makes the player tumble on hit.
            var hitPlayer = other.gameObject.GetComponentInParent<PlayerHandler>();
            hitPlayer.Car.OnHit(1f);
            PhotonNetwork.Destroy(gameObject);
        }

        private void OnCollisionEnter(Collision other)
        {
            PhotonNetwork.Destroy(gameObject);
        }
    }
}
