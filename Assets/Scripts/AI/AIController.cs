using System;
using Assets.Scripts.A.I_test;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

namespace AI
{
    public class AIController : MonoBehaviour
    {
        [SerializeField] private PatrolPath patrolPath;
        [SerializeField] private float wayPointTolerance = 0.1f;
        [SerializeField] private float patrolSpeed = 5.48f;
        // [SerializeField] private float chaseSpeed = 7.5f;
        [SerializeField] private float chaseSpeed = 2.5f;
        private Aggro _aggroComponent;
        private NavMeshAgent _navMeshComponent;
        private int _currentWayPointIndex;
        private Vector3 currentWayPoint;
        private bool _arrivedAtWayPoint;
        private float _timeSinceArrivedAtWaypoint;
        private float _timeSinceLastSawPlayer;
        public GameObject playerGameObject;

        private void Awake()
        {
            _aggroComponent = GetComponent<Aggro>();
            _navMeshComponent = GetComponent<NavMeshAgent>();
        }

        private void Start()
        {
            playerGameObject = _aggroComponent.GetPlayerGameObject();
            currentWayPoint = GetCurrentWayPoint();
        }

        private void Update()
        {
            UpdateTime();
            if (_aggroComponent.playerInSightRange)
            {
                _navMeshComponent.speed = chaseSpeed;
                ChasePlayer();
            }
            else
            {
                _navMeshComponent.speed = patrolSpeed;
                PatrolBehaviour();
            }
        }
        
        private void UpdateTime()
        {
            _timeSinceArrivedAtWaypoint += Time.deltaTime;
        }

        private void ChasePlayer()
        {
            _navMeshComponent.SetDestination(playerGameObject.transform.position);
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
            MoveTowardsWaypoint();
            // MoveTowardsPlayer();
        }
        
        private void MoveTowardsWaypoint()
        {
            var wayPoint = GetCurrentWayPoint();
            _navMeshComponent.SetDestination(wayPoint);
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
