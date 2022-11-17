using UnityEngine;
using UnityEngine.Serialization;

namespace AI
{
    public class PatrolPath : MonoBehaviour
    {
        [SerializeField] private float waypointGizmoRadius = 3f;
        
        public int GetNextIndex(int a)
        {
            if (a + 1 == transform.childCount) return 0;
            return a + 1;
        }

        public Vector3 GetWaypoint(int a)
        {
            return transform.GetChild(a).position;
        }

        private void OnDrawGizmos()
        {
            for (var i = 0; i < transform.childCount; i++)
            {
                var j = GetNextIndex(i);
                Gizmos.DrawSphere(GetWaypoint(i), waypointGizmoRadius);
                Gizmos.DrawLine(GetWaypoint(i), GetWaypoint(j));
            }
        }
    }
}
