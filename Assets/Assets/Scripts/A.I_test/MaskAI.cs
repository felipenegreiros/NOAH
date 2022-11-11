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
        public Quaternion desiredAngle;

        private void Awake()
        {
            player = GameObject.FindGameObjectWithTag("Player");
            agent = GetComponent<NavMeshAgent>();
        }

        private void Update()
        {
            timeSinceLastAttack += Time.deltaTime;
        }

        private Quaternion GetHorizontalAngleBetweenShooterAndPlayer()
        {
            var playerPosition = player.transform.position;
            var shooterPosition = transform.position;
            var lookVector = playerPosition - shooterPosition;
            lookVector.y = 0;
            var rotation = Quaternion.LookRotation(lookVector);
            return rotation;
        }

        private void LookAtPlayer()
        {
            // transform.LookAt(player.transform);
            desiredAngle = GetHorizontalAngleBetweenShooterAndPlayer();
            var t = transform;
            t.rotation = Quaternion.Slerp(t.rotation, desiredAngle, Time.deltaTime * 10);
        }

        private void Shoot()
        {
            if(timeSinceLastAttack <= 1)
            {
                return;
            }

            var bullet = Instantiate(projectile, transform.position, transform.rotation);
            var rb = bullet.GetComponent<Rigidbody>();
            rb.AddForce(transform.forward*4f, ForceMode.Impulse);
            rb.AddForce(transform.up*0.4f, ForceMode.Impulse);
            timeSinceLastAttack = 0;
            
        }
        
        public void AggressiveBehavior()
        {
            LookAtPlayer();
            Shoot();
        }
    }
}
