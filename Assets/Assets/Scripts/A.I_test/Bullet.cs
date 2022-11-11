using System;
using UnityEngine;

namespace Assets.Scripts.A.I_test
{
    public class Bullet : MonoBehaviour
    {
        public float timeToLive = 5f;
        public GameObject playerGameObject;
        public GameObject originalInstigator;
        private Rigidbody _rigidBodyComponent;
        [SerializeField] private float horizontalSpeed = 4f;
        [SerializeField] private float verticalSpeed = -0.1f;
        
        // Start is called before the first frame update
        private void Awake()
        {
            playerGameObject = GameObject.FindWithTag("Player");
            _rigidBodyComponent = GetComponent<Rigidbody>();
            Destroy(gameObject, timeToLive);
        }

        // Update is called once per frame
        private void Update()
        {
        
        }
        
        public void SetOriginalInstigator(GameObject instigator)
        {
            originalInstigator = instigator;
        }
        
        public void Push(Transform desiredTransform)
        {
            var horizontalForce = desiredTransform.forward * horizontalSpeed;
            var verticalForce = desiredTransform.up * verticalSpeed;
            verticalForce.x = 0;
            verticalForce.z = 0;
            _rigidBodyComponent.AddForce(horizontalForce, ForceMode.Impulse);
            _rigidBodyComponent.AddForce(verticalForce, ForceMode.Impulse);
        }

        public void OnTriggerEnter(Collider other)
        {
            if(other.gameObject != playerGameObject)
            {
                return;
            }
            var healthComponent = other.gameObject.GetComponent<Health>();
            healthComponent.TakeDamage(originalInstigator, 10);
            Destroy(gameObject);
        }
    }
}
