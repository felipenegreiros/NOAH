using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.A.I_test;
using UnityEngine;
using UnityEngine.Serialization;

public class CharacScript : MonoBehaviour
{
    [FormerlySerializedAs("anim")] [SerializeField] private Animator animatorComponent;
    private CharacterController _charController;

    public float inX;
    public float inZ;
    [FormerlySerializedAs("kicktime")] public float kickTime = 0;
    [FormerlySerializedAs("punchtime")] public float punchTime = 0;
    private Vector3 _vmovement;
    private Vector3 _vvelocity;
    public float moveSpeed;
    private float _gravidade;
    private int _tempo;
    private bool _kick;

    [SerializeField] private float forceMagnitude;
    
    [SerializeField] public Collider mainCollider;
    [SerializeField] public Collider feetCollider;
    [SerializeField] public Collider handCollider;

    [SerializeField] private GameObject leg;
    [SerializeField] private GameObject hand;
    [SerializeField] private GameObject thisGuyRig;
    [SerializeField] private GameObject bullet;
    [SerializeField] private GameObject sword;
    [SerializeField] private GameObject ps;
    [SerializeField] private Transform bulletPoint;

    public KeyCode upKey = KeyCode.W;
    public KeyCode downKey = KeyCode.S;
    public KeyCode leftKey = KeyCode.A;
    public KeyCode rightKey = KeyCode.D;

    public KeyCode kickKey = KeyCode.K;
    public KeyCode runKey = KeyCode.M;
    public KeyCode punchKey = KeyCode.H;

    private float _hits = 0;
    public Vector3 characPosition;
    public Quaternion characRotation;

    private Rigidbody _rigidBodyComponent;
    private Rigidbody[] _ragRigid;
    private Collider[] _ragCollider;

    private void Start() {
        _rigidBodyComponent = GetComponent<Rigidbody>();
        GetRagDollReferenceBits();
        DisableRagDoll();

        var playerObject = GameObject.FindGameObjectWithTag("Player");
        _charController = playerObject.GetComponent<CharacterController>();
        animatorComponent = playerObject.GetComponent<Animator>();
        
        moveSpeed = 16f;
        _gravidade = 0.5f;

        animatorComponent.SetBool("kick", false);
        animatorComponent.SetBool("kick2", true);
    }

    private void GetRagDollReferenceBits() {
        _ragRigid = thisGuyRig.GetComponentsInChildren<Rigidbody>();
        _ragCollider = thisGuyRig.GetComponentsInChildren<Collider>();
    }

    private void DisableRagDoll() {
        foreach (var col in _ragCollider) {
            col.enabled = false;
        }
        
        foreach (var rag in _ragRigid) {
            rag.isKinematic = true;
        }
        
        animatorComponent.enabled = true;
        mainCollider.enabled = true;
        _rigidBodyComponent.isKinematic = false;
    }

    private void EnableFeet() {
        leg.tag = "Golpe";
    }

    private void DisableFeet() {
        feetCollider.enabled = false;
    }

    private void EnableHand() {
        hand.tag = "Golpe";
    }

    private void RagDollOn() {
        animatorComponent.enabled = false;

        foreach (var col in _ragCollider) {
            col.enabled = true;
        }
        
        foreach (var rag in _ragRigid) {
            rag.isKinematic = false;
        }

        mainCollider.enabled = false;
        _rigidBodyComponent.isKinematic = true;
        _hits = 0;
    }
       private void OnCollisionEnter(Collision collision) {

        if (collision.gameObject.name == "Rosto_AI") {
            RagDollOn();
            Debug.Log("pow");
        }
        
        if (collision.gameObject.CompareTag("balah")) {
            _hits++;
            animatorComponent.SetBool("hit", true);
            Invoke("HitFalse", 0.2f);
            Debug.Log("pow");
        }
        else {
           // Maincollider.enabled = true;
           // Invoke("disablebox", 0.7f);
        }
       }

       private void HitFalse() {
        animatorComponent.SetBool("hit", false);
    }

