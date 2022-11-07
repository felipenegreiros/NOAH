using UnityEngine;

namespace Assets.Scripts.A.I_test
{
    public class Bullet : MonoBehaviour
    {
        public float timeToLive = 5f;
        private Rigidbody _rigidBodyComponent;
        
        // Start is called before the first frame update
        private void Awake()
        {
            _rigidBodyComponent = GetComponent<Rigidbody>();
            Destroy(gameObject, timeToLive);
        }

        // Update is called once per frame
        private void Update()
        {
        
        }
        public void Push(Transform desiredTransform)
        {
            var horizontalForce = desiredTransform.forward * 4f;
            var verticalForce = desiredTransform.up * -0.1f;
            _rigidBodyComponent.AddForce(horizontalForce, ForceMode.Impulse);
            _rigidBodyComponent.AddForce(verticalForce, ForceMode.Impulse);
        }
    }
}
