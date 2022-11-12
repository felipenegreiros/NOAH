using UnityEngine;

namespace Core
{
    public class PlayerAttacks : MonoBehaviour
    {
        public KeyCode kickKey = KeyCode.K;
        public KeyCode punchKey = KeyCode.H;
        
        [SerializeField] private Animator animatorComponent;
        [SerializeField] public Collider mainCollider;
        [SerializeField] public Collider feetCollider;
        [SerializeField] public Collider handCollider;
        [SerializeField] private GameObject leg;
        [SerializeField] private GameObject hand;
        [SerializeField] private GameObject thisGuyRig;
        
        private float kickTime = 0;
        public float punchTime = 0;
        private bool _kick;
        private Rigidbody _rigidBodyComponent;
        private Rigidbody[] _ragRigid;
        private Collider[] _ragCollider;
        
        // Start is called before the first frame update
        void Start()
        {
        
        }

        private void KickMechanics()
        {
            if (Input.GetKey(kickKey)) {
                kickTime = 100;
                animatorComponent.SetBool("run", false);
                animatorComponent.SetBool("run2", true);
                animatorComponent.SetBool("kick", true);
                animatorComponent.SetBool("lbool", false);
                animatorComponent.SetBool("rbool", false);
                _kick = true;
            } else {
                kickTime = kickTime - 5f;
                //isso aq ta so no getkey ai se o cara pressionar nunca vai descer
            }
            
            if (Input.GetKeyUp(kickKey)) {
                animatorComponent.SetBool("kick", false);
                _kick = false;
            }

            if (kickTime < 0) {
                kickTime = 0;
            }
            if (kickTime > 20 ) {
                Invoke("EnableFeet", 0.8f);
                feetCollider.enabled = true;
                // perna.tag = "balah";
            } else {
                leg.tag = "Untagged";
            }
        }

        private void PunchMechanics()
        {
            //PUNCH
            if (Input.GetKey(punchKey)) {
                punchTime = 100;
                animatorComponent.SetBool("run", false);
                animatorComponent.SetBool("run2", true);
                animatorComponent.SetBool("punch", true);
                animatorComponent.SetBool("lbool", false);
                animatorComponent.SetBool("rbool", false);
            }
            else {
                punchTime = punchTime - 4f;
                //isso aq ta so no getkey ai se o cara pressionar nunca vai descer
            }

            if (Input.GetKeyUp(punchKey)) {
                animatorComponent.SetBool("punch", false);
            }

            if (punchTime < 0) {
                punchTime = 0;
            }
        
            if (punchTime > 10) {
                Invoke("EnableHand", 0.4f);
                handCollider.enabled = true;
                // perna.tag = "balah";
            } else {
                hand.tag = "Untagged";
            }
        }
        
        

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
