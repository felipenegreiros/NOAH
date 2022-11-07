using System;
using UnityEngine;

namespace Assets.Scripts.A.I_test
{
    public class Shooter: MonoBehaviour
    {
        public UnityEngine.AI.NavMeshAgent agent;
        public Transform playerTransform;
        [SerializeField] private Animator animatorComponent;
        public LayerMask whatIsGround, whatIsPlayer;
        public float sightRange, attackRange;
        public Collider[] collisionList;
        public bool playerInSightRange, playerInAttackRange;

        private void Awake()
        {
            playerTransform = GameObject.Find("Noah").transform;
            agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
            animatorComponent.SetBool("Walk2", true);
        }

        private void Update()
        {
            collisionList = Physics.OverlapSphere(transform.position, sightRange);
            foreach (var colliderObject in collisionList)
            {
                continue;
            }
        }
    }
}
