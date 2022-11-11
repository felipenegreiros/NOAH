using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.A.I_test
{
    public class MaskAI : MonoBehaviour
    {
        public GameObject player;
        public NavMeshAgent agent;
        public GameObject projectile;
        public float timeSinceLastAttack = 0;

        private void Awake()
        {
            player = GameObject.FindGameObjectWithTag("Player");
            agent = GetComponent<NavMeshAgent>();
        }

        private void Update()
        {
            timeSinceLastAttack += Time.deltaTime;
        }

        public void AggressiveBehavior()
        {
            LookAtPlayer();
            Shoot();
        }

        private void LookAtPlayer()
        {
            transform.LookAt(player.transform);
        }

        private void Shoot()
        {
            if(timeSinceLastAttack <= 1)
            {
                return;
            }
            var rb = Instantiate(projectile, transform.position, transform.rotation).GetComponent<Rigidbody>();
            rb.AddForce(transform.forward*4f, ForceMode.Impulse);
            rb.AddForce(transform.up*0.4f, ForceMode.Impulse);
            timeSinceLastAttack = 0;
            
        }
        // Start is called before the first frame update
        void Start()
        {
        
        }
    }
}
