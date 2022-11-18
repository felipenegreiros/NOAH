using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Control
{
    public class Aggro : MonoBehaviour
    {
        public GameObject playerObject;
        public bool playerInSightRange;
        public float timeSinceLastSawPlayer;
        public GameObject objectCenter;
        public Collider[] collisionList;
        [SerializeField] public float sightRange = 0.5f;
        [Serializable]
        public class AggroEvent : UnityEvent<float>
        {
            //
        }
        [SerializeField] public AggroEvent aggroTrigger;

        private void Awake()
        {
            playerObject = GameObject.FindGameObjectWithTag("Player");
            objectCenter = FindGameObjectInChildWithTag(gameObject, "Center");
            if (!objectCenter)
            {
                objectCenter= gameObject;
            }
        }

        public GameObject GetPlayerGameObject()
        {
            return playerObject;
        }
        
        private static GameObject FindGameObjectInChildWithTag (GameObject parent, string tag)
        {
            var t = parent.transform;
 
            for (var i = 0; i < t.childCount; i++) 
            {
                if(t.GetChild(i).gameObject.CompareTag(tag))
                {
                    return t.GetChild(i).gameObject;
                }
                 
            }
             
            return null;
        }

        // Update is called once per frame
        private void Update()
        {
            playerInSightRange = IsPlayerInRange();
            if (!playerInSightRange)
            {
                timeSinceLastSawPlayer += Time.deltaTime;
            }
            else
            {
                timeSinceLastSawPlayer = 0;
            }
        }

        private bool IsPlayerInRange()
        {
            var sphereCenter = objectCenter.transform.position;
            collisionList = Physics.OverlapSphere(sphereCenter, sightRange);
            var result = collisionList.Any(colliderObject => colliderObject.CompareTag("Player"));
            if (result)
            {
                aggroTrigger.Invoke(5f);
            }
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
