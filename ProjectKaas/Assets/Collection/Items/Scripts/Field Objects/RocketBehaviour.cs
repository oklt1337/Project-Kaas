using System;
using Collection.NetworkPlayer.Scripts;
using Photon.Pun;
using UnityEngine;

namespace Collection.Items.Scripts.Field_Objects
{
    public class RocketBehaviour : MonoBehaviour
    {
        [SerializeField] private float speed;
        [SerializeField] private Vector3 flyVector;

        [Header("Audio stuff")] 
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip startSound;
        [SerializeField] private AudioClip flySound;
        [SerializeField] private AudioClip onHitSound;

        private void Start()
        {
            audioSource.clip = startSound;
            audioSource.Play();
        }

        public void Update()
        {
            // Looping the fly sound.
            if (!audioSource.isPlaying)
            {
                audioSource.loop = true;
                audioSource.clip = flySound;
                audioSource.Play();
            }
            
            var myTransform = transform;
            var position = myTransform.position;
            position += flyVector * (speed * Time.deltaTime);
            myTransform.position = position;
            
            // the second argument, upwards, defaults to Vector3.up
            var rotation = Quaternion.LookRotation(position, Vector3.up);
            transform.localRotation = Quaternion.Euler(rotation.x + 90 , rotation.y , rotation.z);
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
            hitPlayer.Car.PlayAudioClip(onHitSound);
            PhotonNetwork.Destroy(gameObject);
        }

        private void OnCollisionEnter(Collision other)
        {
            PhotonNetwork.Destroy(gameObject);
        }
    }
}
