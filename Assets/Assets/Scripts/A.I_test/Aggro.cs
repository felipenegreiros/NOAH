using System.Linq;
using UnityEngine;

namespace Assets.Scripts.A.I_test
{
    public class Aggro : MonoBehaviour
    {
        public GameObject playerObject;
        public bool playerInSightRange;
        public Collider[] collisionList;
        [SerializeField] public float sightRange = 0.5f;

        private void Awake()
        {
            playerObject = GameObject.FindGameObjectWithTag("Player");
        }
        void Start()
        {
        
        }

        // Update is called once per frame
        private void Update()
        {
            playerInSightRange = IsPlayerInRange();
        }

        private bool IsPlayerInRange()
        {
            collisionList = Physics.OverlapSphere(transform.position, sightRange);
            return collisionList.Any(colliderObject => colliderObject.CompareTag("Player"));
        }
        
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.cyan;
            //Use the same vars you use to draw your Overlap Sphere to draw your Wire Sphere.
            Gizmos.DrawWireSphere (transform.position, sightRange);
        }
    }
}
