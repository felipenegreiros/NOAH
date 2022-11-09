using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.A.I_test
{
    public class Aggro : MonoBehaviour
    {
        public GameObject playerObject;
        public bool playerInSightRange;
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
        }

        private bool IsPlayerInRange()
        {
            collisionList = Physics.OverlapSphere(transform.position, sightRange);
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
            Vector3 target;
            //Use the same vars you use to draw your Overlap Sphere to draw your Wire Sphere.
            if (objectCenter)
            {
                target = objectCenter.transform.position;
            }
            else
            {
                target = transform.position;
            }
            Gizmos.DrawWireSphere (target, sightRange);
        }
    }
}
