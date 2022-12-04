using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;


public class FECharacScript : MonoBehaviour
{
    [SerializeField] private Animator animatorComponent;
    private Transform characterTransform;
    private Rigidbody rigidBodyComponent;
    
    public KeyCode upKey = KeyCode.W;
    public KeyCode downKey = KeyCode.S;
    public KeyCode leftKey = KeyCode.A;
    public KeyCode rightKey = KeyCode.D;

    public KeyCode kickKey = KeyCode.K;
    public KeyCode runKey = KeyCode.M;
    public KeyCode punchKey = KeyCode.H;
    public KeyCode shootKey = KeyCode.R;

    private float inX;
    private float inZ;
    private float kickTime = 0;
    private float punchTime = 0;
    private Vector3 verticalMovement;
    private Vector3 verticalVelocity;
    public float moveSpeed;
    private int tempo;

    [SerializeField] private float forceMagnitude;
    
    [SerializeField] private CapsuleCollider mainCollider;
    [SerializeField] private Collider feetCollider;
    [SerializeField] private BoxCollider handCollider;
    [SerializeField] private Rigidbody handRig;

    [SerializeField] private GameObject leg;
    [SerializeField] private GameObject hand;
    [SerializeField] private GameObject thisGuyRig;
    [SerializeField] private GameObject bullet;
    [SerializeField] private GameObject sword;
    [SerializeField] private GameObject ps;
    [SerializeField] private Transform bulletPoint;
    private GameObject playerGameObject;
    private float hits = 0;

    private void Start()
    {
        playerGameObject = GameObject.FindGameObjectWithTag("Player");
        characterTransform = playerGameObject.GetComponent<Transform>();
        animatorComponent = playerGameObject.GetComponent<Animator>();
        rigidBodyComponent = playerGameObject.GetComponent<Rigidbody>();
        GetRagDollBits();
        RagdollOff();
        moveSpeed = 4f;
        animatorComponent.SetBool(Kick, false);
        animatorComponent.SetBool(Kick2, true);
    }

    private Rigidbody[] ragRigid;
    private Collider[] ragCollider;
    private static readonly int Kick = Animator.StringToHash("kick");
    private static readonly int Kick2 = Animator.StringToHash("kick2");
    private static readonly int Run = Animator.StringToHash("run");
    private static readonly int Run2 = Animator.StringToHash("run2");
    private static readonly int LeftBool = Animator.StringToHash("lbool");
    private static readonly int RightBool = Animator.StringToHash("rbool");
    private static readonly int Punch = Animator.StringToHash("punch");
    private static readonly int Shoot1 = Animator.StringToHash("shoot");
    private static readonly int Bool1 = Animator.StringToHash("bool1");
    private static readonly int Bool2 = Animator.StringToHash("bool2");
    private static readonly int LeftBoolB = Animator.StringToHash("lbool2");
    private static readonly int RightBoolB = Animator.StringToHash("rbool2");
    private static readonly int Crouch = Animator.StringToHash("Agacha");
    private static readonly int Hit = Animator.StringToHash("hit");

    private void GetRagDollBits()
    {
        ragRigid = thisGuyRig.GetComponentsInChildren<Rigidbody>();
        ragCollider = thisGuyRig.GetComponentsInChildren<Collider>();
    }

    private void RagdollOff()
    {
        foreach (var playerCollider in ragCollider)
        {
            playerCollider.enabled = false;
        }
        foreach (var playerRag in ragRigid)
        {
            playerRag.isKinematic = true;
        }
        animatorComponent.enabled = true;
        mainCollider.enabled = true;
        rigidBodyComponent.isKinematic = false;
        handRig.isKinematic = false;
    }

    private void EnableFeet()
    {
       // pecollider.enabled = true;
        leg.tag = "Golpe";
        feetCollider.enabled = true;
    }

    private void DisableFeet()
    {
        feetCollider.enabled = false;
        leg.tag = "Untagged";
    }

