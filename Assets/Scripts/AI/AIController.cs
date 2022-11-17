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
        private Aggro _aggroComponent;
        private NavMeshAgent _navMeshComponent;
        private int _currentWayPointIndex;
        public float timeSinceArrivedAtWaypoint;
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
        }

        private void Update()
        {
            UpdateTime();
            PatrolBehaviour();
        }
        
        private void UpdateTime()
        {
            timeSinceArrivedAtWaypoint += Time.deltaTime;
        }

        private void UpdateWayPointTime()
        {
            if (timeSinceArrivedAtWaypoint >= 1)
            {
                timeSinceArrivedAtWaypoint = 0;
                CycleWayPoint();
            }
        }

        private void MoveTowardsPlayer()
        {
            _navMeshComponent.SetDestination(playerGameObject.transform.position);
        }

        private void PatrolBehaviour()
        {
            if (patrolPath is null) { return; }
            var arrivedAtWayPoint = AtWayPoint();
            if (arrivedAtWayPoint)
            {
                UpdateWayPointTime();
                CycleWayPoint();
            }
            MoveTowardsWaypoint();
        }
        
        private void MoveTowardsWaypoint()
        {
            var wayPoint = GetCurrentWayPoint();
            _navMeshComponent.SetDestination(wayPoint);
        }

        private bool AtWayPoint()
        {
            var distanceToWaypoint = Vector3.Distance(transform.position, GetCurrentWayPoint());
            return distanceToWaypoint < wayPointTolerance;
        }
        
        private Vector3 GetCurrentWayPoint()
        {
            return patrolPath.GetWaypoint(_currentWayPointIndex);
        }
        
        private void CycleWayPoint()
        {
            _currentWayPointIndex = patrolPath.GetNextIndex(_currentWayPointIndex);
        }
    }
}
