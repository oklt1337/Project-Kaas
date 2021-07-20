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
        
        public void FixedUpdate()
        {
            transform.position += Vector3.forward * (speed * Time.deltaTime);
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
