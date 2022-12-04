using System;
using System.Collections;
using Assets.Scripts.A.I_test;
using Attributes;
using Control;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

namespace AI
{
    public class FighterController : MonoBehaviour
    {
        [SerializeField] private float attackingRange = 2.16f;
        [SerializeField] private float timeBetweenAttacks = 1f;
        public float timeSinceLastAttack;
        private GameObject playerGameObject;
        private Health playerHealthComponent;
        private Animator _animatorComponent;
        public bool isPlayerInRange;
        public bool isReadyToAttack;
        private float distanceToPlayer;
        private NavMeshAgent _navMeshComponent;
        private Aggro _aggroComponent;
        private static readonly int Attack = Animator.StringToHash("Attack");
        private static readonly int Walk = Animator.StringToHash("walk");
        private static readonly int AttackFromWalk = Animator.StringToHash("attack_from_walk");
        private static readonly int AttackFromIdle = Animator.StringToHash("attack_from_idle");

        private void Awake()
        {
            _aggroComponent = GetComponent<Aggro>();
            _animatorComponent = GetComponent<Animator>();
            _navMeshComponent = GetComponent<NavMeshAgent>();
        }
        
        private void Start()
        {
            playerGameObject = _aggroComponent.GetPlayerGameObject();
            playerHealthComponent = playerGameObject.GetComponent<Health>();
        }
        
        private void Update()
        {
            UpdateTime();
        }

        public void ChasePlayer()
        {
            var playerPosition = playerGameObject.transform.position;
            distanceToPlayer = Vector3.Distance(transform.position, playerPosition);
            if(distanceToPlayer > 1.1f * attackingRange)
            {
                StartWalkingAnimation();
                _navMeshComponent.SetDestination(playerPosition);
            }
            else
            {
                StopWalkingAnimation();
                AttackBehaviour();
            }
        }

        private void StartWalkingAnimation()
        {
            _animatorComponent.SetBool(Walk, true);
        }

        private void StopWalkingAnimation()
        {
            _animatorComponent.SetBool(Walk, false);
            _navMeshComponent.isStopped = true;
            _navMeshComponent.ResetPath();
        }

        private void AttackBehaviour()
        {
            var canAttack = AnalyzeAttackConditions();
            if (canAttack)
            {
                TriggerSingleAttack();
            }

        }

        private void TriggerSingleAttack()
        {
            _animatorComponent.applyRootMotion = false;
            _animatorComponent.SetTrigger(AttackFromWalk);
            _animatorComponent.SetTrigger(AttackFromIdle);
            timeSinceLastAttack = 0;
        }


        private bool AnalyzeAttackConditions()
        {
            isReadyToAttack = timeSinceLastAttack > timeBetweenAttacks;
            isPlayerInRange = distanceToPlayer <= attackingRange;
            if (!isPlayerInRange)
            {
                _animatorComponent.applyRootMotion = true;
            }
            return isReadyToAttack && isPlayerInRange;
        }

        private bool IsPlayerInAttackRange()
        {
            var distance = Vector3.Distance(playerGameObject.transform.position, transform.position);
            return distance <= attackingRange;
        }

        private void UpdateTime()
        {
            timeSinceLastAttack += Time.deltaTime;
        }
        
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            //Use the same vars you use to draw your Overlap Sphere to draw your Wire Sphere.
            Gizmos.DrawWireSphere (transform.position, attackingRange);
        }
    }
}