    private void DisableBox() {
       // boxcollider.enabled = false;
    }
    // Update is called once per frame
    public void Update() 
    {
        if (_hits > 4) {
            RagDollOn();
        }
        
        if (Input.GetKey(upKey)) {
            DisableRagDoll();
        }

        inX = Input.GetAxis("Horizontal");
        inZ = Input.GetAxis("Vertical");

        IncrementTempo();

        //CORRER
        if (Input.GetKey(runKey)) {
            animatorComponent.SetBool("run", true);
            animatorComponent.SetBool("run2", false);
            kickTime = 0;
            inX = inX * 100;
        }
        
        if (Input.GetKeyUp(runKey)) {
            animatorComponent.SetBool("run", false);
            animatorComponent.SetBool("run2", true);
        }
        
        //CHUTE
        if (Input.GetKey(kickKey)) {
            kickTime = 100;
            animatorComponent.SetBool("run", false);
            animatorComponent.SetBool("run2", true);
            animatorComponent.SetBool("kick", true);
            animatorComponent.SetBool("lbool", false);
            animatorComponent.SetBool("rbool", false);
            _kick = true;
        }
        else {
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

        //WALK
        if (Input.GetKey(upKey)) {
            animatorComponent.SetBool("bool1", true);
            animatorComponent.SetBool("bool2", true);
            inX = inX * 100;
           // tempo = 0;
        }

        if (Input.GetKeyUp(upKey)) {
           // anim.SetBool("run", false);
           // anim.SetBool("run2", true);
           animatorComponent.SetBool("bool1", false);
            animatorComponent.SetBool("bool2", false);
            _tempo = 0;
        }
        
        if (Input.GetKey(leftKey)) {
            animatorComponent.SetBool("lbool", true);
            animatorComponent.SetBool("lbool2", false);
        } else {
            animatorComponent.SetBool("lbool", false);
            animatorComponent.SetBool("lbool2", true);
        }
        
        if (Input.GetKey(rightKey)) {
            animatorComponent.SetBool("rbool", true);
            animatorComponent.SetBool("rbool2", false);
        }
        else {
            animatorComponent.SetBool("rbool", false);
            animatorComponent.SetBool("rbool2", true);  
        }
        
        if (_tempo > 160) {
           // anim.SetBool("run", true);
           // anim.SetBool("run2", false);
        }

        //AGACHAR
        if (Input.GetKeyDown(downKey)) {
            animatorComponent.SetBool("Agacha", true);
            mainCollider.enabled = false;
        }
        
        if (Input.GetKeyUp(downKey)) {
            animatorComponent.SetBool("Agacha", false);
            mainCollider.enabled = true;
        }
    }

    private void SetTransform(Transform oldTransform)
    {
        var transform1 = transform;
        oldTransform.position = transform1.position;
        oldTransform.rotation = transform1.rotation;
        oldTransform.localScale = transform1.localScale;
    }
    
    private void FixedUpdate() {
        //o problema da tremedeira eh o collider e os comandos uparrow com leftright arrow quando executados juntos

        var currentTransform = _charController.transform.forward;
        var movementValueIncrease = inZ * (2f * Time.deltaTime);
        _vmovement = currentTransform * movementValueIncrease;
        
        var rotateValueIncrease = inX * 2f * Time.deltaTime * Vector3.up;
        _charController.transform.Rotate(rotateValueIncrease);

        if (!Input.GetKeyDown(upKey) && !Input.GetKeyDown(runKey)) return;
        var moveIncrease = moveSpeed * Time.deltaTime * _vmovement;
        _charController.Move(moveIncrease);
        _tempo = 0;
    }
    
    private void IncrementTempo() {
        _tempo++;
    }
    
    private void OnControllerColliderHit(ControllerColliderHit hit) {
        var rigidBody = hit.collider.attachedRigidbody;
        if (rigidBody == null) return;
        var transformPosition = transform.position;
        var forceDirection = hit.gameObject.transform.position - transformPosition;
        forceDirection.y = 0;
        forceDirection.Normalize();
        rigidBody.AddForceAtPosition(forceDirection * forceMagnitude, transformPosition, ForceMode.Impulse);
    }

}
