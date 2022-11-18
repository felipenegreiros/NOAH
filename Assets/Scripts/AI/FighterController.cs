using System;
using Assets.Scripts.A.I_test;
using Control;
using UnityEngine;

namespace AI
{
    public class FighterController : MonoBehaviour
    {
        [SerializeField] private float attackingRange = 2.16f;
        [SerializeField] private float timeBetweenAttacks = 1f;
        private GameObject playerGameObject;
        private Animator _animatorComponent;
        public bool isPlayerInRange;
        public bool isReadyToAttack;
        public float timeSinceLastAttack;
        private Aggro _aggroComponent;
        private static readonly int Attack = Animator.StringToHash("Attack");

        private void Awake()
        {
            _aggroComponent = GetComponent<Aggro>();
            _animatorComponent = GetComponent<Animator>();
        }
        
        private void Start()
        {
            playerGameObject = _aggroComponent.GetPlayerGameObject();
        }
        
        private void Update()
        {
            UpdateTime();
            AttackBehaviour();
        }

        private void AttackBehaviour()
        {
            var canAttack = AnalyzeAttackConditions();
            if (canAttack)
            {
                timeSinceLastAttack = 0f;
                _animatorComponent.SetBool(Attack, true);
            }
            else
            {
                _animatorComponent.SetBool(Attack, false);
            }
        }

        private bool AnalyzeAttackConditions()
        {
            isReadyToAttack = timeSinceLastAttack >= timeBetweenAttacks;
            isPlayerInRange = IsPlayerInAttackRange();
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
