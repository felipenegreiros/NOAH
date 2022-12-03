using System;
using Assets.Scripts.A.I_test;
using Control;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

namespace AI
{
    public class PatrolController : MonoBehaviour
    {
        [SerializeField] private PatrolPath patrolPath;
        [SerializeField] private float wayPointTolerance = 0.1f;
        [SerializeField] private float patrolSpeed = 5.48f;
        [SerializeField] private float chaseSpeed = 2.5f;
        [SerializeField] private float dwellingTime = 5f;
        private Aggro _aggroComponent;
        private NavMeshAgent _navMeshComponent;
        private Animator _animatorComponent;
        private FighterController _fighterControllerComponent;
        private int _currentWayPointIndex;
        private Vector3 currentWayPoint;
        private bool _arrivedAtWayPoint;
        private float _timeSinceArrivedAtWaypoint;
        private float distanceToPlayer;
        private float _timeSinceLastSawPlayer;
        public GameObject playerGameObject;
        private static readonly int Walk = Animator.StringToHash("walk");

        private void Awake()
        {
            _aggroComponent = GetComponent<Aggro>();
            _navMeshComponent = GetComponent<NavMeshAgent>();
            _animatorComponent = GetComponent<Animator>();
            _fighterControllerComponent = GetComponent<FighterController>();
            _timeSinceArrivedAtWaypoint = 0;
        }

        private void Start()
        {
            playerGameObject = _aggroComponent.GetPlayerGameObject();
            _animatorComponent.SetBool(Walk, true);
            currentWayPoint = GetCurrentWayPoint();
        }

        private void Update()
        {
            if (_aggroComponent.playerInSightRange)
            {
                _navMeshComponent.speed = chaseSpeed;
                EnableMovement();
                ChasePlayer();
            }
            else
            {
                _navMeshComponent.speed = patrolSpeed;
                PatrolBehaviour();
            }
            UpdateTime();
        }

        private void UpdateTime()
        {
            _timeSinceArrivedAtWaypoint += Time.deltaTime;
        }

        private void ChasePlayer()
        {
            _fighterControllerComponent.ChasePlayer();
        }

        private void PatrolBehaviour()
        {
            if (patrolPath is null) { return; }
            _arrivedAtWayPoint = AtWayPoint();
            if (_arrivedAtWayPoint)
            {
                _timeSinceArrivedAtWaypoint = 0;
                CycleWayPoint();
            }
            if (_timeSinceArrivedAtWaypoint > dwellingTime)
            {
                EnableMovement();
                MoveTowardsWaypoint();
            }
            else
            {
                DisableMovement();
                // LookTowardsWayPoint();
            }
        }

        private void DisableMovement()
        {
            _animatorComponent.SetBool(Walk, false);
            _navMeshComponent.isStopped = true;
        }

        private void EnableMovement()
        {
            _animatorComponent.SetBool(Walk, true);
            _navMeshComponent.isStopped = false;
        }

        private void MoveTowardsWaypoint()
        {
            var wayPoint = GetCurrentWayPoint();
            _navMeshComponent.SetDestination(wayPoint);
        }

        private void LookTowardsWayPoint()
        {
            var direction = currentWayPoint - transform.position;
            var toRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, 1f);
        }

        private bool AtWayPoint()
        {
            var distanceToWayPoint = Vector3.Distance(transform.position, currentWayPoint);
            return distanceToWayPoint < wayPointTolerance;
        }
        
        private Vector3 GetCurrentWayPoint()
        {
            return patrolPath.GetWaypoint(_currentWayPointIndex);
        }
        
        private void CycleWayPoint()
        {
            _currentWayPointIndex = patrolPath.GetNextIndex(_currentWayPointIndex);
            var newWayPoint = GetCurrentWayPoint();
            currentWayPoint = newWayPoint;
        }
    }
}