    private void EnableHand()
    {
        hand.tag = "Golpe";
        handCollider.enabled = true;
      
    }

    private void DisableHand()
    {
        hand.tag = "Untagged";
        handCollider.enabled = false;

    }

    private void EnableSword()
    {
        handCollider.center = new Vector3(-2.5f, 0, 0);
        handCollider.size = new Vector3(6, 0.5f, 1);

        sword.SetActive(true);
    }

    private void EnableRagDoll()
    {
        animatorComponent.enabled = false;

        foreach (var col in ragCollider)
        {
            col.enabled = true;
        }
        foreach (var rag in ragRigid)
        {
            rag.isKinematic = false;
        }

        mainCollider.enabled = false;
        rigidBodyComponent.isKinematic = true;
        hits = 0;
    }
       private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.name == "Rosto_AI")
        {
            EnableRagDoll();
        }
        if (collision.gameObject.CompareTag("balah"))
        {
            DisableHand();
            DisableFeet();
            hits++;
            animatorComponent.SetBool(Hit, true);
            Invoke("HitFalse", 0.2f);
        }

    }

       private void HitFalse()
    {
        animatorComponent.SetBool(Hit, false);
    }
       
    // Update is called once per frame
    private void Update()
    {
        if (hits > 4)
        {
            EnableRagDoll();
        }

        if (Input.GetKey(upKey))
        {
            RagdollOff();
        }

        inX = Input.GetAxis("Horizontal");
        inZ = Input.GetAxis("Vertical");

        IncrementTempo();

        // CORRER
        RunMechanics();
        
        // CHUTE
        KickMechanics();

        // PUNCH
        PunchMechanics();

        // WALK
        WalkMechanics();

        // AGACHAR
        CrouchMechanics();

        // ATIRAR
        ShootingMechanics();
    }

    private void RunMechanics()
    {
        if (Input.GetKey(runKey))
        {
            animatorComponent.SetBool(Run, true);
            animatorComponent.SetBool(Run2, false);
            kickTime = 0;
            // inX = inX * 100;

            // Debug.Log("colon");
        }

        if (Input.GetKeyUp(runKey))
        {
            animatorComponent.SetBool(Run, false);
            animatorComponent.SetBool(Run2, true);
        }
    }

    private void KickMechanics()
    {
        if (Input.GetKey(kickKey))
        {
            // inX = inX * 100;
        }

        if (Input.GetKeyDown(kickKey))
        {
            kickTime = 100;
            animatorComponent.SetBool(Run, false);
            animatorComponent.SetBool(Run2, true);
            animatorComponent.SetBool(Kick, true);
            animatorComponent.SetBool(LeftBool, false);
            animatorComponent.SetBool(RightBool, false);
        }
        else
        {
            kickTime = kickTime - 5f;
        }

        if (Input.GetKeyUp(kickKey))
        {
            animatorComponent.SetBool(Kick, false);
        }

        if (kickTime < 0)
        {
            kickTime = 0;
        }

        if (kickTime > 20)
        {
        }
    }

    private void PunchMechanics()
    {
        if (Input.GetKeyDown(punchKey))
        {
            punchTime = 100;
            animatorComponent.SetBool(Run, false);
            animatorComponent.SetBool(Run2, true);
            animatorComponent.SetBool(Punch, true);
            animatorComponent.SetBool(LeftBool, false);
            animatorComponent.SetBool(RightBool, false);
            handRig.isKinematic = false;
        }
        else
        {
            punchTime = punchTime - 2f;
        }

        if (Input.GetKeyUp(punchKey))
        {
            animatorComponent.SetBool(Punch, false);
        }

        if (punchTime < 0)
        {
            punchTime = 0;
        }

        if (punchTime > 5)
        {
            handRig.isKinematic = false;
        }
        else
        {
            handCollider.enabled = false;
        }
    }

    private void WalkMechanics()
    {
        if (Input.GetKey(upKey))
        {
            animatorComponent.SetBool(Bool1, true);
            animatorComponent.SetBool(Bool2, true);
        }

        if (Input.GetKeyUp(upKey))
        {
            animatorComponent.SetBool(Bool1, false);
            animatorComponent.SetBool(Bool2, false);
            tempo = 0;
        }

        if (Input.GetKey(leftKey))
        {
            animatorComponent.SetBool(LeftBool, true);
            animatorComponent.SetBool(LeftBoolB, false);
            inX = -inX * -100;
        }
        else
        {
            animatorComponent.SetBool(LeftBool, false);
            animatorComponent.SetBool(LeftBoolB, true);
        }

        if (Input.GetKey(rightKey))
        {
            animatorComponent.SetBool(RightBool, true);
            animatorComponent.SetBool(RightBoolB, false);
            inX = inX * 100;
        }
        else
        {
            animatorComponent.SetBool(RightBool, false);
            animatorComponent.SetBool(RightBoolB, true);
        }

        if (tempo > 160)
        {
        }
    }

    private void ShootingMechanics()
    {
        if (Input.GetKeyUp(shootKey))
        {
            animatorComponent.SetBool(Run, false);
            animatorComponent.SetBool(Run2, true);
            animatorComponent.SetBool(Shoot1, true);
            animatorComponent.SetBool(Bool1, false);
            animatorComponent.SetBool(Bool2, false);
            Invoke("Shoot", 0.3f);
        }

        if (Input.GetKeyUp(shootKey))
        {
            animatorComponent.SetBool(Shoot1, false);
        }

        if (Input.GetKeyUp(shootKey))
        {
        }

        //Debug.Log(inX);
        if (inX is > 100 or < -100)
        {
            inX = 100;
        }
    }

    private void CrouchMechanics()
    {
        if (Input.GetKeyDown(downKey))
        {
            animatorComponent.SetBool(Crouch, true);
            // Maincollider.enabled = false;
            mainCollider.height = 6;
            mainCollider.center = new Vector3(0, 3, 0);
        }

        if (Input.GetKeyUp(downKey))
        {
            animatorComponent.SetBool(Crouch, false);
            // Maincollider.enabled = true;
            mainCollider.height = 13;
            mainCollider.center = new Vector3(0, 7, 0);
        }

        if (Input.GetKey(downKey))
        {
        }
        else
        {
            animatorComponent.SetBool(Crouch, false);
        }

        if (Input.GetKey(downKey))
        {
            EnableSword();
            handCollider.enabled = true;
        }
    }

    private void Shoot()
    {
        if (!animatorComponent.GetBool(Shoot1)) return;
        var bulletInstance = Instantiate(bullet, bulletPoint.position, Quaternion.identity);
        var bulletRigidBody = bulletInstance.GetComponent<Rigidbody>();
        bulletRigidBody.AddForce(transform.forward * 4f, ForceMode.Impulse);
        bulletRigidBody.AddForce(transform.up * -0.1f, ForceMode.Impulse);
    }
    private void FixedUpdate()
    {
        verticalMovement = characterTransform.transform.forward * inZ *(1.5f * Time.deltaTime);
        characterTransform.transform.Rotate(Vector3.up * inX * (1.5f * Time.deltaTime));
        if (!Input.GetKeyDown(KeyCode.UpArrow) && !Input.GetKeyDown(KeyCode.M)) return;
        rigidBodyComponent.AddForce(verticalMovement * moveSpeed * Time.deltaTime);
        tempo = 0;

    }

    private void IncrementTempo()
    {
        tempo++;
    }
    
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        var rigidBody = hit.collider.attachedRigidbody;
        if (rigidBody == null) return;
        var forceDirection = hit.gameObject.transform.position - transform.position;
        forceDirection.y = 0;
        forceDirection.Normalize();
        rigidBody.AddForceAtPosition(forceDirection * forceMagnitude, transform.position, ForceMode.Impulse);
    }
}
